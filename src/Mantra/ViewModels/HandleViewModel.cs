using System.Collections.ObjectModel;
using MvvmHelpers.Commands;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using Mantra.Plugins;
using Point = System.Windows.Point;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 处理页视图模型
/// </summary>
internal class HandleViewModel : BaseViewModel
{
    #region Private Members

    /// <summary>
    /// 计算机视觉
    /// </summary>
    private readonly IComputerVision _computerVision = new TesseactComputerVision();

    /// <summary>
    /// 翻译器
    /// </summary>
    private readonly ITranslatorText _translator = new BaiduTranslatorText();

    #endregion

    #region Public Properties

    /// <summary>
    /// 原图片缓存
    /// </summary>
    public string? ImgSource { get; set; }

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
    public ObservableCollection<BoundingBox> BoundingBoxCollection { get; set; } = new();

    /// <summary>
    /// 选中文字区域
    /// </summary>
    public BoundingBox? SelectedBoundingBox { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// 批量光学识别命令
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public ICommand BatchOCRCommand => new AsyncCommand(OnBatchOCRAsync);

    /// <summary>
    /// 单个光学识别命令
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public ICommand SingleOCRCommand => new AsyncCommand<BoundingBox>(OnSingleOCRAsync);

    /// <summary>
    /// 移除框命令
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ICommand RemoveBoundingBoxCommand => new Command<BoundingBox>(OnRemoveBoundingBox);

    /// <summary>
    /// 单个翻译命令
    /// </summary>
    public ICommand SingleTranslateCommand => new AsyncCommand<BoundingBox>(OnSingleTranslateAsync);

    /// <summary>
    /// 消除换行
    /// </summary>
    public ICommand RemoveNewLineCommand => new Command<BoundingBox>(OnRemoveNewLine);

    /// <summary>
    /// 选中命令
    /// </summary>
    public ICommand SelectBoundingBoxCommand => new Command<BoundingBox>(OnSelectBoundingBox);

    /// <summary>
    /// 添加墨水命令
    /// </summary>
    public ICommand AddInkCommand => new Command<BoundingBox>(OnAddInk);

    /// <summary>
    /// 移除墨水命令
    /// </summary>
    public ICommand RemoveInkCommand => new Command<BoundingBox>(OnRemoveInk);

    #endregion

    #region Private Methods

    /// <summary>
    /// <see cref="BatchOCRCommand"/> 时触发
    /// </summary>
    private async Task OnBatchOCRAsync()
    {
        if (ImgSource == null)
        {
            MessageBox.Show("图片不存在", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var boxes = await _computerVision.ReadFileLocalAsync(ImgSource, "eng");
        BoundingBoxCollection = new ObservableCollection<BoundingBox>(boxes);
    }

    /// <summary>
    /// <see cref="SingleOCRCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private async Task OnSingleOCRAsync(BoundingBox value)
    {
        var bitmap = CropImage(ImgSource!,
            new Rectangle
            {
                X = (int) value.Left,
                Y = (int) value.Top,
                Width = (int) value.Width,
                Height = (int) value.Height
            });

        var converter = new ImageConverter();
        var bytes = (byte[]) converter.ConvertTo(bitmap, typeof(byte[]))!;

        var boxes = await _computerVision.ReadFileStreamAsync(bytes, "eng");
        value.OriginalText = string.Join(string.Empty, from box in boxes select box.OriginalText);

        // Set selected
        SelectedBoundingBox = value;

        // Translate text
        await OnSingleTranslateAsync(value);
    }

    /// <summary>
    /// <see cref="RemoveBoundingBoxCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveBoundingBox(BoundingBox value)
    {
        BoundingBoxCollection.Remove(value);
        if (value == SelectedBoundingBox) SelectedBoundingBox = null;
    }

    /// <summary>
    /// <see cref="SingleTranslateCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private async Task OnSingleTranslateAsync(BoundingBox value)
    {
        value.TranslatedText = await _translator.TranslateAsync(value.OriginalText, "en", "zh");
    }

    /// <summary>
    /// <see cref="RemoveNewLineCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveNewLine(BoundingBox value)
        => value.OriginalText = value.OriginalText.Replace("\n", " ").Replace("\r", " ");

    /// <summary>
    /// <see cref="SelectBoundingBoxCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnSelectBoundingBox(BoundingBox value)
        => SelectedBoundingBox = value;

    /// <summary>
    /// <see cref="AddInkCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnAddInk(BoundingBox value)
    {
        var bitmap = CropImage(ImgSource!,
            new Rectangle
            {
                X = (int) value.Left,
                Y = (int) value.Top,
                Width = (int) value.Width,
                Height = (int) value.Height
            });

        var color = bitmap.GetPixel(0, 0);
        value.Ink = new Ink
        {
            Background = Colors.AsString(color)
        };
    }

    /// <summary>
    /// <see cref="RemoveInkCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveInk(BoundingBox value)
        => value.Ink = null;

    /// <summary>
    /// Set image original size
    /// </summary>
    private void SetOriginalSize()
    {
        if (ImgSource == null) return;

        // Get image original size
        var bitmap = new Bitmap(ImgSource);
        ImgPixelHeight = bitmap.Height;
        ImgPixelWidth = bitmap.Width;
    }

    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cropArea"></param>
    /// <returns></returns>
    private static Bitmap CropImage(string path, Rectangle cropArea)
    {
        var bitmap = new Bitmap(path);
        return bitmap.Clone(cropArea, bitmap.PixelFormat);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    public void Initialize(object? pushValue)
    {
#if DEBUG
        pushValue ??= @"C:\Users\sou1m\Desktop\Mantra\test.png";
#endif
        if (pushValue is string path)
        {
            ImgSource = path;
            SetOriginalSize();
        }
    }

    #region Handle Create Rect

    /// <summary>
    /// 是否在拖拽过程中
    /// </summary>
    private bool _dragInProgress;

    /// <summary>
    /// 创建的 Rect
    /// </summary>
    private BoundingBox? _createdBoundingBox;

    /// <summary>
    /// 鼠标拖动过程中最后记录的位置
    /// </summary>
    private Point _lastPoint;

    /// <summary>
    /// 创建矩形边的最小值
    /// 创建时小于此值将会被移除
    /// </summary>
    private const double MinSize = 25;

    /// <summary>
    /// 鼠标左键按下时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MouseDownHandler(object sender, MouseButtonEventArgs e)
    {
        var el = (UIElement) sender;
        var point = Mouse.GetPosition(el);
        _createdBoundingBox = new BoundingBox {Left = point.X, Top = point.Y};
        _dragInProgress = true;
        BoundingBoxCollection.Add(_createdBoundingBox);

        _lastPoint = point;

        // https://docs.microsoft.com/zh-cn/previous-versions/ms771301(v=vs.100)?redirectedfrom=MSDN
        // 强制捕获鼠标，否则在产生Rectangle后，由于鼠标后续将会在新产生的Rectangle控件上，
        // 导致无法触发 MouseUp 事件。
        // 当一个对象捕获鼠标时，所有与鼠标相关的事件都被视为具有鼠标捕获的对象执行该事件，即使鼠标指针位于另一个对象之上。
        el.CaptureMouse();
    }

    /// <summary>
    /// 鼠标移动时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        if (_dragInProgress && _createdBoundingBox != null)
        {
            // 查看鼠标移动了多少
            var point = Mouse.GetPosition((Canvas) sender);
            var offsetX = point.X - _lastPoint.X;
            var offsetY = point.Y - _lastPoint.Y;

            // 更新位置
            _createdBoundingBox.Width += offsetX;
            _createdBoundingBox.Height += offsetY;

            // 保存鼠标位置
            _lastPoint = point;
        }
    }

    /// <summary>
    /// 鼠标左键抬起时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MouseUpHandler(object sender, MouseButtonEventArgs e)
    {
        // 如果小于 MinSize 将不会被创建
        if (_createdBoundingBox != null &&
            (_createdBoundingBox.Width <= MinSize || _createdBoundingBox.Height <= MinSize))
        {
            BoundingBoxCollection.Remove(_createdBoundingBox);
        }

        _dragInProgress = false;
        _createdBoundingBox = null;

        var el = (UIElement) sender;
        // 释放强制捕获的鼠标
        el.ReleaseMouseCapture();
    }

    #endregion

    #endregion
}