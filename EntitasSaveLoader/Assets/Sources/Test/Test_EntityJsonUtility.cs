using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;


internal class Test_EntityJsonUtility
{
    [Test]
    public void Entitas_AddComponent_NeedContext()
    {
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var t1 = new SomeIntComponent {Value = 10};
        var t2 = new SomeIntComponent {Value = 20};
        entity.AddComponent(0, t1);
        entity.AddComponent(1, t2);
    }

    [Test]
    public void NewEntityWithComponentOrNull_Return_NewEntityWithComponentAdded()
    {
        var contexts = new Contexts();

        //arrange
        var json =
            @"{""ContextType"":""Game"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":10}\""}"",""{\""TypeName\"":\""SomeFloatComponent\"",\""Json\"":\""{\\\""Value\\\"":2.0}\""}""]}";

        //Assert.NotNull(json);

        var newEntity = EntitySaveLoader.MakeNewEntity(json, contexts);
        //assert
        Assert.AreEqual(2, newEntity.GetComponents().Length);
        Assert.AreEqual("Game", newEntity.contextInfo.name);
        Assert.AreEqual(2.0, ((SomeFloatComponent) newEntity.GetComponents()[0]).Value);
        Assert.AreEqual(10, ((SomeIntComponent) newEntity.GetComponents()[1]).Value);
    }

    [Test]
    public void TrimComponentString()
    {
        string a = "someComponent";
        Assert.AreEqual("some", EntitySaveLoader.RemoveComponentSubfix(a));

        string b = "saveDataComponent";
        Assert.AreEqual("saveData", EntitySaveLoader.RemoveComponentSubfix(b));

    }

    

    [Test]
    public void ToJson_return_Json_1()
    {
        //arrange
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var c1 = new SomeIntComponent {Value = 10};
        var c2 = new SomeIntComponent {Value = 20};
        entity.AddComponent(0, c1);
        entity.AddComponent(1, c2);
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.Indented);
        Debug.WriteLine(resultJson);
        //assert
        var expected =
            @"{
  ""ContextType"": ""Game"",
  ""ComponentsWrapperJsons"": [
    ""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":10}\""}"",
    ""{\""TypeName\"":\""SomeIntComponent\"",\""Json\"":\""{\\\""Value\\\"":20}\""}""
  ]
}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void ToJson_return_Json_2()
    {
        //arrange
        var contexts = new Contexts();
        var entity = contexts.input.CreateEntity();
        var c1 = new SomeBoolComponent {Value = true};
        entity.AddComponent(0, c1);
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None);
        //assert
        var expected =
            @"{""ContextType"":""Input"",""ComponentsWrapperJsons"":[""{\""TypeName\"":\""SomeBoolComponent\"",\""Json\"":\""{\\\""Value\\\"":true}\""}""]}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void ToJson_return_Json_WhenNoComponent()
    {
        //arrange
        var contexts = new Contexts();
        var entity = contexts.input.CreateEntity();
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None);
        //assert
        var expected =
            @"{""ContextType"":""Input"",""ComponentsWrapperJsons"":[]}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void Test_IsFlagComponent()
    {
        var flagComponent = new SavingDataComponent();

        Assert.AreEqual(true, EntitySaveLoader.IsFlagComponent(flagComponent));

        var stringComponent = new SomeStringComponent();
        stringComponent.Value = "name";

        Assert.AreEqual(false, EntitySaveLoader.IsFlagComponent(stringComponent));


    }
    
}
