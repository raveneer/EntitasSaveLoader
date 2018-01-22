using System;

/// <summary>
/// with this attribute, class will not be saved and desirialized by saveloader
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreSaveAttribute : System.Attribute
{
}