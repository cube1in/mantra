using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class FontFamilyComboBoxConverter : BaseMultiValueConverter<FontFamilyComboBoxConverter>
{
    private ComboBox _comboBox = null!;

    public override object? Convert(object[] values, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is string str && values[1] is ComboBox comboBox)
        {
            _comboBox = comboBox;
            var source = (Application.Current.TryFindResource(str) as FontFamily)?.Source;
            if (source != null) str = source;

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.FontFamily.Source == str)
                {
                    return item;
                }
            }

            return comboBox.Items[0];
        }

        return values[0];
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        if (value is ComboBoxItem item)
        {
            return new[] {item.Tag, _comboBox};
        }

        throw new NotSupportedException();
    }
}