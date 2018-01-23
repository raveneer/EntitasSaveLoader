using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entitas;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

public class EntitySaveLoader
{
    private static readonly Dictionary<string, string> _templetDictionary = new Dictionary<string, string>();

    private static readonly Dictionary<string, EntityTemplete> _Templetes = new Dictionary<string, EntityTemplete>();

    private static bool _dictionaryReady;
    private static bool _groupDictionaryReady;

    public static void SaveAllEntitiesInScene(Contexts contexts, string saveFileName)
    {
        var savingEntities = contexts.game.GetGroup(GameMatcher.SavingData).GetEntities();
        var saveData = new EntitiesSaveData();
        foreach (var savingEntity in savingEntities)
        {
            //todo : saveAll 에서는 템플릿 이름을 지어줄 수가 없다...
            saveData.EntityInfos.Add(MakeEntityInfo(savingEntity, null));
        }

        var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        var path = $"Assets/Resources/EntityTemplete/{saveFileName}.json";

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, json);
        Debug.Log("SaveEntitiesInScene done!");
    }

    public static void LoadEntitiesFromSaveFile(Contexts contexts, string saveFileName)
    {
        var saveFileAsset = Resources.Load<TextAsset>($"EntityTemplete/{saveFileName}");

        if (saveFileAsset.text == null)
        {
            Debug.Log($"no save file : {saveFileName}");
            return;
        }

        var saveData = JsonConvert.DeserializeObject<EntitiesSaveData>(saveFileAsset.text);
        foreach (var entityInfo in saveData.EntityInfos)
        {
            MakeEntityFromEntityInfo(entityInfo, contexts);
        }

        Debug.Log("LoadEntitiesFromSaveFile done!");
    }
    

    /// <summary>
    /// find templete or templeteGroup, make entity.
    /// </summary>
    public static IEntity MakeEntityFromTempleteName(string templeteName, Contexts _contexts)
    {
        //razy init
        if (!_dictionaryReady)
        {
            ReloadTempletesFromResource();
        }

        if (!_groupDictionaryReady)
        {
            ReloadTempleteGroupFromResource();
        }

        if (_templetDictionary.ContainsKey(templeteName))
        {
            return MakeEntityFromJson(_templetDictionary[templeteName], _contexts);
        }

        if (_Templetes.ContainsKey(templeteName))
        {
            return MakeEntityFromEntityInfo(_Templetes[templeteName], _contexts);
        }

        Debug.Log($"can't find name templet: {templeteName}");
        return null;

    }
    
    public static IEntity MakeEntityFromJson(string json, Contexts contexts)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.Log("empty json!");
        }

        var entityInfo = JsonConvert.DeserializeObject<EntityTemplete>(json);
        return MakeEntityFromEntityInfo(entityInfo, contexts);
    }

    //todo : support input, ui etc... components
    private static IEntity MakeEntityFromEntityInfo(EntityTemplete entityTemplete, Contexts contexts)
    {
        IEntity newEntity = MakeEntityByContext(entityTemplete, contexts);
        AddTagComponents(entityTemplete, newEntity);
        AddComponents(entityTemplete, newEntity);
        return newEntity;
    }

    public static string RemoveComponentSubfix(string nameOfComponent)
    {
        if (nameOfComponent.EndsWith("Component"))
        {
            return nameOfComponent.Remove(nameOfComponent.Length - 9, 9);
        }
            
        return nameOfComponent;
    }

    public static bool IsFlagComponent(IComponent component)
    {
        return component.GetType().GetFields().Length == 0;
    }

    /// <summary>
    /// only value or string field have components serialized.
    /// ref type componets ignored.
    /// </summary>
    public static string MakeEntityInfoJson(IEntity entity, Formatting jsonFormatting, string templeteName)
    {
        EntityTemplete entityTemplete = MakeEntityInfo(entity, templeteName);
        var jsonStr = JsonConvert.SerializeObject(entityTemplete, jsonFormatting);

        return jsonStr;
    }
    
    public static EntityTemplete MakeEntityInfo(IEntity entity, string templeteName)
    {
        var entityInfo = new EntityTemplete
        {
            Name = templeteName,
            Context = entity.contextInfo.name
        };

        foreach (var component in entity.GetComponents())
        {
            if (!IsHaveIgnoreSaveAttibute(component))
            {
                string componentName = EntitySaveLoader.RemoveComponentSubfix(component.GetType().ToString());

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

    public static void ReloadTempletesFromResource()
    {
        var templetAssets = Resources.LoadAll<TextAsset>("EntityTemplete");
        foreach (var textAsset in templetAssets)
        {
            var key = textAsset.name;
            var value = textAsset.text;
            if (_templetDictionary.ContainsKey(key))
            {
                _templetDictionary[key] = value;
            }
            else
            {
                _templetDictionary.Add(key, value);
            }
        }
        _dictionaryReady = true;
    }

    public static void ReloadTempleteGroupFromResource()
    {
        var templetAssets = Resources.LoadAll<TextAsset>("EntityTemplete/Group");

        Debug.Log($"templetAssets.count : {templetAssets.Length}");

        foreach (var textAsset in templetAssets)
        {
            Debug.Log($"textAsset.text : {textAsset.text}");

            var jObject = JObject.Parse(textAsset.text);

            foreach (var jPair in jObject)
            {
                var newTemplete = jPair.Value.ToObject<EntityTemplete>();
                Debug.Log(newTemplete.ToString());
                _Templetes.Add(jPair.Key, newTemplete);
            }
        }

        Debug.Log($"templetes : {_Templetes.Count}");
        _groupDictionaryReady = true;
    }

    /// <summary>
    ///     make Json file and save to Resource/EntityTemplete.
    /// </summary>
    public static void GenerateEntityTemplete(IEntity entity, string tmepleteName)
    {
        var json = MakeEntityInfoJson(entity, Formatting.Indented, tmepleteName);
        var path = $"Assets/Resources/EntityTemplete/{tmepleteName}.json";

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, json);
    }

    #region private - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    private static bool IsHaveIgnoreSaveAttibute(object obj)
    {
        var t = obj.GetType();
        return Attribute.IsDefined(t, typeof(IgnoreSaveAttribute));
    }

    /// <summary>
    /// only make new Entity. not add components
    /// </summary>
    private static IEntity MakeEntityByContext(EntityTemplete entityTemplete, Contexts contexts)
    {
        System.Diagnostics.Debug.WriteLine($"entityInfo.ContextType : {entityTemplete.Context}");
        IEntity newEntity = null;
        switch (entityTemplete.Context)
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
                throw new Exception($" {entityTemplete.Context} is not support type. if you have atribute, add it EntitySaveLoaderMakeEntityByContext() ");
        }

        return newEntity;
    }


    private static void AddComponents(EntityTemplete entityTemplete, IEntity newEntity)
    {
        //add components
        //deserialized componentValue is JObject. Jobject can be casted with dynamic (ToObject)
        foreach (KeyValuePair<string, dynamic> componentInfo in entityTemplete.Components)
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

    private static void AddTagComponents(EntityTemplete entityTemplete, IEntity newEntity)
    {
        if (!string.IsNullOrEmpty(entityTemplete.Tags))
        {
            //System.Diagnostics.Debug.WriteLine($"tag : {entityTemplete.Tags}");
            var parsedTags = entityTemplete.Tags.Split(',');
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


    public class EntityTemplete
    {
        public string Name;
        public string Context;
        public string Tags;
        public Dictionary<string, object> Components = new Dictionary<string, object>();

        public override string ToString()
        {
            string str = $"Name : {Name}, "
                         + $"Context : {Context}, "
                         + $"Tags : {Tags}, "
                ;
            return str;

        }
    }

    public class EntitiesSaveData
    {
        public List<EntityTemplete> EntityInfos = new List<EntityTemplete>();
    }
}