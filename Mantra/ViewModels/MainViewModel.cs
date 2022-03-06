using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mantra;

internal class MainViewModel
{
    public string ImgUrl { get; set; } = "https://raw.githubusercontent.com/mantra-inc/open-mantra-dataset/main/images/tojime_no_siora/ja/010.jpg";

    public string ImgSource { get; set; }

    public string ImgTarget { get; set; }

    public ICommand ShowCommand => new AsyncCommand(OnShowAsync);

    public ICommand OCRCommand => new AsyncCommand(OnOCRAsync);

    public ICommand TranslateCommand => new AsyncCommand(OnTranslateAsync);


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
        await Task.CompletedTask;
    }

    private async Task OnTranslateAsync()
    {
        await Task.CompletedTask;
    }
}
