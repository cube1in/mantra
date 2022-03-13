using System;
using System.Globalization;
using System.Windows.Media.Imaging;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class BitmapImageToPixelsConverter : BaseValueConverter<BitmapImageToPixelsConverter>
{
    public override object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string str && Uri.TryCreate(str, UriKind.Absolute, out var uir))
        {
            var bi = new BitmapImage(uir);
            return parameter switch
            {
                "true" => bi.PixelWidth,
                _ => bi.PixelHeight,
            };
        }

        return null;
    }

    public override object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}