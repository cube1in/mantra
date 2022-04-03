using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class ApplicationPageConverter : BaseValueConverter<ApplicationPageConverter>
{
    public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        // Find the appropriate page
        return value switch
        {
            ApplicationPage.Upload => new UploadPage(),
            ApplicationPage.Collection => new CollectionPage(),
            ApplicationPage.Handle => new HandlePage(),
            _ => throw new NotSupportedException()
        };
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}