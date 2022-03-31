using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class BitmapHelper
{
    public static BitmapSource ConvertBitmap(Bitmap source)
    {
        return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            source.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
    }

    public static Bitmap BitmapFromSource(BitmapSource bitmapSource)
    {
        using var outStream = new MemoryStream();
        BitmapEncoder enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(bitmapSource));
        enc.Save(outStream);
        var bitmap = new Bitmap(outStream);

        return bitmap;
    }
}