// ReSharper disable once CheckNamespace

using System;

namespace Mantra;

/// <summary>
/// 矩形
/// </summary>
internal class Rect : BaseViewModel
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
    /// 所属的组
    /// 默认为 1
    /// </summary>
    public int Group { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; } = "#FFDB7093";

    /// <summary>
    /// 矩形内的源词
    /// </summary>
    public string OriginalText { get; set; } = "没有识别结果";

    /// <summary>
    /// 矩形内的翻译
    /// </summary>
    public string TranslatedText { get; set; } = "没有翻译结果";
}