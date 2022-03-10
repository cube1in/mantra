// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class UploadPage : BasePage
{
    public UploadPage()
    {
        InitializeComponent();
        DataContext = new UploadViewModel();
    }
}