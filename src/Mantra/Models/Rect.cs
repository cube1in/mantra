// ReSharper disable once CheckNamespace

namespace Mantra;

/// <summary>
/// 矩形
/// </summary>
internal class Rect
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
    public int Group { get; set; } = 1;
}