using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable InconsistentNaming
namespace Mantra.Translators.Baidu;

internal class Baidu : ITranslator
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
    private readonly string _clientId;

    /// <summary>
    /// 百度云中开通对应服务应用的 Secret Key
    /// </summary>
    private readonly string _clientSecret;

    /// <summary>
    /// Http Client
    /// </summary>
    private static readonly HttpClient Client = new();

    #endregion

    public Baidu()
    {
        const string path = @"C:\Users\sou1m\Documents\baidu_translator.json";
        if (!File.Exists(path)) throw new FileNotFoundException();

        var jo = JObject.Parse(File.ReadAllText(path));
        _clientId = jo["ClientId"]!.Value<string>()!;
        _clientSecret = jo["ClientSecret"]!.Value<string>()!;
    }

    private async Task<string?> GetTokenAsync()
    {
        var paraList = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", _clientId),
            new("client_secret", _clientSecret)
        };

        var response = await Client.PostAsync(AuthHost, new FormUrlEncodedContent(paraList));
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Token>(result, JsonSettings.SerializerSettings)?.AccessToken;
    }

    #region ITranslate

    public string Translate(string input, string from, string to)
    {
        return TranslateAsync(input, from, to).Result;
    }

    public IEnumerable<string> TranslateGroup(IEnumerable<string> groupInput, string from, string to)
    {
        return TranslateGroupAsync(groupInput, from, to).Result;
    }

    public async Task<string> TranslateAsync(string input, string from, string to)
    {
        var jsonString = $@"{{""q"":""{input}"",""from"":""{from}"",""to"":""{to}""}}";
        var response = await Client.PostAsync($"{TransHost}?access_token={await GetTokenAsync()}",
            new StringContent(jsonString, null, "application/json"));

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return string.Join(Environment.NewLine,
            JsonConvert.DeserializeObject<TranslateResponse>(result, JsonSettings.SerializerSettings)!.Result
                .TransResult.Select(r => r.Dst));
    }

    public async Task<IEnumerable<string>> TranslateGroupAsync(IEnumerable<string> groupInput, string from, string to)
    {
        var input = string.Join(JsonSettings.SpecialDelimiter, groupInput);
        var text = await TranslateAsync(input, from, to);
        return text.Split(JsonSettings.SpecialDelimiter);
    }

    #endregion
}