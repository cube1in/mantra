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

        // 蛇形
        ContractResolver = new DefaultContractResolver {NamingStrategy = new SnakeCaseNamingStrategy()},

        Converters = new JsonConverter[] {new StringEnumConverter()}
    };

    /// <summary>
    /// 用于分割组内各段文字的分隔符
    /// Θ
    /// </summary>
    public static char SpecialDelimiter = 'Θ';
}