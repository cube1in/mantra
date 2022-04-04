// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class CollectionPage
{
    private CollectionViewModel ViewModel => (DataContext as CollectionViewModel)!;

    public CollectionPage()
    {
        InitializeComponent();
        DataContext = new CollectionViewModel();
    }

    public override void OnApplyTemplate()
    {
        ViewModel.Initialize(PushValue);
    }
}