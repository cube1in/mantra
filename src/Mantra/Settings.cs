using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mantra;

internal static class Settings
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

    /// <summary>
    /// 项目名称
    /// </summary>
    public static string ProjectName => DateTime.Now.ToString("yyyyMMddHHmmss");

    /// <summary>
    /// 项目路径
    /// </summary>
    public static string ProjectPath => Path.Combine(AppContext.BaseDirectory, "cache");
}