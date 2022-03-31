using System;
using System.Globalization;
using Mantra.Core.Models;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class CompareBoundingBoxToBooleanConverter : BaseMultiValueConverter<CompareBoundingBoxToBooleanConverter>
{
    public override object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is BoundingBox current && values[1] is BoundingBox selected)
        {
            return parameter switch
            {
                null => selected == current,
                _ => selected != current
            };
        }

        // values[0] is null
        return false;
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        return Array.Empty<object>();
    }
}