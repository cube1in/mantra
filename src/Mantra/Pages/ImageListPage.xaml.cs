// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class ImageListPage
{
    private ImageListViewModel ViewModel => (DataContext as ImageListViewModel)!;

    public ImageListPage()
    {
        InitializeComponent();
        DataContext = new ImageListViewModel();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        ViewModel.Initialized(PushValue);
    }
}