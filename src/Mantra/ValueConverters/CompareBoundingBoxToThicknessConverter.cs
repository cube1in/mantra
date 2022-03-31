using System;
using System.Globalization;
using System.Windows;
using Mantra.Core.Models;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class CompareBoundingBoxToThicknessConverter : BaseMultiValueConverter<CompareBoundingBoxToThicknessConverter>
{
    public override object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is BoundingBox box1 && values[1] is BoundingBox box2)
        {
            return box1 == box2 ? new Thickness(1) : new Thickness(0);
        }

        // values[0] is null
        return new Thickness(0);
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}