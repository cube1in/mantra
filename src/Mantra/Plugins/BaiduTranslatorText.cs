using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mantra.Core.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mantra.Plugins;

internal class BaiduTranslatorText : ITranslatorText
{
    #region Private Members

    /// <summary>
    /// 身份验证Host
    /// </summary>
    private const string AuthHost = "https://aip.baidubce.com/oauth/2.0/token";

    /// <summary>
    /// 机器翻译
    /// </summary>
    private const string TransHost = "https://aip.baidubce.com/rpc/2.0/mt/texttrans/v1";

    /// <summary>
    /// 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
    /// </summary>
    private static readonly string ClientId;

    /// <summary>
    /// 百度云中开通对应服务应用的 Secret Key
    /// </summary>
    private static readonly string ClientSecret;

    /// <summary>
    /// Http Client
    /// </summary>
    private static readonly HttpClient Client = new();

    /// <summary>
    /// 序列化设置
    /// </summary>
    private static readonly JsonSerializerSettings SerializerSettings = new()
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

    #endregion

    static BaiduTranslatorText()
    {
        const string path = @"C:\Users\sou1m\Documents\baidu_translator.json";
        if (!File.Exists(path)) throw new FileNotFoundException();

        var jo = JObject.Parse(File.ReadAllText(path));
        ClientId = jo["ClientId"]!.Value<string>()!;
        ClientSecret = jo["ClientSecret"]!.Value<string>()!;
    }

    private async Task<string?> GetTokenAsync()
    {
        var paraList = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", ClientId),
            new("client_secret", ClientSecret)
        };

        var response = await Client.PostAsync(AuthHost, new FormUrlEncodedContent(paraList));
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Token>(result, SerializerSettings)?.AccessToken;
    }

    #region ITranslate

    public string Translate(string input, string from, string to)
    {
        return TranslateAsync(input, from, to).Result;
    }

    public async Task<string> TranslateAsync(string input, string from, string to)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var jsonString = $@"{{""q"":""{input}"",""from"":""{from}"",""to"":""{to}""}}";
        var response = await Client.PostAsync($"{TransHost}?access_token={await GetTokenAsync()}",
            new StringContent(jsonString, null, "application/json"));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return string.Join(Environment.NewLine,
            JsonConvert.DeserializeObject<TranslateResponse>(result, SerializerSettings)!.Result
                .TransResult.Select(r => r.Dst));
    }

    #endregion
}

internal class Token
{
    /// <summary>
    /// 要获取的Access Token
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;

    /// <summary>
    /// Access Token的有效期(秒为单位，有效期30天)
    /// </summary>
    public long ExpiresIn { get; set; }
}

internal class TranslateResponse
{
    public string LogId { get; set; } = null!;

    public TranslateResult Result { get; set; } = null!;
}

internal class TranslateResult
{
    public IEnumerable<TranslateContext> TransResult { get; set; } = null!;

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;
}

internal class TranslateContext
{
    public string Dst { get; set; } = null!;

    public string Src { get; set; } = null!;
}