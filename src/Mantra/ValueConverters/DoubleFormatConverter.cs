using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class DoubleFormatConverter : BaseValueConverter<DoubleFormatConverter>
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double nb)
        {
            return Math.Round(nb);
        }

        throw new NotSupportedException();
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}