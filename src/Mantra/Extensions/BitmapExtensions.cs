using System.Drawing;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class BitmapExtensions
{
    public static Bitmap Replace(this Bitmap source, Bitmap replace, float x, float y)
    {
        var graphics = Graphics.FromImage(source);
        graphics.DrawImage(replace, x, y);
        graphics.Dispose();

        return source;
    }
}