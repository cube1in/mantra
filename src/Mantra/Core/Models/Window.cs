using System.ComponentModel;
using PropertyChanged;

namespace Mantra.Core.Models;

/// <summary>
/// 窗
/// </summary>
[AddINotifyPropertyChangedInterface]
internal class Window : INotifyPropertyChanged
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
    /// 文字
    /// </summary>
    public Text Text { get; set; } = new();

    #region INotifyPropertyChanged

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning disable CS0067

    #endregion
}