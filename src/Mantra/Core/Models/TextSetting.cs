using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace Mantra.Core.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[AddINotifyPropertyChangedInterface]
internal class TextSetting : INotifyPropertyChanged
{
    /// <summary>
    /// 字体颜色
    /// </summary>
    public string Foreground { get; set; } = "#000000";

    /// <summary>
    /// 背景颜色
    /// </summary>
    public string Background { get; set; } = "#FFFFFF";

    /// <summary>
    /// 字体大小
    /// </summary>
    public double FontSize { get; set; } = 15;

    /// <summary>
    /// 字体重量
    /// </summary>
    public string FontWeight { get; set; } = "Regular";

    /// <summary>
    /// 字体
    /// </summary>
    public string FontFamily { get; set; } = "pack://application:,,,/Fonts/#Comic Sans MS";

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

    /// <summary>
    /// 垂直对齐
    /// </summary>
    public string VerticalAlignment { get; set; } = "Top";

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}