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

    public override async void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        await ViewModel.InitializeAsync(PushValue);
    }

    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.HandleMouseDown(sender, e);
    }

    private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        ViewModel.HandleMouseUp(sender, e);
    }

    private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
    {
        ViewModel.HandleMouseMove(sender,  e);
    }
}