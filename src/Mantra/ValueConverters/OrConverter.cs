using System;
using System.Globalization;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class OrConverter : BaseMultiValueConverter<OrConverter>
{
    public override object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.All(x=>x is bool))
        {
            return values.Cast<bool>().Any(x => x);
        }

        throw new NotSupportedException();
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}