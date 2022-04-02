using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mantra;

internal static class JsonSettings
{
    public static readonly JsonSerializerSettings SerializerSettings = new()
    {
        // 默认值
        DefaultValueHandling = DefaultValueHandling.Ignore,

        // 空值
        NullValueHandling = NullValueHandling.Ignore,

        // 循环序列化
        ReferenceLoopHandling = ReferenceLoopHandling.Error,

        // 驼峰
        ContractResolver = new CamelCasePropertyNamesContractResolver(),

        // 转换器
        Converters = new JsonConverter[] {new StringEnumConverter()}
    };
}