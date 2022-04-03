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
        var result = value as TextSetting == TextSetting.Default;
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