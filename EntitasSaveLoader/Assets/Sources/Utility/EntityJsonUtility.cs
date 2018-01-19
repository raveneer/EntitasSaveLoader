using System;
using System.Collections.Generic;
using System.IO;
using Entitas;
using EntityTempleteSaveLoader;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class EntityJsonUtility
{
    private static  Dictionary<string, string> _templetDictionary = new Dictionary<string, string>();
    private static bool _dictionaryReady;
    

    public static void ReloadTempletesFromResource()
    {
        var templetAssets = Resources.LoadAll<TextAsset>("EntityTemplete");
        foreach (var textAsset in templetAssets)
        {
            //UnityEngine.Debug.Log($"textAsset.name {textAsset.name}, textAsset.text ={textAsset.text}");
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

    public static IEntity MakeEntityFromTemplete(string templeteName, Contexts _contexts)
    {
        //razy init
        if (!_dictionaryReady)
        {
            ReloadTempletesFromResource();
        }

        if (!_templetDictionary.ContainsKey(templeteName))
        {
            UnityEngine.Debug.Log($"error : can't find templeteName {templeteName}.");
            return null;
        }

        return MakeNewEntity(_templetDictionary[templeteName], _contexts);
    }
    
    public static Entity MakeNewEntity(string json, Contexts contexts)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.WriteLine("json null!");
            throw new Exception();
        }

        EntityInfo entityInfo = JsonConvert.DeserializeObject<EntityInfo>(json);
        //Debug.WriteLine($"{entityInfo.ContextType}, {entityInfo.ComponentsWrapperJsons.Count}");
        Entity newEntity = MakeEntityByContext(contexts, entityInfo);

        //add components
        for (int i = 0; i < entityInfo.ComponentsWrapperJsons.Count; i++)
        {
            IComponent componenet = AnonymousClassJsonParser.MakeNewClassOrNull(entityInfo.ComponentsWrapperJsons[i]) as IComponent;
            newEntity.AddComponent(i, componenet);
        }

        return newEntity;
    }


    public static string MakeEntityInfoJson(IEntity entity, Formatting jsonFormatting)
    {
        List<string> componentsWrapperJsons = new List<string>();
        foreach (var component in entity.GetComponents())
        {
            componentsWrapperJsons.Add(JsonConvert.SerializeObject(new ClassWrapper(component)));
        }
        var entityInfo = new EntityInfo() {ContextType = entity.contextInfo.name, ComponentsWrapperJsons = componentsWrapperJsons};
        var jsonStr = JsonConvert.SerializeObject(entityInfo, jsonFormatting);
        
        return jsonStr;
    }

    /// <summary>
    /// make nested Json file and save to Resource/EntityTemplete.
    /// </summary>
    public static void EntityInfoWriteToFile(IEntity entity, string fileName)
    {
        string json = MakeEntityInfoJson(entity, Formatting.Indented);
        string path = $"Assets/Resources/EntityTemplete/{fileName}.json";

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, json);
    }

    private static Entity MakeEntityByContext(Contexts contexts, EntityInfo entityInfo)
    {
        Entity newEntity = null;
        switch (entityInfo.ContextType)
        {
            case "Game":
                newEntity = contexts.game.CreateEntity();
                break;
                /*case "Input":
                    newEntity = contexts.input.CreateEntity();
                    break;
                case "Ui":
                    newEntity = contexts.ui.CreateEntity();*/
                break;
            default:
                throw new Exception("not support type");
        }

        return newEntity;
    }

    public class EntityInfo
    {
        public string ContextType;
        public List<string> ComponentsWrapperJsons = new List<string>();
    }

}
