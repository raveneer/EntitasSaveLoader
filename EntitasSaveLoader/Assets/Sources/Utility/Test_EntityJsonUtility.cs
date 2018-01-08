using NUnit.Framework;

namespace UnitTestProject
{
    class Test_EntityJsonUtility
    {
        [Test]
        public void Entitas_AddComponent_NeedContext()
        {
            Contexts contexts = new Contexts();
            var entity = contexts.game.CreateEntity();
            Fire t1 = new Fire() { Strength = 10 };
            Fire t2 = new Fire() { Strength = 20 };
            entity.AddComponent(0, t1);
            entity.AddComponent(1, t2);
        }

        [Test]
        public void NewEntityWithComponentOrNull_Return_NewEntityWithComponentAdded()
        {
            Contexts contexts = Contexts.sharedInstance;
            //arrange
            string json =
                @"{""ContextType"":""Game"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""Fire\"",\""Json\"":\""{\\\""Strength\\\"":10.0}\""}"",""{\""TypeName\"":\""Fire\"",\""Json\"":\""{\\\""Strength\\\"":20.0}\""}""]}";
            //action
            var newEntity = EntityJsonUtility.MakeNewEntity(json, contexts);
            //assert
            Assert.AreEqual(2, newEntity.GetComponents().Length);
            Assert.AreEqual("Game", newEntity.contextInfo.name);
            Assert.AreEqual(10, ((Fire)newEntity.GetComponents()[0]).Strength);
            Assert.AreEqual(20, ((Fire)newEntity.GetComponents()[1]).Strength);
        }
        
        [Test]
        public void ToJson_return_Json_1()
        {
            //arrange
            Contexts contexts = new Contexts();
            var entity = contexts.game.CreateEntity();
            Fire c1 = new Fire() { Strength = 10 };
            Fire c2 = new Fire() { Strength = 20 };
            entity.AddComponent(0, c1);
            entity.AddComponent(1, c2);
            //action
            var resultJson = EntityJsonUtility.MakeEntityInfoJson(entity);
            //assert
            string expected =
                @"{""ContextType"":""Game"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""Fire\"",\""Json\"":\""{\\\""Strength\\\"":10.0}\""}"",""{\""TypeName\"":\""Fire\"",\""Json\"":\""{\\\""Strength\\\"":20.0}\""}""]}";
            Assert.AreEqual(expected, resultJson);
        }

        [Test]
        public void ToJson_return_Json_2()
        {
            //arrange
            Contexts contexts = new Contexts();
            var entity = contexts.input.CreateEntity();
            SpaceKeyComponent c1 = new SpaceKeyComponent() { IsKeyUp = true };
            entity.AddComponent(0, c1);
            //action
            var resultJson = EntityJsonUtility.MakeEntityInfoJson(entity);
            //assert
            string expected =
                @"{""ContextType"":""Input"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SpaceKeyComponent\"",\""Json\"":\""{\\\""IsKeyUp\\\"":true}\""}""]}";
            Assert.AreEqual(expected, resultJson);
        }

        [Test]
        public void ToJson_return_Json_WhenNoComponent()
        {
            //arrange
            Contexts contexts = new Contexts();
            var entity = contexts.input.CreateEntity();
            //action
            var resultJson = EntityJsonUtility.MakeEntityInfoJson(entity);
            //assert
            string expected =
                @"{""ContextType"":""Input"",""ComponentsWrapperJsons"":[]}";
            Assert.AreEqual(expected, resultJson);
        }
     
    }

}
