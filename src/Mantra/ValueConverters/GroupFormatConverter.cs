using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class GroupFormatConverter : BaseValueConverter<GroupFormatConverter>
{
    public override object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is 0 ? "# 未分组" : $"# {value}";
    }

    public override object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}