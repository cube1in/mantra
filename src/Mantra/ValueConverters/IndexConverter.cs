using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Mantra.Core.Models;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class IndexConverter : BaseMultiValueConverter<IndexConverter>
{
    public override object Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values[0] is ObservableCollection<BoundingBox> array && values[1] is BoundingBox item)
        {
            return array.IndexOf(item) + 1;
        }

        throw new NotSupportedException();
    }

    public override object[] ConvertBack(object value, Type[] targetTypes, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}