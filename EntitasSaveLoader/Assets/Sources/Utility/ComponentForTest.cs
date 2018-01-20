using System;
using Entitas;

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
    public Object ObjectRef;
}

public enum SomeEnum
{
    Zero = 0,
    First,
    Second
}
