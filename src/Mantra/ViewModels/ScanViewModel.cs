using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers.Commands;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 扫描页
/// </summary>
internal class ScanViewModel
{
    #region Private Members

    /// <summary>
    /// HttpClient
    /// </summary>
    private static readonly HttpClient Client = new();

    #endregion

    #region Public Properties

    /// <summary>
    /// 图片网络地址
    /// </summary>
    public string ImgUrl { get; set; } =
        "http://5b0988e595225.cdn.sohucs.com/images/20180408/6c037f1f87ee4f95a1c2b9fefa490eb3.jpeg";

    /// <summary>
    /// 原图片缓存
    /// </summary>
    public byte[]? ImgSource { get; set; }

    /// <summary>
    /// 目标图片缓存
    /// </summary>
    public byte[]? ImgTarget { get; set; }

    /// <summary>
    /// 图片实际宽度
    /// </summary>
    public int ImgPixelWidth { get; set; }

    /// <summary>
    /// 图片实际高度
    /// </summary>
    public int ImgPixelHeight { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// 显示命令
    /// </summary>
    public ICommand ShowCommand => new AsyncCommand(OnShowAsync);

    /// <summary>
    /// 光学识别命令
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public ICommand OCRCommand => new AsyncCommand(OnOCRAsync);

    /// <summary>
    /// 翻译命令
    /// </summary>
    public ICommand TranslateCommand => new AsyncCommand(OnTranslateAsync);

    /// <summary>
    /// 原文字区域
    /// </summary>
    public ObservableCollection<RectItem>? SourceRectItems { get; set; }

    /// <summary>
    /// 目标文字区域
    /// </summary>
    public ObservableCollection<RectItem>? TargetRectItems { get; set; }

    #endregion

    #region Private Methods

    /// <summary>
    /// 显示命令时触发
    /// </summary>
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

    /// <summary>
    /// 光学扫描时触发
    /// </summary>
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

    /// <summary>
    /// 翻译时触发
    /// </summary>
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

    /// <summary>
    /// 获取 Url 图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private static async Task<byte[]> GetBytesFromUrl(string url)
    {
        return await Client.GetByteArrayAsync(url);
    }

    #endregion
}