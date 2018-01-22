using System;
using System.Diagnostics;
using EntityTempleteSaveLoader;
using Newtonsoft.Json;

/// <summary>
///     클래스래퍼를 사용하지 않는 버젼.
/// </summary>
public class AnonymousClassJsonParser2
{
    public static object MakeNewClassOrNull(string nestedJson)
    {
        if (string.IsNullOrWhiteSpace(nestedJson))
            return null;

        var wrapper = JsonConvert.DeserializeObject<ClassWrapper>(nestedJson);

        if (string.IsNullOrWhiteSpace(wrapper.TypeName))
            return null;

        try
        {
            var classType = Type.GetType(wrapper.TypeName, true);
            return JsonConvert.DeserializeObject(wrapper.Json, classType);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return null;
        }
    }

    public static string MakeNestedJson(object obj)
    {
        var wrapper = new ClassWrapper(obj);
        return JsonConvert.SerializeObject(wrapper);
    }
}