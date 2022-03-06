using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mantra;

internal class MainViewModel : BaseViewModel
{
    #region Private Members


    #endregion

    #region Public Properties

    public string ImgUrl { get; set; } = "https://bddhaec337xvm.xnvda7fch4zhr.mangadex.network/BtSoon9hY3za7r4lE-NDSYT29ek0_Sj4O_gNA1LzZHnV5IQbqvry-TcNVadrZngmgwigJuxOtu3PpOGEmZTnE4ZDSCN3tfGIpBtmNK4cZgOucsEM7BDQed12NoIhF7I5mwx54oTXZWQpYYOe20KqVeePrDQVtEmU7gPrfQraRjDZ29u9IXh_EICgJk8nI24y/data/e086fcedec23c63a6af7f766e2ac54b1/M5-60a4f587b56f77f5b72419b24e7d2f14693104a18e12fef7122eac1c84c8f776.png";

    public string? ImgSource { get; set; }

    public string? ImgTarget { get; set; }

    public ICommand ShowCommand => new AsyncCommand(OnShowAsync);

    public ICommand OCRCommand => new AsyncCommand(OnOCRAsync);

    public ICommand TranslateCommand => new AsyncCommand(OnTranslateAsync);

    #endregion

    #region Private Methods

    private async Task OnShowAsync()
    {
        if (!Uri.IsWellFormedUriString(ImgUrl, UriKind.Absolute))
        {
            // show warning message
            return;
        }

        ImgSource = ImgTarget = ImgUrl;
        await Task.CompletedTask;
    }

    private async Task OnOCRAsync()
    {
        if (!Uri.IsWellFormedUriString(ImgSource, UriKind.Absolute))
        {
            // show warning message
            return;
        }

        await Baidu.DoOCRAsync(ImgSource);
    }

    private async Task OnTranslateAsync()
    {
        await Task.CompletedTask;
    }

    #endregion
}
