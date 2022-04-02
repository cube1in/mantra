using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace Mantra.Core.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[AddINotifyPropertyChangedInterface]
internal class Text : INotifyPropertyChanged
{
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

    /// <summary>
    /// 文字设置
    /// </summary>
    public TextSetting? Setting { get; set; }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}