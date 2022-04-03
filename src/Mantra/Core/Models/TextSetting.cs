using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PropertyChanged;

namespace Mantra.Core.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[AddINotifyPropertyChangedInterface]
internal class TextSetting : INotifyPropertyChanged
{
    /// <summary>
    /// 字体颜色
    /// </summary>
    public string Foreground { get; set; }

    /// <summary>
    /// 背景颜色
    /// </summary>
    public string Background { get; set; }

    /// <summary>
    /// 字体大小
    /// </summary>
    public double FontSize { get; set; }

    /// <summary>
    /// 字体重量
    /// </summary>
    public string FontWeight { get; set; }

    /// <summary>
    /// 字体
    /// </summary>
    public string FontFamily { get; set; }

    /// <summary>
    /// 方向
    /// 默认水平 Horizontal
    /// Vertical
    /// </summary>
    public string Orientation { get; set; }

    /// <summary>
    /// 水平对齐
    /// </summary>
    public string HorizontalAlignment { get; set; }

    /// <summary>
    /// 垂直对齐
    /// </summary>
    public string VerticalAlignment { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public TextSetting()
    {
        Foreground = "#000000";
        Background = "#FFFFFF";
        FontSize = 15;
        FontWeight = "Regular";
        FontFamily = "pack://application:,,,/Fonts/#Comic Sans MS";
        Orientation = "Horizontal";
        HorizontalAlignment = "Left";
        VerticalAlignment = "Top";
    }

    /// <summary>
    /// 默认
    /// </summary>
    public static readonly TextSetting Default = new();

    #region INotifyPropertyChanged

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning disable CS0067

    #endregion
}