using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Mantra;

internal class ResizeThumb : Thumb
{
    public ResizeThumb()
    {
        DragDelta += HandleDragDelta;
    }

    private void HandleDragDelta(object? sender, DragDeltaEventArgs e)
    {
        // DataContext is DesignItem
        if (DataContext is Control item)
        {
            double deltaVertical, deltaHorizontal;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, item.ActualHeight - item.MinHeight);
                    item.Height -= deltaVertical;
                    break;
                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, item.ActualHeight - item.MinHeight);
                    Canvas.SetTop(item, Canvas.GetTop(item) + deltaVertical);
                    item.Height -= deltaVertical;
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, item.ActualWidth - item.MinWidth);
                    Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                    item.Width -= deltaHorizontal;
                    break;
                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, item.ActualWidth - item.MinWidth);
                    item.Width -= deltaHorizontal;
                    break;
            }
        }

        e.Handled = true;
    }
}