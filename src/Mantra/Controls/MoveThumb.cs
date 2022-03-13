using System.Windows.Controls;
using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 拖拽移动Thumb
/// </summary>
internal class MoveThumb : Thumb
{
    #region Construction

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public MoveThumb()
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
        // DataContext is DesignerContainer
        if (DataContext is ContentControl container)
        {
            var left = container.GetCanvasLeftWithCascade(out var element);
            var top = container.GetCanvasTopWithCascade(out element);

            Canvas.SetLeft(element, left + e.HorizontalChange);
            Canvas.SetTop(element, top + e.VerticalChange);
        }
    }

    #endregion
}