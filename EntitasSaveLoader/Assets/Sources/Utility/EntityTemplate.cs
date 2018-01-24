using System.Collections.Generic;

public class EntityTemplate
{
    public string Name;
    public string Context;
    public string Tags;
    public Dictionary<string, object> Components = new Dictionary<string, object>();

    public override string ToString()
    {
        string str = $"Name : {Name}, "
                        + $"Context : {Context}, "
                        + $"Tags : {Tags}, "
            ;
        return str;

    }
}
