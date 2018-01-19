using System;
using System.Diagnostics;
using EntityTempleteSaveLoader;
using Newtonsoft.Json;

/// <summary>
///     json.net 을 사용함. 객체를 래퍼로 래핑하여 타입과 json 정보를 함께 저장한다.
///     리플렉션을 이용하여 클래스를 만든다. 따라서 클래스 타입이 어셈블리에 있다면 생성되고 없으면 실패한다.
///     주의 : IOS에서 작동여부가 불확실하다. 테스트 필요.
/// </summary>
public class AnonymousClassJsonParser
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