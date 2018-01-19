using NUnit.Framework;


    class Test_EntityJsonUtility
    {
        [Test]
        public void Entitas_AddComponent_NeedContext()
        {
            Contexts contexts = new Contexts();
            var entity = contexts.game.CreateEntity();
            SomeIntComponent t1 = new SomeIntComponent() { Value = 10 };
            SomeIntComponent t2 = new SomeIntComponent() { Value = 20 };
            entity.AddComponent(0, t1);
            entity.AddComponent(1, t2);
        }

        [Test]
        public void NewEntityWithComponentOrNull_Return_NewEntityWithComponentAdded()
        {
            Contexts contexts = Contexts.sharedInstance;
            //arrange
            string json =
                @"{""ContextType"":""Game"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":10}\""}"",""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":20}\""}""]}";
            //action
            var newEntity = EntityJsonUtility.MakeNewEntity(json, contexts);
            //assert
            Assert.AreEqual(2, newEntity.GetComponents().Length);
            Assert.AreEqual("Game", newEntity.contextInfo.name);
            Assert.AreEqual(10, ((SomeIntComponent)newEntity.GetComponents()[0]).Value);
            Assert.AreEqual(20, ((SomeIntComponent)newEntity.GetComponents()[1]).Value);
        }

        [Test]
        public void ToJson_return_Json_1()
        {
            //arrange
            Contexts contexts = new Contexts();
            var entity = contexts.game.CreateEntity();
            SomeIntComponent c1 = new SomeIntComponent() { Value = 10 };
            SomeIntComponent c2 = new SomeIntComponent() { Value = 20 };
            entity.AddComponent(0, c1);
            entity.AddComponent(1, c2);
            //action
            var resultJson = EntityJsonUtility.MakeEntityInfoJson(entity);
            //assert
            string expected =
                @"{""ContextType"":""Game"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":10}\""}"",""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":20}\""}""]}";
            Assert.AreEqual(expected, resultJson);
        }

        [Test]
        public void ToJson_return_Json_2()
        {
            //arrange
            Contexts contexts = new Contexts();
            var entity = contexts.input.CreateEntity();
            SomeBoolComponent c1 = new SomeBoolComponent() { Value = true };
            entity.AddComponent(0, c1);
            //action
            var resultJson = EntityJsonUtility.MakeEntityInfoJson(entity);
            //assert
            string expected =
                @"{""ContextType"":""Input"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SomeBoolComponent\"",\""Json\"":\""{\\\""Value\\\"":true}\""}""]}";
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
