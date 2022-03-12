using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Point = System.Windows.Point;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 扫描页
/// </summary>
internal class ScanViewModel : BaseViewModel
{
    #region Private Members

    /// <summary>
    /// HttpClient
    /// </summary>
    private static readonly HttpClient Client = new();

    #endregion

    #region Public Properties

    /// <summary>
    /// 原图片缓存
    /// </summary>
    public byte[]? ImgSource { get; set; }

    /// <summary>
    /// 图片实际宽度
    /// </summary>
    public int ImgPixelWidth { get; set; }

    /// <summary>
    /// 图片实际高度
    /// </summary>
    public int ImgPixelHeight { get; set; }

    /// <summary>
    /// 原文字区域
    /// </summary>
    public ObservableCollection<Rect> SourceRectItems { get; set; } = new();

    #endregion

    #region Commands

    /// <summary>
    /// 光学识别命令
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public ICommand OCRCommand => new AsyncCommand(OnOCRAsync);

    /// <summary>
    /// 移除框命令
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ICommand RemoveCommand => new AsyncCommand<Rect>(OnRemoveAsync);

    #endregion

    #region Private Methods

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

        // var blocks = Tesseact.DoOCR1(ImgSource);
        // SourceRectItems = new();
        //
        // if (blocks.Any())
        // {
        //     foreach (var block in blocks)
        //     {
        //         var rect = block.BoundingBox;
        //         if (rect != null)
        //         {
        //             SourceRectItems.Add(new Rect
        //                 {Left = rect.Value.X1, Top = rect.Value.Y1, Height = rect.Value.Height, Width = rect.Value.Width});
        //         }
        //     }
        // }

        // await Task.CompletedTask;
        // ReSharper disable once StringLiteralTypo
        var regions = await Tesseact.DoOCRAsync(ImgSource, "eng_best");
        SourceRectItems = new ObservableCollection<Rect>(from region in regions
            select new Rect {Left = region.Left, Top = region.Top, Width = region.Width, Height = region.Height});
    }

    private async Task OnRemoveAsync(Rect rect)
    {
        SourceRectItems.Remove(rect);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Set image original size
    /// </summary>
    private void SetOriginalSize()
    {
        if (ImgSource == null) return;

        // Get image original size
        var bitmap = new Bitmap(new MemoryStream(ImgSource));
        ImgPixelHeight = bitmap.Height;
        ImgPixelWidth = bitmap.Width;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    public async Task InitializeAsync(object? pushValue)
    {
        if (pushValue is string path)
        {
            ImgSource = await File.ReadAllBytesAsync(path);
            SetOriginalSize();
        }
    }

    private bool _dragInProgress;
    private Rect? _createdRect;
    private Point _lastPoint;

    public void HandleMouseDown(object sender, MouseButtonEventArgs e)
    {
        var point = Mouse.GetPosition((Canvas) sender);
        _createdRect = new Rect {Left = point.X, Top = point.Y};
        _dragInProgress = true;
        SourceRectItems.Add(_createdRect);
        
        // 加上一个值，否则鼠标将会永远在 Rectangle 上，导致 MouseUp 永远无法触发
        _lastPoint = new Point(point.X + 10, point.Y + 10);
    }

    public void HandleMouseMove(object sender, MouseEventArgs e)
    {
        if (_dragInProgress && _createdRect != null)
        {
            // 查看鼠标移动了多少
            var point = Mouse.GetPosition((Canvas) sender);
            var offsetX = point.X - _lastPoint.X;
            var offsetY = point.Y - _lastPoint.Y;

            // 更新位置
            _createdRect.Width += offsetX;
            _createdRect.Height += offsetY;
            SourceRectItems.Remove(_createdRect);
            SourceRectItems.Add(_createdRect);

            // 保存鼠标位置
            _lastPoint = point;
        }
    }

    public void HandleMouseUp(object sender, MouseButtonEventArgs e)
    {
        _dragInProgress = false;
        _createdRect = null;
    }

    #endregion
}