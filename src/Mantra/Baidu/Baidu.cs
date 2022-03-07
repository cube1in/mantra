using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
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
    /// 图片翻译
    /// </summary>
    private const string PictureTransHost = "https://aip.baidubce.com/file/2.0/mt/pictrans/v1";

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

    #endregion

    private static async Task<string?> GetTokenAsync(HttpClient client)
    {
        var paraList = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", ClientId),
            new("client_secret", ClientSecret)
        };

        var response = await client.PostAsync(AuthHost, new FormUrlEncodedContent(paraList));
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Token>(result, JsonSettings.SerializerSettings)?.AccessToken;
    }

    public static async Task<OCRResponse?> DoOCRAsync(HttpClient client, MemoryStream stream)
    {
        var token = GetTokenAsync(client);

        var paraList = new List<KeyValuePair<string, string>>
        {
            new("image", Convert.ToBase64String(stream.ToArray())),
        };

        var response =
            await client.PostAsync($"{OCRHost}?access_token={await token}", new FormUrlEncodedContent(paraList));
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<OCRResponse>(jsonString, JsonSettings.SerializerSettings);
    }

    public static async Task<PictureTransResponse?> DoTranslateAsync(HttpClient client, Stream stream)
    {
        var token = GetTokenAsync(client);

        var bytesContent = new StreamContent(stream);
        bytesContent.Headers.ContentType =  MediaTypeHeaderValue.Parse("mutipart/form-data");

        var multiForm = new MultipartFormDataContent();
        // 源语种方向
        // multiForm.Add(new StringContent("en"), "from");
        // 	译文语种方向
        // multiForm.Add(new StringContent("zh"), "to");
        // 	固定值：3
        // multiForm.Add(new StringContent("3"), "v");
        // 图片贴合类型：0 - 关闭文字贴合 1 - 返回整图贴合 2 - 返回块区贴合
        // multiForm.Add(new StringContent("1"), "paste");
        // 请求翻译的图片数据
        multiForm.Add(bytesContent, "image");

        var response = await client.PostAsync($"{PictureTransHost}?access_token={await token}&from=en&to=zh&v=3&paste=1", multiForm);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PictureTransResponse>(jsonString);
    }
}