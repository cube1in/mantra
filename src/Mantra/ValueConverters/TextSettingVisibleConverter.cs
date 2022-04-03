using System;
using System.Globalization;
using System.Windows;
using Mantra.Core.Models;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class TextSettingVisibleConverter : BaseValueConverter<TextSettingVisibleConverter>
{
    public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TextSetting textSetting) return Visibility.Collapsed;

        var defaultSetting = new TextSetting();
        var result = textSetting.Foreground == defaultSetting.Foreground &&
                     textSetting.Background == defaultSetting.Background &&
                     Math.Abs(textSetting.FontSize - defaultSetting.FontSize) < 0.001 &&
                     textSetting.FontWeight == defaultSetting.FontWeight &&
                     textSetting.FontFamily == defaultSetting.FontFamily &&
                     textSetting.Orientation == defaultSetting.Orientation &&
                     textSetting.HorizontalAlignment == defaultSetting.HorizontalAlignment &&
                     textSetting.VerticalAlignment == defaultSetting.VerticalAlignment;

        return parameter switch
        {
            null => result ? Visibility.Collapsed : Visibility.Visible,
            _ => result ? Visibility.Visible : Visibility.Collapsed
        };
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}