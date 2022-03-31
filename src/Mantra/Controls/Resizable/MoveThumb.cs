using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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

    #region Override Methods

    protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
    {
        RaiseEvent(e);
        if (DataContext is ToggleButton button)
        {
            // HitTest cannot be used in the container because we need MoveThumb always exist
            // So, override the OnMouseDoubleClick method of MoveThumb to support DoubleClick
            button.IsChecked = true;
            button.Command.Execute(button.CommandParameter);
        }
    }

    #endregion
}