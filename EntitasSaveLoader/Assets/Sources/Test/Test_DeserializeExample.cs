using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using NUnit.Framework;

public class Test_Deserialze_Example  {

    [Test]
    public void Can_Deserialize_ClassHaveConstructorWithParam()
    {
        var obj = new SomeContainer1(){Ref = new ClassHaveConstructorWithParam(1,2)};

        var jsonStr = JsonConvert.SerializeObject(obj);
        Debug.WriteLine(jsonStr);
        Assert.AreEqual(jsonStr, @"{""Ref"":{""X"":1,""Y"":2}}");

        var deserializedObj = JsonConvert.DeserializeObject<SomeContainer1>(jsonStr);

        Assert.AreEqual(1, deserializedObj.Ref.X);
        Assert.AreEqual(2, deserializedObj.Ref.Y);
    }

    [Test]
    public void Can_Deserialize_ClassHavePublicField()
    {
        var obj = new SomeContainer2() { Ref = new ClassHavePublicField(){X = 1, Y = 2} };

        var jsonStr = JsonConvert.SerializeObject(obj);
        Debug.WriteLine(jsonStr);
        Assert.AreEqual(jsonStr, @"{""Ref"":{""X"":1,""Y"":2}}");

        var deserializedObj = JsonConvert.DeserializeObject<SomeContainer2>(jsonStr);

        Assert.AreEqual(1, deserializedObj.Ref.X);
        Assert.AreEqual(2, deserializedObj.Ref.Y);
    }

    [Test]
    public void Can_Deserialize_ClassHavePublicSetterProperty()
    {
        var obj = new SomeContainer3() { Ref = new ClassHavePublicSetterProperty() { X = 1, Y = 2 } };

        var jsonStr = JsonConvert.SerializeObject(obj);
        Debug.WriteLine(jsonStr);
        Assert.AreEqual(jsonStr, @"{""Ref"":{""X"":1,""Y"":2}}");

        var deserializedObj = JsonConvert.DeserializeObject<SomeContainer3>(jsonStr);

        Assert.AreEqual(1, deserializedObj.Ref.X);
        Assert.AreEqual(2, deserializedObj.Ref.Y);
    }

    [Test]
    public void CanNot_Deserialize_ClassHaveBlackConstructorOverride()
    {

        var obj = new SomeContainer4() { Ref = new ClassHaveBlackConstructorOverload(1,2) };

        var jsonStr = JsonConvert.SerializeObject(obj);
        Debug.WriteLine(jsonStr);
        Assert.AreEqual(jsonStr, @"{""Ref"":{""X"":1,""Y"":2}}");

        var deserializedObj = JsonConvert.DeserializeObject<SomeContainer4>(jsonStr);

        //not deserialized!
        Assert.AreEqual(0, deserializedObj.Ref.X);
        Assert.AreEqual(0, deserializedObj.Ref.Y);
    }
}


public class SomeContainer1
{
    public ClassHaveConstructorWithParam Ref;
}

public class SomeContainer2
{
    public ClassHavePublicField Ref;
}

public class SomeContainer3
{
    public ClassHavePublicSetterProperty Ref;
}

public class SomeContainer4
{
    public ClassHaveBlackConstructorOverload Ref;
}


/// <summary>
    /// this calss have private set property with constructor.
    /// </summary>
    public class ClassHaveConstructorWithParam
{
    public int X { get; }
    public int Y { get; }

    public ClassHaveConstructorWithParam(int x, int y)
    {
        X = x;
        Y = y;
    }
}

/// <summary>
/// this calss have public field.
/// </summary>
public class ClassHavePublicField
{
    public int X;
    public int Y;
}

/// <summary>
/// this calss doesn't have constructor, but have public setter property
/// </summary>
public class ClassHavePublicSetterProperty
{
    public int X { get; set; }
    public int Y { get; set; }
}

/// <summary>
/// this class have overloaded black constructor. this make problem with deserialize
/// </summary>
public class ClassHaveBlackConstructorOverload
{
    public int X { get;}
    public int Y { get;}

    public ClassHaveBlackConstructorOverload(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ClassHaveBlackConstructorOverload()
    {
    }
}





