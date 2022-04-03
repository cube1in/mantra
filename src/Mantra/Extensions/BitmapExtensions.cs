using System.Drawing;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class BitmapExtensions
{
    public static Bitmap Replace(this Bitmap source, Bitmap replace, int x, int y)
    {
        var graphics = Graphics.FromImage(source);
        graphics.DrawImageUnscaled(replace, x, y);
        graphics.Dispose();

        return source;
    }

    public static Bitmap Crop(this Bitmap source, Rectangle rect)
    {
        return source.Clone(rect, source.PixelFormat);
    }
}