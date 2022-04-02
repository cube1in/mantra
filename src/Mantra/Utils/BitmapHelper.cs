using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
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

    public static Bitmap InternalRender(FrameworkElement element, System.Windows.Size size)
    {
        // As the control has no parent container,
        // you need to call Measure and Arrange in order to do a proper layout.
        element.Measure(size);
        element.Arrange(new Rect(size));
        element.UpdateLayout();

        var renderTargetBitmap = new RenderTargetBitmap((int) element.ActualWidth, (int) element.ActualHeight, 96, 96,
            PixelFormats.Pbgra32);
        renderTargetBitmap.Render(element);

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

        using var stream = new MemoryStream();
        encoder.Save(stream);

        return new Bitmap(stream);
    }
}