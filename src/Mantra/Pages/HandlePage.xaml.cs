using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class HandlePage
{
    private HandleViewModel ViewModel => (DataContext as HandleViewModel)!;

    public HandlePage()
    {
        InitializeComponent();
        DataContext = new HandleViewModel();
    }

    public override void OnApplyTemplate()
    {
        ViewModel.Initialize(PushValue);
    }

    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.MouseDownHandler(sender, e);
    }

    private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        ViewModel.MouseUpHandler(sender, e);
    }

    private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
    {
        ViewModel.MouseMoveHandler(sender, e);
    }

    private void DownloadButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (FindName("CanvasItemsControl") is not ItemsControl itemsControl) return;

        var bitmaps = (from textPadding in itemsControl.GetVisualDescendents<TextPadding>()
                where textPadding.Name == "TextPadding" && textPadding.IsVisible
                select BitmapHelper.InternalRender(textPadding, new Size(textPadding.ActualWidth, textPadding.ActualHeight)))
            .ToList();

        ViewModel.DownloadHandler(bitmaps);
    }
}