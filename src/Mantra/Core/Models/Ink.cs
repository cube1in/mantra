namespace Mantra.Core.Models;

internal class Ink
{
    /// <summary>
    /// 字体颜色
    /// </summary>
    public string Foreground { get; set; } = "#FF000000";

    /// <summary>
    /// 背景颜色
    /// </summary>
    public string Background { get; set; } = "#FFFFFFFF";

    /// <summary>
    /// 字体大小
    /// </summary>
    public double FontSize { get; set; } = 13;

    /// <summary>
    /// 字体重量
    /// </summary>
    public string FontWeight { get; set; } = "Normal";

    /// <summary>
    /// 字体
    /// </summary>
    public string FontFamily { get; set; } = "";

    /// <summary>
    /// 排版
    /// </summary>
    public string TextAlignment { get; set; } = "Left";

    /// <summary>
    /// 行高
    /// </summary>
    public double LineHeight { get; set; } = 0;

    /// <summary>
    /// 横向字符间距
    /// </summary>
    public double HorizontalCharacterSpacing { get; set; } = 1;

    /// <summary>
    /// 竖向字符间距
    /// </summary>
    public double VerticalCharacterSpacing { get; set; } = 1;

    /// <summary>
    /// 方向
    /// 默认水平 Horizontal
    /// Vertical
    /// </summary>
    public string Orientation { get; set; } = "Horizontal";

    /// <summary>
    /// 水平对齐
    /// </summary>
    public string HorizontalAlignment { get; set; } = "Left";
}