using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class DebugConverter : BaseValueConverter<DebugConverter>
{
    public override object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public override object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}