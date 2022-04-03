using System;
using System.Drawing;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class BitmapConverter : BaseValueConverter<BitmapConverter>
{
    public override object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is Bitmap bitmap)
        {
            return BitmapHelper.ConvertBitmap(bitmap);
        }

        return null;
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}