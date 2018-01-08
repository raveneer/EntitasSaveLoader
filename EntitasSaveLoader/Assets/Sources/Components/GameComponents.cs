using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using UnityEngine.UI;

[Game]
public class Fire : IComponent
{
    public float Strength;
}

[Game]
public class Health : IComponent
{
    public float Value;
}

[Game]
public class Name : IComponent
{
    public string Value;
}

[Game]
public class Position2D : IComponent
{
    public float X;
    public float Y;
}

[Input, Unique]
public class LeftMouseComponent : IComponent
{
}

[Input, Unique]
public class RightMouseComponent : IComponent
{
}

[Input, Unique]
public class MiddleMouseComponent : IComponent
{
}

[Input, Unique]
public class SpaceKeyComponent : IComponent
{
    public bool IsKeyUp;
}

[Input]
public class MouseDownComponent : IComponent
{
    public Vector2 position;
}

[Input]
public class MousePositionComponent : IComponent
{
    public Vector2 position;
}

[Input]
public class MouseUpComponent : IComponent
{
    public Vector2 position;
}

