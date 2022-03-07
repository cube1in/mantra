using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class MainViewModel : BaseViewModel
{
    #region Private Members

    private static readonly HttpClient Client = new();

    #endregion

    #region Public Properties

    public string ImgUrl { get; set; } =
        "http://5b0988e595225.cdn.sohucs.com/images/20180408/6c037f1f87ee4f95a1c2b9fefa490eb3.jpeg";

    public byte[]? ImgSource { get; set; }

    public byte[]? ImgTarget { get; set; }

    public int ImgPixelWidth { get; set; }

    public int ImgPixelHeight { get; set; }

    public ICommand ShowCommand => new AsyncCommand(OnShowAsync);

    // ReSharper disable once InconsistentNaming
    public ICommand OCRCommand => new AsyncCommand(OnOCRAsync);

    public ICommand TranslateCommand => new AsyncCommand(OnTranslateAsync);

    public ObservableCollection<RectItem>? SourceRectItems { get; set; }

    public ObservableCollection<RectItem>? TargetRectItems { get; set; }

    #endregion

    #region Private Methods

    private async Task OnShowAsync()
    {
        if (!Uri.IsWellFormedUriString(ImgUrl, UriKind.Absolute))
        {
            // show warning message
            return;
        }

        var buffer = await GetBytesFromUrl(ImgUrl);
        ImgSource = ImgTarget = buffer;

        // Get image original size
        var bitmap = new Bitmap(new MemoryStream(buffer));
        ImgPixelHeight = bitmap.Height;
        ImgPixelWidth = bitmap.Width;
    }

    private async Task OnOCRAsync()
    {
        if (ImgSource == null)
        {
            // show warning message
            return;
        }

        // var ocrResponse = await Baidu.DoOCRAsync(Client, new MemoryStream(ImgSource));
        //
        // if (ocrResponse != null)
        // {
        //     SourceRectItems = new ObservableCollection<RectItem>(from context in ocrResponse.WordsResult
        //         select new RectItem
        //         {
        //             Left = context.Location.Left, Top = context.Location.Top, Width = context.Location.Width,
        //             Height = context.Location.Height
        //         });
        // }

        // ReSharper disable once StringLiteralTypo
        var regions = await Tesseact.DoOCRAsync(ImgSource, "en");
        SourceRectItems = new ObservableCollection<RectItem>(from region in regions
            select new RectItem {Left = region.Left, Top = region.Top, Width = region.Width, Height = region.Height});
    }

    private async Task OnTranslateAsync()
    {
        if (ImgSource == null)
        {
            // show warning message
            return;
        }

        var transResponse = await Baidu.DoTranslateAsync(Client, new MemoryStream(ImgSource));
        await Task.CompletedTask;
    }

    private static async Task<byte[]> GetBytesFromUrl(string url)
    {
        return await Client.GetByteArrayAsync(url);
    }

    #endregion
}

internal class RectItem
{
    public int Left { get; set; }

    public int Top { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}