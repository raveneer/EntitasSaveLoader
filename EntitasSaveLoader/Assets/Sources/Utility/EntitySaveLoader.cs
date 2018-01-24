using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Entitas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class EntitySaveLoader
{
    private readonly Dictionary<string, EntityTemplate> _tempeletDic = new Dictionary<string, EntityTemplate>();

    private ITemplateLoader _templateLoader;
    
    public EntitySaveLoader(ITemplateLoader templateLoader1)
    {
        _templateLoader = templateLoader1;
    }

    private bool _dictionaryReady;

    public void SaveAllEntitiesInScene(Contexts contexts, string saveFileName)
    {
        var savingEntities = contexts.game.GetGroup(GameMatcher.SavingData).GetEntities();
        var saveData = new EntitiesSaveData();
        foreach (var savingEntity in savingEntities)
        {
            //todo : how to save template name here? or not needed?
            saveData.EntityInfos.Add(MakeEntityInfo(savingEntity, null));
        }

        var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        var path = $"Assets/Resources/EntityTemplate/SaveFile/{saveFileName}.json";
        
        File.WriteAllText(path, json);
        Debug.WriteLine("SaveEntitiesInScene done!");
    }

    public void LoadEntitiesFromSaveFile(Contexts contexts, string saveFileName)
    {
        var savedTuple = _templateLoader.LoadSavedEntityFile(saveFileName);
        
        var saveData = JsonConvert.DeserializeObject<EntitiesSaveData>(savedTuple.Item2);
        foreach (var entityInfo in saveData.EntityInfos)
        {
            MakeEntityFromEntityInfo(entityInfo, contexts);
        }

        Debug.WriteLine("LoadEntitiesFromSaveFile done!");
    }
    
    public IEntity MakeEntityFromtemplate(string templateName, Contexts _contexts)
    {
        if (!_dictionaryReady)
        {
            ReLoadTemplets();
        }

        if (!_tempeletDic.ContainsKey(templateName))
        {
            Debug.WriteLine($"can't find name templet: {templateName}");
            return null;
        }

        return MakeEntityFromEntityInfo(_tempeletDic[templateName], _contexts);
    }

    public void ReLoadTemplets()
    {
        _tempeletDic.Clear();
        LoadSingletemplates();
        LoadtemplateGroups();
        _dictionaryReady = true;
        Debug.WriteLine($"ReLoadTemplets done.");
    }

    public IEntity MakeEntityFromJson(string json, Contexts contexts)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.WriteLine("empty json!");
        }

        var entityInfo = JsonConvert.DeserializeObject<EntityTemplate>(json);
        return MakeEntityFromEntityInfo(entityInfo, contexts);
    }

    //todo : support input, ui etc... components
    private IEntity MakeEntityFromEntityInfo(EntityTemplate EntityTemplate, Contexts contexts)
    {
        IEntity newEntity = MakeEntityByContext(EntityTemplate, contexts);
        AddTagComponents(EntityTemplate, newEntity);
        AddComponents(EntityTemplate, newEntity);
        return newEntity;
    }

    public string RemoveComponentSubfix(string nameOfComponent)
    {
        if (nameOfComponent.EndsWith("Component"))
        {
            return nameOfComponent.Remove(nameOfComponent.Length - 9, 9);
        }
            
        return nameOfComponent;
    }

    public bool IsFlagComponent(IComponent component)
    {
        return component.GetType().GetFields().Length == 0;
    }

    /// <summary>
    /// only value or string field have components serialized.
    /// ref type componets ignored.
    /// </summary>
    public string MakeEntityInfoJson(IEntity entity, Formatting jsonFormatting, string templateName)
    {
        EntityTemplate EntityTemplate = MakeEntityInfo(entity, templateName);
        var jsonStr = JsonConvert.SerializeObject(EntityTemplate, jsonFormatting);

        return jsonStr;
    }
    
    public EntityTemplate MakeEntityInfo(IEntity entity, string templateName)
    {
        var entityInfo = new EntityTemplate
        {
            Name = templateName,
            Context = entity.contextInfo.name
        };

        foreach (var component in entity.GetComponents())
        {
            if (!IsHaveIgnoreSaveAttibute(component))
            {
                string componentName = RemoveComponentSubfix(component.GetType().ToString());

                if (IsFlagComponent(component))
                {
                    entityInfo.Tags += componentName +",";
                }
                else
                {
                    entityInfo.Components.Add(componentName, component);
                }
            }
        }
        
        return entityInfo;
    }

    private void LoadSingletemplates()
    {
        var tuples = _templateLoader.LoadSingleTemplateFile();
        foreach (var tuple in tuples)
        {
            var json = tuple.Item2;
            var newtemplate = JsonConvert.DeserializeObject<EntityTemplate>(json);
            Debug.WriteLine(newtemplate.ToString());

            if (_tempeletDic.ContainsKey(newtemplate.Name))
            {
                throw new Exception();
            }

            _tempeletDic.Add(newtemplate.Name, newtemplate);
        }
    }

    private void LoadtemplateGroups()
    {
        var tuples = _templateLoader.LoadGroupTemplateFiles();

        foreach (var tuple in tuples)
        {
            var jObject = JObject.Parse(tuple.Item2);

            foreach (var jPair in jObject)
            {
                var newtemplate = jPair.Value.ToObject<EntityTemplate>();
                Debug.WriteLine(newtemplate.ToString());

                if (_tempeletDic.ContainsKey(jPair.Key))
                {
                    throw new Exception($"already have key {jPair.Key}");
                }

                _tempeletDic.Add(jPair.Key, newtemplate);
            }
        }

        Debug.WriteLine($"templates : {_tempeletDic.Count}");
    }

    /// <summary>
    ///     make Json file and save to Resource/EntityTemplate.
    /// </summary>
    public void SaveEntityTemplateToSingleFile(IEntity entity, string templateName)
    {
        var json = MakeEntityInfoJson(entity, Formatting.Indented, templateName);
        var path = $"Assets/Resources/EntityTemplate/SingleEntity/{templateName}.json";

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, json);
    }

    #region private - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    private bool IsHaveIgnoreSaveAttibute(object obj)
    {
        var t = obj.GetType();
        return Attribute.IsDefined(t, typeof(IgnoreSaveAttribute));
    }

    /// <summary>
    /// only make new Entity. not add components
    /// </summary>
    private IEntity MakeEntityByContext(EntityTemplate EntityTemplate, Contexts contexts)
    {
        System.Diagnostics.Debug.WriteLine($"entityInfo.ContextType : {EntityTemplate.Context}");
        IEntity newEntity = null;
        switch (EntityTemplate.Context)
        {
            case "Game":
                newEntity = contexts.game.CreateEntity();
                break;
            /*case "Input":
                newEntity = contexts.input.CreateEntity();
                break;
            case "Ui":
                newEntity = contexts.ui.CreateEntity();
            break;*/
            default:
                throw new Exception($" {EntityTemplate.Context} is not support type. if you have atribute, add it EntitySaveLoaderMakeEntityByContext() ");
        }

        return newEntity;
    }


    private void AddComponents(EntityTemplate EntityTemplate, IEntity newEntity)
    {
        //add components
        //deserialized componentValue is JObject. Jobject can be casted with dynamic (ToObject)
        foreach (KeyValuePair<string, dynamic> componentInfo in EntityTemplate.Components)
        {
            var componentLookUpName = RemoveComponentSubfix(componentInfo.Key);

            if (!GameComponentsLookup.componentNames.Contains(componentLookUpName))
            {
                throw new Exception($"{componentLookUpName} is not in GameComponentsLookup");
            }

            int componentLookUpIndex = (int)typeof(GameComponentsLookup).GetField(componentLookUpName).GetValue(null);
            var componentType = GameComponentsLookup.componentTypes[componentLookUpIndex];
            var component = componentInfo.Value.ToObject(componentType);

            //Debug.Log($"componentLookUpIndex : {componentLookUpIndex}");

            ((Entity)newEntity).AddComponent(componentLookUpIndex, component as IComponent);
        }
    }

    private void AddTagComponents(EntityTemplate EntityTemplate, IEntity newEntity)
    {
        if (!string.IsNullOrEmpty(EntityTemplate.Tags))
        {
            //System.Diagnostics.Debug.WriteLine($"tag : {EntityTemplate.Tags}");
            var parsedTags = EntityTemplate.Tags.Split(',');
            //add tag components
            foreach (var tagName in parsedTags)
            {
                if (string.IsNullOrEmpty(tagName))
                {
                    continue;
                }
                var componentLookUpName = RemoveComponentSubfix(tagName);

                if (!GameComponentsLookup.componentNames.Contains(componentLookUpName))
                {
                    throw new Exception("{componentLookUpName} is not in GameComponentsLookup");
                }

                int componentLookUpIndex = (int) typeof(GameComponentsLookup).GetField(componentLookUpName).GetValue(null);
                var componentType = GameComponentsLookup.componentTypes[componentLookUpIndex];
                var tagComponent = Activator.CreateInstance(componentType);

                //Debug.Log($"componentLookUpIndex : {componentLookUpIndex}");

                ((Entity)newEntity).AddComponent(componentLookUpIndex, tagComponent as IComponent);
            }
        }
    }

#endregion

}
