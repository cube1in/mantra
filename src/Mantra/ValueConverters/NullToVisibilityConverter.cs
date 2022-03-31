using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class NullToVisibilityConverter : BaseValueConverter<NullToVisibilityConverter>
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return parameter switch
        {
            // Inverse
            not null => value is not null ? Visibility.Hidden : Visibility.Visible,
            // Default
            null => value is null ? Visibility.Hidden : Visibility.Visible
        };
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}