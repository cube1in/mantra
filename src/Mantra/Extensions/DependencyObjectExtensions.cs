using System;
using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal static class DependencyObjectExtensions
{
    public static bool HasParent(this DependencyObject d, Type type)
    {
        while (true)
        {
            var parent = VisualTreeHelper.GetParent(d);
            if (parent is null) return false;
            if (parent.GetType() == type) return true;

            d = parent;
        }
    }

    public static bool HasParent<T>(this DependencyObject d) where T : DependencyObject
    {
        return HasParent(d, typeof(T));
    }
}