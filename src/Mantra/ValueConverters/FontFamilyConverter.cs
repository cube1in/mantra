using System;
using System.Globalization;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class FontFamilyConverter:BaseValueConverter<FontFamilyConverter>
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ComboBoxItem item)
        {
            return item.FontFamily;
        }

        throw new NotSupportedException();
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}