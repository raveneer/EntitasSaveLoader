using Entitas;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

[Game]
public class SomeFloatComponent : IComponent
{
    public float Value;
}

[Game]
public class SomeStringComponent : IComponent
{
    public string Value;
}

[Game]
public class SomeIntComponent : IComponent
{
    public int Value;
}

[Game]
public class SomeBoolComponent : IComponent
{
    public bool Value;
}

[Game]
public class SomeEnumComponent : IComponent
{
    public SomeEnum Value;
}

[Game]
public class SomeRefTypeComponent : IComponent
{
    public Coord CoordRef;
}

[Game]
public class SomeVector3Component : IComponent
{
    public Vector3 Value;
}

[IgnoreSave]
[Game]
public class SomeGameObjectRefTypeComponent : IComponent
{
    public GameObject Ref;
}

[IgnoreSave]
public class SomeComponentHaveIgnoreSaveAttribute : IComponent
{

}

[Game]
public class SomeTagComponent : IComponent
{
}

public class Coord
{
    public int X;
    public int Y;
}

public enum SomeEnum
{
    Zero = 0,
    First,
    Second
}
