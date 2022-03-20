using PropertyChanged;

namespace Mantra.Core.Models;

/// <summary>
/// 边界框
/// </summary>
[AddINotifyPropertyChangedInterface]
internal class BoundingBox : BaseViewModel
{
    /// <summary>
    /// 左边位置
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// 顶部位置
    /// </summary>
    public double Top { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// 高度
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// 颜色
    /// #FFDB7093
    /// </summary>
    public string Color { get; set; } = "#FFDB7093";

    /// <summary>
    /// 矩形内的源词
    /// 必须具有默认值，否则在传到翻译 api 时会导致插入值错误
    /// </summary>
    public string OriginalText { get; set; } = "没有识别结果";

    /// <summary>
    /// 矩形内的翻译
    /// 必须具有默认值，否则在传到翻译 api 时会导致插入值错误
    /// </summary>
    public string TranslatedText { get; set; } = "没有翻译结果";
}