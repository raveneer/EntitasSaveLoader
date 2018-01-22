using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;
using NUnit.Framework;

public class EntitySaveLoader
{
    private static readonly Dictionary<string, string> _templetDictionary = new Dictionary<string, string>();
    private static bool _dictionaryReady;
    
    public static void SaveAllEntitiesInScene(Contexts contexts, string saveFileName)
    {
        var savingEntities = contexts.game.GetGroup(GameMatcher.SavingData).GetEntities();
        var saveData = new EntitiesSaveData();
        foreach (var savingEntity in savingEntities)
        {
            saveData.EntityInfos.Add(MakeEntityInfo(savingEntity));
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
    
    public static IEntity MakeEntityFromTempleteName(string templeteName, Contexts _contexts)
    {
        //razy init
        if (!_dictionaryReady)
        {
            ReloadTempletesFromResource();
        }

        if (!_templetDictionary.ContainsKey(templeteName))
        {
            Debug.Log($"error : can't find templeteName {templeteName}.");
            return null;
        }

        return MakeEntityFromJson(_templetDictionary[templeteName], _contexts);
    }
    
    public static IEntity MakeEntityFromJson(string json, Contexts contexts)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.Log("empty json!");
        }

        var entityInfo = JsonConvert.DeserializeObject<EntityInfo>(json);
        return MakeEntityFromEntityInfo(entityInfo, contexts);
    }

    //todo : support input, ui etc... components
    private static IEntity MakeEntityFromEntityInfo(EntityInfo entityInfo, Contexts contexts)
    {
        IEntity newEntity = MakeEntityByContext(entityInfo, contexts);

        //add components
        //deserialized componentValue is JObject. Jobject can be casted with dynamic (ToObject)
        foreach (KeyValuePair<string, dynamic> componentInfo in entityInfo.Components)
        {
            var component = componentInfo.Value.ToObject(Type.GetType(componentInfo.Key));
            var componentLookUpName = EntitySaveLoader.RemoveComponentSubfix(componentInfo.Key);
            int componentLookUpIndex = (int)typeof(GameComponentsLookup).GetField(componentLookUpName).GetValue(null);

            //Debug.Log($"componentLookUpIndex : {componentLookUpIndex}");

            if (IsFlagComponent(component as IComponent))
            {
                ((Entity)newEntity).AddComponent(componentLookUpIndex, component as IComponent);
            }
            else
            {
                ((Entity)newEntity).AddComponent(componentLookUpIndex, component as IComponent);
            }
        }
        
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
    public static string MakeEntityInfoJson(IEntity entity, Formatting jsonFormatting)
    {
        EntityInfo entityInfo = MakeEntityInfo(entity);
        var jsonStr = JsonConvert.SerializeObject(entityInfo, jsonFormatting);

        return jsonStr;
    }
    
    public static EntityInfo MakeEntityInfo(IEntity entity)
    {
        var entityInfo = new EntityInfo
        {
            ContextType = entity.contextInfo.name
        };

        foreach (var component in entity.GetComponents())
        {
            if (!IsHaveIgnoreSaveAttibute(component))
            {
                entityInfo.Components.Add(component.GetType().ToString(), component);
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

    /// <summary>
    ///     make nested Json file and save to Resource/EntityTemplete.
    /// </summary>
    public static void GenerateEntityTemplete(IEntity entity, string fileName)
    {
        var json = MakeEntityInfoJson(entity, Formatting.Indented);
        var path = $"Assets/Resources/EntityTemplete/{fileName}.json";

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
    private static IEntity MakeEntityByContext(EntityInfo entityInfo, Contexts contexts)
    {
        System.Diagnostics.Debug.WriteLine($"entityInfo.ContextType : {entityInfo.ContextType}");
        IEntity newEntity = null;
        switch (entityInfo.ContextType)
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
                throw new Exception($" {entityInfo.ContextType} is not support type. if you have atribute, add it EntitySaveLoaderMakeEntityByContext() ");
        }

        return newEntity;
    }


    #endregion


    public class EntityInfo
    {
        public string ContextType;
        public Dictionary<string, object> Components = new Dictionary<string, object>();
    }

    public class EntitiesSaveData
    {
        public List<EntityInfo> EntityInfos = new List<EntityInfo>();
    }
    
    
}