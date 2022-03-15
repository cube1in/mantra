using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class InverseBooleanConverter : BaseValueConverter<InverseBooleanConverter>
{
    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolean)
        {
            return !boolean;
        }

        throw new NotSupportedException();
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}