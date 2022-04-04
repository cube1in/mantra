using System;
using System.Collections.Generic;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class ApplicationPageConverter : BaseValueConverter<ApplicationPageConverter>
{
    private static readonly Dictionary<ApplicationPage, BasePage> HistoryPages = new();

    public override object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ApplicationCurrentPage currentPage) throw new NotSupportedException();

        if (HistoryPages.TryGetValue(currentPage.ApplicationPage, out var history) && currentPage.UseCache)
        {
            // 将动画改为加载动画
            history.ShouldAnimateOut = false;
            return history;
        }

        BasePage page = currentPage.ApplicationPage switch
        {
            ApplicationPage.Upload => new UploadPage(),
            ApplicationPage.Collection => new CollectionPage(),
            ApplicationPage.Handle => new HandlePage(),
            _ => throw new NotSupportedException()
        };

        if (!HistoryPages.TryAdd(currentPage.ApplicationPage, page))
        {
            HistoryPages.Remove(currentPage.ApplicationPage);
            HistoryPages.Add(currentPage.ApplicationPage, page);
        }

        return page;
    }

    public override object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}