using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

// ReSharper disable once InconsistentNaming
internal static class UIElementExtensions
{
    /// <summary>
    /// 级联获取 CanvasTop
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double GetCanvasTopWithCascade(this UIElement? element, out UIElement value)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        double top;
        while (true)
        {
            top = Canvas.GetTop(element);
            if (top is double.NaN)
            {
                if (VisualTreeHelper.GetParent(element) is UIElement parent)
                {
                    element = parent;
                    continue;
                }
            }

            break;
        }

        value = element;
        return top;
    }

    /// <summary>
    /// 级联获取 CanvasLeft
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double GetCanvasLeftWithCascade(this UIElement? element, out UIElement value)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        double left;
        while (true)
        {
            left = Canvas.GetLeft(element);
            if (left is double.NaN)
            {
                if (VisualTreeHelper.GetParent(element) is UIElement parent)
                {
                    element = parent;
                    continue;
                }
            }

            break;
        }

        value = element;
        return left;
    }
}