

#if NETFRAMEWORK
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Soukoku.AspNet.Mvc.ViteIntegration;

public static class JsonWrapper
{
    static readonly JsonSerializerSettings Options = new()
    {
         ContractResolver = new DefaultContractResolver
         {
             NamingStrategy = new CamelCaseNamingStrategy()
         }
    };

    public static T? Deserialize<T>(string jsonText)
    {
        return JsonConvert.DeserializeObject<T>(jsonText, Options);
    }

    public static string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, Options);
    }
}
#else
using System.Text.Json;

namespace Soukoku.AspNetCore.ViteIntegration;

static class JsonWrapper
{
    static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static T? Deserialize<T>(string jsonText)
    {
        return JsonSerializer.Deserialize<T>(jsonText, Options);
    }

    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, Options);
    }
}
#endif