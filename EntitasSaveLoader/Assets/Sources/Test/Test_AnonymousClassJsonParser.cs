using System;
using System.Diagnostics;
using NUnit.Framework;

public class Test_AnonymousClassJsonParser
{
    [Test]
    public void MakeNewObjectWithJson_1()
    {
        var jsonString = @"{""TypeName"":""SomeDog"",""Json"":""{\""BitePower\"":10.0}""}";
        var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
        Assert.IsTrue(newObject is SomeDog);
        Assert.AreEqual(10, ((SomeDog) newObject).BitePower);
    }

    [Test]
    public void MakeNewObjectWithJson_2()
    {
        var jsonString = @"{""TypeName"":""SomeCat"",""Json"":""{\""ScratchPower\"":5.0}""}";
        var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
        Assert.IsTrue(newObject is SomeCat);
        Assert.AreEqual(5, ((SomeCat) newObject).ScratchPower);
    }

    [Test]
    public void MakeNewObjectWithJson_3()
    {
        var fido = new SomeDog {BitePower = 10};
        var json = AnonymousClassJsonParser.MakeNestedJson(fido);
        var expected = @"{""TypeName"":""SomeDog"",""Json"":""{\""BitePower\"":10.0}""}";
        Assert.AreEqual(expected, json);
    }

    [Test]
    public void SerialAndDeselial()
    {
        var fido = new SomeDog {BitePower = 10};
        var jsonString = AnonymousClassJsonParser.MakeNestedJson(fido);
        var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
        Assert.AreEqual(fido.GetType(), newObject.GetType());
        Assert.AreEqual(fido.BitePower, ((SomeDog) newObject).BitePower);
    }

    [Test]
    public void SerialAndDeserial_SupportValueTypes()
    {
        var typeCheck = new SomeClassHaveValueTypeFiled(1.1f, 2, "three", true, SomeEnum.Second);
        var jsonString = AnonymousClassJsonParser.MakeNestedJson(typeCheck);
        var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
        Assert.AreEqual(typeCheck.GetType(), newObject.GetType());
        Assert.AreEqual(1.1f, ((SomeClassHaveValueTypeFiled)newObject).FValue);
        Assert.AreEqual(2 , ((SomeClassHaveValueTypeFiled)newObject).IValue);
        Assert.AreEqual("three", ((SomeClassHaveValueTypeFiled)newObject).SValue);
        Assert.AreEqual(true , ((SomeClassHaveValueTypeFiled)newObject).BValue);
        Assert.AreEqual(SomeEnum.Second, ((SomeClassHaveValueTypeFiled)newObject).EValue);
    }


    [Timeout(1000)]
    [Test]
    public void Performance_Serialize()
    {
        var fido = new SomeDog {BitePower = 10};
        for (var i = 0; i < 10000; i++)
        {
            var jsonString = AnonymousClassJsonParser.MakeNestedJson(fido);
        }
    }


    [Timeout(1000)]
    [Test]
    public void Performance_DeSerialize()
    {
        var jsonString = @"{""TypeName"":""SomeCat"",""Json"":""{\""ScratchPower\"":5.0}""}";
        for (var i = 0; i < 10000; i++)
        {
            var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
            Assert.IsTrue(newObject is SomeCat);
        }
    }

    [Test]
    public void MakeNewComponentWithJson_ReturnNull_NotMatchClassName()
    {
        var jsonString = @"{""TypeName"":""blah"",""Json"":""{\""BitePower\"":10.0}""}";
        var newObject = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
        Assert.IsNull(newObject);
    }

}

public class SomeCat
{
    public float ScratchPower = 0;
}

public class SomeDog
{
    public float BitePower = 10;
}

public class SomeClassHaveValueTypeFiled
{
    public float FValue;
    public int IValue;
    public string SValue;
    public bool BValue;
    public SomeEnum EValue;

    public SomeClassHaveValueTypeFiled(float fValue, int iValue, string sValue, bool bValue, SomeEnum eValue)
    {
        FValue = fValue;
        IValue = iValue;
        SValue = sValue;
        BValue = bValue;
        EValue = eValue;
    }
}

public class SomeClassHaveRefTypeFiled
{
    public SomeCat SomeCatRef;
}
