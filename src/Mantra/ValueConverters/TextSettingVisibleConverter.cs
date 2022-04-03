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
        if (value is not TextSetting textSetting) return Visibility.Hidden;

        var result = textSetting.Foreground == "#000000" &&
                     textSetting.Background == "#FFFFFF" &&
                     Math.Abs(textSetting.FontSize - 15) < 0.001 &&
                     textSetting.FontWeight == "Regular" &&
                     textSetting.FontFamily == "pack://application:,,,/Fonts/#Comic Sans MS" &&
                     textSetting.Orientation == "Horizontal" &&
                     textSetting.HorizontalAlignment == "Left" &&
                     textSetting.VerticalAlignment == "Top";

        return parameter switch
        {
            null => result ? Visibility.Hidden : Visibility.Visible,
            _ => result ? Visibility.Visible : Visibility.Hidden
        };
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}