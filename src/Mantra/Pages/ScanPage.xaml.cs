using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class ScanPage
{
    private ScanViewModel ViewModel => (DataContext as ScanViewModel)!;

    public ScanPage()
    {
        InitializeComponent();
        DataContext = new ScanViewModel();
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
        ViewModel.MouseMoveHandler(sender,  e);
    }
}