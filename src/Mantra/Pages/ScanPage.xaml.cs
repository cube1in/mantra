// ReSharper disable once CheckNamespace

namespace Mantra;

internal partial class ScanPage : BasePage
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
}