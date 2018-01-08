using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entitas;
using Newtonsoft.Json;

namespace UnitTestProject
{
    public class EntityJsonUtility
    {
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

        public static string MakeEntityInfoJson(IEntity entity)
        {
            List<string>componentsWrapperJsons = new List<string>();
            foreach (var component in entity.GetComponents())
            {
                componentsWrapperJsons.Add(JsonConvert.SerializeObject(new ClassWrapper(component)));
            }
            var entityInfo = new EntityInfo() { ContextType = entity.contextInfo.name, ComponentsWrapperJsons = componentsWrapperJsons };
            var jsonStr = JsonConvert.SerializeObject(entityInfo);

            return jsonStr;
        }

        /// <summary>
        /// 엔티티를 생성하기 위해 필요한 정보들. 
        /// </summary>
        public class EntityInfo
        {
            public string ContextType;
            public List<string> ComponentsWrapperJsons = new List<string>();
        }
    }
}