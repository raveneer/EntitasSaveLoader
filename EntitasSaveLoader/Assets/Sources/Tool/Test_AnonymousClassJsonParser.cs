using NUnit.Framework;

namespace UnitTestProject
{
    internal class Test_AnonymousClassJsonParser
    {
        [Test]
        public void MakeNewComponentWithJson_1()
        {
            var jsonString = @"{""TypeName"":""UnitTestProject.SomeDog"",""Json"":""{\""BitePower\"":10.0}""}";
            var newComponent = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
            Assert.IsTrue(newComponent is SomeDog);
            Assert.AreEqual(10, ((SomeDog) newComponent).BitePower);
        }

        [Test]
        public void MakeNewComponentWithJson_2()
        {
            var jsonString = @"{""TypeName"":""UnitTestProject.SomeCat"",""Json"":""{\""ScratchPower\"":5.0}""}";
            var newComponent = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
            Assert.IsTrue(newComponent is SomeCat);
            Assert.AreEqual(5, ((SomeCat) newComponent).ScratchPower);
        }

        [Test]
        public void MakeNestedJsonFromIComponent()
        {
            var fido = new SomeDog {BitePower = 10};
            var json = AnonymousClassJsonParser.MakeNestedJson(fido);
            var expected = @"{""TypeName"":""UnitTestProject.SomeDog"",""Json"":""{\""BitePower\"":10.0}""}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void SelialAndDeselial()
        {
            var fido = new SomeDog {BitePower = 10};
            var jsonString = AnonymousClassJsonParser.MakeNestedJson(fido);
            var newComponent = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
            Assert.AreEqual(fido.GetType(), newComponent.GetType());
            Assert.AreEqual(fido.BitePower, ((SomeDog) newComponent).BitePower);
        }

        [Timeout(500)]
        [Test]
        [Ignore("1만개에 500ms 정도로 매우 빠름.")]
        public void Performance_Serialize()
        {
            var fido = new SomeDog {BitePower = 10};
            for (var i = 0; i < 10000; i++)
            {
                var jsonString = AnonymousClassJsonParser.MakeNestedJson(fido);
            }
        }


        [Timeout(300)]
        [Test]
        [Ignore("1만개에 300ms 정도로 매우 빠름.")]
        public void Performance_DeSerialize()
        {
            var jsonString = @"{""TypeName"":""UnitTestProject.SomeCat"",""Json"":""{\""ScratchPower\"":5.0}""}";
            for (var i = 0; i < 10000; i++)
            {
                var newComponent = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
                Assert.IsTrue(newComponent is SomeCat);
            }
        }

        [Test]
        public void MakeNewComponentWithJson_ReturnNull_NotMatchClassName()
        {
            var jsonString = @"{""TypeName"":""blah"",""Json"":""{\""BitePower\"":10.0}""}";
            var newComponent = AnonymousClassJsonParser.MakeNewClassOrNull(jsonString);
            Assert.IsNull(newComponent);
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
}