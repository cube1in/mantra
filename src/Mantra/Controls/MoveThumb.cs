using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class MoveThumb : Thumb
{
    public MoveThumb()
    {
        DragDelta += HandleDragDelta;
    }

    private void HandleDragDelta(object? sender, DragDeltaEventArgs e)
    {
        // DataContext is DesignerItem
        if (DataContext is Control item)
        {
            var left = Canvas.GetLeft(item);
            var top = Canvas.GetTop(item);

            Canvas.SetLeft(item, left + e.HorizontalChange);
            Canvas.SetTop(item, top + e.VerticalChange);
        }
    }
}