using System;
using System.Diagnostics;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
{
    public override object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Find the appropriate page
        switch ((ApplicationPage) value)
        {
            case ApplicationPage.Upload:
                return new UploadPage();

            case ApplicationPage.ImageList:
                return new ImageListPage();

            case ApplicationPage.Scanlation:
                return new ScanPage();

            case ApplicationPage.Revise:
                return new RevisePage();

            case ApplicationPage.Stuff:
                return new StuffPage();

            default:
                Debugger.Break();
                return null;
        }
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}