using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class Compare2ThicknessConverter : BaseMultiValueConverter<Compare2ThicknessConverter>
{
    public override object Convert(object[] values, Type? targetType, object? parameter, CultureInfo culture)
    {
        return values[0] == values[1] ? new Thickness(1) : new Thickness(0);
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}