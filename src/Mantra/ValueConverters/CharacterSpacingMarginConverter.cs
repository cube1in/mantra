using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class CharacterSpacingMarginConverter : BaseMultiValueConverter<CharacterSpacingMarginConverter>
{
    public override object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is double horizontal && values[1] is double vertical)
        {
            return new Thickness(0, 0, horizontal, vertical);
        }

        return null;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}