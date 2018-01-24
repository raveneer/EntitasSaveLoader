using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;


internal class Test_SaveLoader
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
    public void MakeNewEntity_Return_NewEntityWithComponentAdded()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        var contexts = new Contexts();

        //arrange
        var json =
            @"{""Name"":null,""Context"":""Game"",""Tags"":""SavingData,SomeTag,"",""Components"":{""SomeInt"":{""Value"":10}}}";

        var newEntity = EntitySaveLoader.MakeEntityFromJson(json, contexts) as GameEntity;
        //assert
        Assert.AreEqual(3, newEntity.GetComponents().Length);
        Assert.AreEqual("Game", newEntity.contextInfo.name);
        Assert.AreEqual(10,newEntity.someInt.Value);
    }

    [Test]
    public void MakeNewEntity_Return_NewEntityWithTagsAdded()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        var contexts = new Contexts();

        //arrange
        var json =
            @"{""Name"":null,""Context"":""Game"",""Tags"":""SavingData,SomeTag"",""Components"":{""SomeBool"":{""Value"":true}}}";

        var newEntity = EntitySaveLoader.MakeEntityFromJson(json, contexts) as GameEntity;
        //assert

        Assert.IsTrue(newEntity.isSomeTag);
        Assert.IsTrue(newEntity.isSavingData);
    }

    [Test]
    public void TrimComponentString()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        string a = "someComponent";
        Assert.AreEqual("some", EntitySaveLoader.RemoveComponentSubfix(a));

        string b = "saveDataComponent";
        Assert.AreEqual("saveData", EntitySaveLoader.RemoveComponentSubfix(b));
    }
    
    [Test]
    public void MakeEntityInfoJson_return_Json_Indented()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        //arrange
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var c1 = new SomeIntComponent {Value = 10};
        var c2 = new SomeFloatComponent {Value = 2.0f};
        entity.AddComponent(0, c1);
        entity.AddComponent(1, c2);
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.Indented, "newtemplate");
        Debug.WriteLine(resultJson);
        //assert
        var expected =
            @"{
  ""Name"": ""newtemplate"",
  ""Context"": ""Game"",
  ""Tags"": null,
  ""Components"": {
    ""SomeInt"": {
      ""Value"": 10
    },
    ""SomeFloat"": {
      ""Value"": 2.0
    }
  }
}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void MakeEntityInfoJson_return_Json_NoneFormat()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        //arrange
        var contexts = new Contexts();
        var entity = contexts.input.CreateEntity();
        var c1 = new SomeBoolComponent {Value = true};
        entity.AddComponent(0, c1);
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);

        //assert
        var expected =
            @"{""Name"":null,""Context"":""Input"",""Tags"":null,""Components"":{""SomeBool"":{""Value"":true}}}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void MakeEntityInfoJson_return_Json_WhenNoComponent()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        //arrange
        var contexts = new Contexts();
        var entity = contexts.input.CreateEntity();
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);

        //assert
        var expected =
            @"{""Name"":null,""Context"":""Input"",""Tags"":null,""Components"":{}}";
        Assert.AreEqual(expected, resultJson);
    }
    
    [Test]
    public void Test_IsFlagComponent()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        var flagComponent = new SavingDataComponent();

        Assert.AreEqual(true, EntitySaveLoader.IsFlagComponent(flagComponent));

        var stringComponent = new SomeStringComponent();
        stringComponent.Value = "name";

        Assert.AreEqual(false, EntitySaveLoader.IsFlagComponent(stringComponent));
    }
    
    [Test]
    public void NotSave_IgnoreSaveAttribute()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        entity.AddSomeFloat(10);
        entity.isSomeComponentHaveIgnoreSaveAttribute = true;
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);
        //assert
        var expected =
            @"{""Name"":null,""Context"":""Game"",""Tags"":null,""Components"":{""SomeFloat"":{""Value"":10.0}}}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void Support_RefTypeFiled()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var t1 = new SomeRefTypeComponent() { PositionRef = new Position(){X = 10, Y = 20} };
        entity.AddComponent(0, t1);

        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.Indented, null);
        Debug.WriteLine(resultJson);

        var newEntity = EntitySaveLoader.MakeEntityFromJson(resultJson, contexts) as GameEntity;
        Assert.AreEqual(10, newEntity.someRefType.PositionRef.X);
        Assert.AreEqual(20, newEntity.someRefType.PositionRef.Y);

    }

    [Test]
    public void TagTypeComponents_AddedTo_Tags()
    {
        var EntitySaveLoader = new EntitySaveLoader(null);
        //arrange
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var c1 = new SomeBoolComponent { Value = true };
        entity.AddComponent(0, c1);
        entity.isSavingData = true;
        entity.isSomeTag = true;
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);

        //assert
        var expected =
            @"{""Name"":null,""Context"":""Game"",""Tags"":""SavingData,SomeTag,"",""Components"":{""SomeBool"":{""Value"":true}}}";
        Assert.AreEqual(expected, resultJson);
    }

}
