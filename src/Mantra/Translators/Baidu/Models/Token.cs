// ReSharper disable once CheckNamespace
namespace Mantra.Translators.Baidu;

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
