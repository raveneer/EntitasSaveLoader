using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// with this attribute, class will not be saved and desirialized by saveloader
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreSaveAttribute : System.Attribute
{
}