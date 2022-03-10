// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class ScanPage : BasePage
{
    public ScanPage()
    {
        InitializeComponent();
        DataContext = new ScanViewModel();
    }
}