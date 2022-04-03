using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class Compare2BooleanConverter : BaseMultiValueConverter<Compare2BooleanConverter>
{
    public override object Convert(object[] values, Type? targetType, object? parameter, CultureInfo culture)
    {
        return parameter switch
        {
            null => values[0] == values[1],
            _ => values[0] != values[1]
        };
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return Array.Empty<object>();
    }
}