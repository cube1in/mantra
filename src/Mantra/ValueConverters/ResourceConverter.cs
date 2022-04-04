using System;
using System.Globalization;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class ResourceConverter : BaseValueConverter<ResourceConverter>
{
    public override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        return value == null ? null : Application.Current.TryFindResource(value);
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}