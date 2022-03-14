using System;
using System.Globalization;
using System.IO;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class FilepathBytesConverter:BaseValueConverter<FilepathBytesConverter>
{
    public override object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string path && File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }

        return null;
    }

    public override object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}