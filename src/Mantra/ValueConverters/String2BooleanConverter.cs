using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class String2BooleanConverter : BaseValueConverter<String2BooleanConverter>
{
    public override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str1 && parameter is string str2) return str1 == str2;

        throw new NotSupportedException();
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string str) return str;
        throw new NotSupportedException();
    }
}