using System;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;


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
        var contexts = new Contexts();

        //arrange
        var json =
            @"{""Context"":""Game"",""Components"":{""SomeIntComponent"":{""Value"":10},""SomeStringComponent"":{""Value"":""aaa""}}}";

        var newEntity = EntitySaveLoader.MakeEntityFromJson(json, contexts);
        //assert
        Assert.AreEqual(2, newEntity.GetComponents().Length);
        Assert.AreEqual("Game", newEntity.contextInfo.name);
        Assert.AreEqual(10, ((SomeIntComponent) newEntity.GetComponents()[0]).Value);
        Assert.AreEqual("aaa", ((SomeStringComponent) newEntity.GetComponents()[1]).Value);
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
    public void MakeEntityInfoJson_return_Json_Indented()
    {
        //arrange
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var c1 = new SomeIntComponent {Value = 10};
        var c2 = new SomeFloatComponent {Value = 2.0f};
        entity.AddComponent(0, c1);
        entity.AddComponent(1, c2);
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.Indented, "newTemplete");
        Debug.WriteLine(resultJson);
        //assert
        var expected =
            @"{
  ""Name"": ""newTemplete"",
  ""Context"": ""Game"",
  ""Components"": {
    ""SomeIntComponent"": {
      ""Value"": 10
    },
    ""SomeFloatComponent"": {
      ""Value"": 2.0
    }
  }
}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void MakeEntityInfoJson_return_Json_NoneFormat()
    {
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
            @"{""Name"":null,""Context"":""Input"",""Components"":{""SomeBoolComponent"":{""Value"":true}}}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void MakeEntityInfoJson_return_Json_WhenNoComponent()
    {
        //arrange
        var contexts = new Contexts();
        var entity = contexts.input.CreateEntity();
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);

        //assert
        var expected =
            @"{""Name"":null,""Context"":""Input"",""Components"":{}}";
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
    
    [Test]
    public void NotSave_IgnoreSaveAttribute()
    {
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        entity.AddSomeFloat(10);
        entity.isSomeComponentHaveIgnoreSaveAttribute = true;
        //action
        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.None, null);
        Debug.WriteLine(resultJson);
        //assert
        var expected =
            @"{""Name"":null,""Context"":""Game"",""Components"":{""SomeFloatComponent"":{""Value"":10.0}}}";
        Assert.AreEqual(expected, resultJson);
    }

    [Test]
    public void Support_RefTypeFiled()
    {
        var contexts = new Contexts();
        var entity = contexts.game.CreateEntity();
        var t1 = new SomeRefTypeComponent() { CoordRef = new Coord(){X = 10, Y = 20} };
        entity.AddComponent(0, t1);

        var resultJson = EntitySaveLoader.MakeEntityInfoJson(entity, Formatting.Indented, null);
        Debug.WriteLine(resultJson);

        var newEntity = EntitySaveLoader.MakeEntityFromJson(resultJson, contexts) as GameEntity;
        Assert.AreEqual(10, newEntity.someRefType.CoordRef.X);
        Assert.AreEqual(20, newEntity.someRefType.CoordRef.Y);

    }

}
