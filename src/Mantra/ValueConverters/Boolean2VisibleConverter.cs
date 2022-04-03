using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class Boolean2VisibleConverter : BaseValueConverter<Boolean2VisibleConverter>
{
    public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            return parameter switch
            {
                null => boolean ? Visibility.Visible : Visibility.Collapsed,
                _ => boolean ? Visibility.Collapsed : Visibility.Visible
            };
        }

        throw new NotSupportedException();
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}