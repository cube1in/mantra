// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class ImageListPage : BasePage
{
    public ImageListPage()
    {
        InitializeComponent();
        DataContext = new ImageListViewModel();
    }
}