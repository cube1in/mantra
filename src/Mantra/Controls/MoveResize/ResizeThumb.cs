using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 调整大小Thumb 只更新 DesignerItem 的宽度、高度和/或位置，具体取决于 ResizeThumb 的垂直和水平对齐方式。
/// </summary>
internal class ResizeThumb : Thumb
{
    #region Construction

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public ResizeThumb()
    {
        DragDelta += HandleDragDelta;
    }

    /// <summary>
    /// 处理拖动
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">DragDeltaEventArgs</param>
    private void HandleDragDelta(object? sender, DragDeltaEventArgs e)
    {
        // DataContext is DesignItem
        if (DataContext is ContentControl designerItem)
        {
            double deltaVertical, deltaHorizontal;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                    designerItem.Height -= deltaVertical;
                    break;
                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                    var top = designerItem.GetCanvasTopWithCascade(out var element);
                    Canvas.SetTop(element, top + deltaVertical);
                    designerItem.Height -= deltaVertical;
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                    var left = designerItem.GetCanvasLeftWithCascade(out var element);
                    Canvas.SetLeft(element, left + deltaHorizontal);
                    designerItem.Width -= deltaHorizontal;
                    break;
                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                    designerItem.Width -= deltaHorizontal;
                    break;
            }
        }

        e.Handled = true;
    }

    #endregion
}