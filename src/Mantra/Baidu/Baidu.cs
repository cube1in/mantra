using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mantra;

internal static class Baidu
{
    #region Private Members

    /// <summary>
    /// 身份验证Host
    /// </summary>
    private const string AuthHost = "https://aip.baidubce.com/oauth/2.0/token";

    /// <summary>
    /// OCR Host
    /// </summary>
    private const string OCRHost = "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate";

    /// <summary>
    /// 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
    /// 返回token示例
    /// </summary>
    public const string TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

    /// <summary>
    /// 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
    /// </summary>
    private const string ClientId = "";

    /// <summary>
    /// 百度云中开通对应服务应用的 Secret Key
    /// </summary>
    private const string ClientSecret = "";

    /// <summary>
    /// HttpClient
    /// </summary>
    private static readonly HttpClient Client = new();

    #endregion

    public static async Task<string?> GetTokenAsync()
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
        return JsonConvert.DeserializeObject<Token>(result, JsonSettings.SerializerSettings)?.AccessToken;
    }

    public static async Task<OCRResponse?> DoOCRAsync(string imgUrl)
    {
        var token = GetTokenAsync();
        var base64 = Convert.ToBase64String(await Client.GetByteArrayAsync(imgUrl));
        var paraList = new List<KeyValuePair<string, string>>
        {
            new("image", base64),
        };

        var response = await Client.PostAsync($"{OCRHost}?access_token={await token}", new FormUrlEncodedContent(paraList));
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<OCRResponse>(jsonString, JsonSettings.SerializerSettings);
    }

    public static async Task DoTranslateAsync(string text)
    {
        await Task.CompletedTask;
    }
}