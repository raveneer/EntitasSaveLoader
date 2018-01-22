using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entitas;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Test_AnonymousClassJsonParser2
{
    [Test]
    public void MakeNewObjectWithJson_1()
    {
        Contexts _contexts = new Contexts();

        var container = new SomeContainer();
        var someInt = new SomeIntComponent(){Value = 10};
        var someString = new SomeStringComponent(){Value = "aaa"};

        container.Components.Add(someInt.GetType().ToString(), someInt);
        container.Components.Add(someString.GetType().ToString(), someString);

        var jsonString = JsonConvert.SerializeObject(container);
        Debug.WriteLine(jsonString);
        string expectString = @"{""Components"":{""SomeIntComponent"":{""Value"":10},""SomeStringComponent"":{""Value"":""aaa""}}}";
        
        //assert
        Assert.AreEqual(jsonString, expectString);
        

        //디시리얼- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        SomeContainer newContainer = JsonConvert.DeserializeObject<SomeContainer>(jsonString);

        Assert.AreEqual(2, newContainer.Components.Count);

        Assert.IsTrue(newContainer.Components.ContainsKey("SomeIntComponent"));
        Assert.IsTrue(newContainer.Components.ContainsKey("SomeStringComponent"));

        GameEntity newEntity = _contexts.game.CreateEntity();

        //add components
        //deserialized componentValue is JObject. Jobject can be casted with dynamic (ToObject)
        foreach (KeyValuePair<string, dynamic> componentInfo in newContainer.Components)
        {
            var comp = componentInfo.Value.ToObject(Type.GetType(componentInfo.Key));
            var componentLookUpName = EntitySaveLoader.RemoveComponentSubfix(componentInfo.Key);
            int componentLookUpIndex = (int)typeof(GameComponentsLookup).GetField(componentLookUpName).GetValue(null);
            
            Debug.WriteLine($"componentLookUpIndex : {componentLookUpIndex}");
            Assert.NotNull(newEntity);

            newEntity.AddComponent(componentLookUpIndex, comp as IComponent);
            
            Debug.WriteLine($"{newEntity}");
            Debug.WriteLine($"component added");
        }

        Debug.WriteLine($"{newEntity}");

        Assert.AreEqual(10, newEntity.someInt.Value);
        Assert.AreEqual("aaa", newEntity.someString.Value);
    }
}

public class SomeContainer
{
    //key is type, dynamic is Icomponent.
    public Dictionary<string, object> Components = new Dictionary<string, object>();
}
