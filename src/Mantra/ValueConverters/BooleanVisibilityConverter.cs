using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class BooleanVisibilityConverter : BaseValueConverter<BooleanVisibilityConverter>
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            if (parameter != null) boolean = !boolean;

            return boolean switch
            {
                true => Visibility.Visible,
                _ => Visibility.Hidden
            };
        }

        throw new NotSupportedException();
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}