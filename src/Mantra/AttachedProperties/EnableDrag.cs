using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class EnableDrag : BaseAttachedProperty<EnableDrag, bool>
{
    protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not UIElement uiElement || e.NewValue is bool == false)
        {
            return;
        }

        if ((bool) e.NewValue)
        {
            uiElement.MouseMove += UIElementOnMouseMove;
        }
        else
        {
            uiElement.MouseMove -= UIElementOnMouseMove;
        }
    }

    private static void UIElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
    {
        if (sender is UIElement uiElement)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                DependencyObject parent = uiElement;
                var avoidInfiniteLoop = 0;
                // Search up the visual tree to find the first parent window.
                while (parent is not Window)
                {
                    parent = VisualTreeHelper.GetParent(parent)!;
                    avoidInfiniteLoop++;
                    if (avoidInfiniteLoop == 1000)
                    {
                        // Something is wrong - we could not find the parent window.
                        return;
                    }
                }

                var window = parent as Window;
                window?.DragMove();
            }
        }
    }
}