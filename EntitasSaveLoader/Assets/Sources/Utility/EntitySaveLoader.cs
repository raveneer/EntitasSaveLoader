﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Entitas;
using EntityTempleteSaveLoader;
using Newtonsoft.Json;
using UnityEngine;
using System.Reflection;

public class EntitySaveLoader
{
    private static  Dictionary<string, string> _templetDictionary = new Dictionary<string, string>();
    private static bool _dictionaryReady;
    

    public static void SaveEntitiesInScene(Contexts contexts, string saveFileName)
    {
        var savingEntities = contexts.game.GetGroup(GameMatcher.SavingData).GetEntities();

        EntitiesSaveData saveData = new EntitiesSaveData();

        Debug.Log($"saving entities : {savingEntities.Length}");
        foreach (var savingEntity in savingEntities)
        {
            saveData.EntityInfoJsons.Add(MakeEntityInfoJson(savingEntity, Formatting.None));
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        string path = $"Assets/Resources/EntityTemplete/{saveFileName}.json";

        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        File.WriteAllText(path, json);

        Debug.Log("SaveEntitiesInScene done!");
    }

    public static void LoadEntitiesFromSaveFile(Contexts contexts, string saveFileName)
    {
        //파일을 읽어온다.
        var saveFileAsset = Resources.Load<TextAsset>($"EntityTemplete/{saveFileName}");

        Debug.Log($" {saveFileAsset.name}, {saveFileAsset.text}");

        if (saveFileAsset.text == null)
        {
            Debug.Log($"no save file : {saveFileName}");
            return;
        }

        //파일을 saveData로 디시리얼.
        EntitiesSaveData saveData = JsonConvert.DeserializeObject<EntitiesSaveData>(saveFileAsset.text);
        //saveData의 데이터 리스트를 다시 엔티티인포로 디시리얼.
        foreach (var infoJson in saveData.EntityInfoJsons)
        {
            Debug.Log($"saved entities : {saveData.EntityInfoJsons.Count}");
            //엔티티인포들을 이용해서 엔티티를 생성한다.
            MakeNewEntity(infoJson, contexts);
        }

        Debug.Log("LoadEntitiesFromSaveFile done!");
    }


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

    
    public static IEntity MakeNewEntity(string json, Contexts contexts)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new Exception();
        }

        EntityInfo entityInfo = JsonConvert.DeserializeObject<EntityInfo>(json);
        IEntity newEntity = MakeEntityByContext(contexts, entityInfo);
        
        //add components
        for (int i = 0; i < entityInfo.ComponentsWrapperJsons.Count; i++)
        {
            var component = AnonymousClassJsonParser.MakeNewClassOrNull(entityInfo.ComponentsWrapperJsons[i]);
            //todo : this line works well but test fail...why?

            System.Diagnostics.Debug.WriteLine(component.GetType().ToString());

            string componenTypeNameRemovedSubFix = RemoveComponentSubfix(component.GetType().ToString());
            //string componenTypeName = typeof(GameComponentsLookup).GetField(component.GetType().ToString()).Name;
            
            //todo : 알았다. 이름 끝에 component가 붙으면 안되는거였어!
            
            int componentLookUpIndex = (int)(typeof(GameComponentsLookup).GetField(componenTypeNameRemovedSubFix).GetValue(null));

            //todo : if component is flag, not add, just replce.
            if (IsFlagComponent(component as IComponent))
            {
                ((Entity)newEntity).AddComponent(componentLookUpIndex, component as IComponent);
            }
            else
            {
                ((Entity)newEntity).AddComponent(componentLookUpIndex, component as IComponent);
            }

            //check for adding groups
            var fireEntities = contexts.game.GetGroup(GameMatcher.Fire).GetEntities();
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
                    newEntity = contexts.ui.CreateEntity();
                break;*/
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

    public class EntitiesSaveData
    {
        public List<string> EntityInfoJsons = new List<string>();
    }
    
}