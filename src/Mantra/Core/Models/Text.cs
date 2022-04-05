using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Mantra.Core.Abstractions;
using PropertyChanged;

namespace Mantra.Core.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[AddINotifyPropertyChangedInterface]
internal class Text : INotifyPropertyChanged, ICloneable<Text>
{
    /// <summary>
    /// 矩形内的源词
    /// 必须具有默认值，否则在传到翻译 api 时会导致插入值错误
    /// </summary>
    public string OriginalText { get; set; }

    /// <summary>
    /// 矩形内的翻译
    /// 必须具有默认值，否则在传到翻译 api 时会导致插入值错误
    /// </summary>
    public string TranslatedText { get; set; }

    /// <summary>
    /// 文字设置
    /// </summary>
    public TextSetting Setting { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public Text()
    {
        OriginalText = "没有识别结果";
        TranslatedText = "没有翻译结果";
        Setting = new TextSetting();
    }

    #region INotifyPropertyChanged

#pragma warning disable CS0067
    public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning disable CS0067

    #endregion

    public Text Clone() => (Text) MemberwiseClone();
}