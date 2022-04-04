using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mantra.Core.Models;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class TextPadding : Border
{
    /// <summary>
    /// 文字依赖属性
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(Text), typeof(TextPadding),
            new PropertyMetadata(OnTextPropertyChanged));

    /// <summary>
    /// 文字
    /// </summary>
    public Text Text
    {
        get => (Text) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// <see cref="TextProperty"/> 更改后调用
    /// </summary>
    /// <param name="d">DependencyObject</param>
    /// <param name="e">DependencyPropertyChangedEventArgs</param>
    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue != e.NewValue)
        {
            var padding = (TextPadding) d;
            var text = (Text) e.NewValue;
            var setting = text.Setting;
            var converter = new BrushConverter();
            var background = (SolidColorBrush) converter.ConvertFromString(setting.Background)!;

            padding.SnapsToDevicePixels = true;
            padding.Background = background;
            padding.Child = new TextBlock
            {
                Text = text.TranslatedText,
                Background = background,
                Foreground = (SolidColorBrush) converter.ConvertFromString(setting.Foreground)!,
                FontSize = setting.FontSize,
                FontFamily = (FontFamily) Application.Current.FindResource(setting.FontFamily)!,
                FontWeight = (FontWeight) new FontWeightConverter().ConvertFromString(setting.FontWeight)!,
                HorizontalAlignment = Enum.Parse<HorizontalAlignment>(setting.HorizontalAlignment),
                VerticalAlignment = Enum.Parse<VerticalAlignment>(setting.VerticalAlignment),
                TextWrapping = TextWrapping.Wrap
            };
        }
    }
}