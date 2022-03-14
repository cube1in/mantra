using System.Windows;
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
        base.OnApplyTemplate();
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

    private void Grid_OnDrop(object sender, DragEventArgs e)
    {
        ViewModel.DropHandler(sender, e);
    }
}