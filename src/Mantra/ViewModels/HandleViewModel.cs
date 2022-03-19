using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using Mantra.Plugins;
using Mantra.Translators.Baidu;
using Point = System.Windows.Point;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 处理页视图模型
/// </summary>
internal class HandleViewModel : BaseViewModel, IDropTarget
{
    #region Private Members

    /// <summary>
    /// 计算机视觉
    /// </summary>
    private readonly IComputerVision _computerVision = new TesseactComputerVision();

    /// <summary>
    /// 翻译器
    /// </summary>
    private readonly ITranslatorText _translator = new Baidu();

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
    public ObservableCollection<BoundingBox> RectItems { get; set; } = new();

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
    public ICommand RemoveCommand => new Command<BoundingBox>(OnRemove);

    /// <summary>
    /// 移除组命令
    /// </summary>
    public ICommand RemoveGroupCommand => new Command<int>(OnRemoveGroup);

    /// <summary>
    /// 批量翻译命令
    /// </summary>
    public ICommand BatchTranslateCommand => new AsyncCommand<int>(OnBatchTranslateAsync);

    /// <summary>
    /// 单个翻译命令
    /// </summary>
    public ICommand SingleTranslateCommand => new AsyncCommand<BoundingBox>(OnSingleTranslateAsync);

    /// <summary>
    /// 消除换行
    /// </summary>
    public ICommand RemoveNewLineCommand => new Command<BoundingBox>(OnRemoveNewLine);

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
        RectItems = new ObservableCollection<BoundingBox>(boxes);
    }

    /// <summary>
    /// <see cref="SingleOCRCommand"/> 时触发
    /// </summary>
    /// <param name="item"></param>
    private async Task OnSingleOCRAsync(BoundingBox item)
    {
        var bitmap = CropImage(ImgSource!,
            new Rectangle
            {
                X = (int) item.Left,
                Y = (int) item.Top,
                Width = (int) item.Width,
                Height = (int) item.Height
            });

        var converter = new ImageConverter();
        var bytes = (byte[]) converter.ConvertTo(bitmap, typeof(byte[]))!;

        var boxes = await _computerVision.ReadFileStreamAsync(bytes, "eng");
        item.OriginalText = string.Join(string.Empty, from box in boxes select box.OriginalText);

        // Translate text
        await OnSingleTranslateAsync(item);
    }

    /// <summary>
    /// 移除时触发
    /// </summary>
    /// <param name="rect"></param>
    private void OnRemove(BoundingBox rect)
    {
        RectItems.Remove(rect);
    }

    /// <summary>
    /// <see cref="RemoveGroupCommand"/> 时触发
    /// </summary>
    /// <param name="group"></param>
    private void OnRemoveGroup(int group)
    {
        var itemToRemove = RectItems.Where(r => r.Group == group).ToList();
        foreach (var item in itemToRemove)
        {
            RectItems.Remove(item);
        }
    }

    /// <summary>
    /// <see cref="BatchTranslateCommand"/> 时触发
    /// </summary>
    /// <param name="group"></param>
    private async Task OnBatchTranslateAsync(int group)
    {
        if (RectItems.Any())
        {
            var itemGroup = (from item in RectItems where item.Group == @group select item).ToList();
            var groupInput = from item in itemGroup select item.OriginalText;
            var texts = (await _translator.TranslateGroupAsync(groupInput, "en", "zh")).ToList();

            var index = 0;
            foreach (var item in itemGroup)
            {
                // Check for out of range
                item.TranslatedText = texts.Count > index ? texts[index] : string.Empty;
                index++;
            }
        }
    }

    /// <summary>
    /// <see cref="SingleTranslateCommand"/> 时触发
    /// </summary>
    /// <param name="item"></param>
    private async Task OnSingleTranslateAsync(BoundingBox item)
    {
        item.TranslatedText = await _translator.TranslateAsync(item.OriginalText, "en", "zh");
    }

    /// <summary>
    /// <see cref="RemoveNewLineCommand"/> 时触发
    /// </summary>
    /// <param name="item"></param>
    private void OnRemoveNewLine(BoundingBox item)
    {
        item.OriginalText = item.OriginalText.Replace("\n", " ").Replace("\r", " ");
    }

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
    private BoundingBox? _createdRect;

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
        _createdRect = new BoundingBox {Left = point.X, Top = point.Y};
        _dragInProgress = true;
        RectItems.Add(_createdRect);

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
        if (_dragInProgress && _createdRect != null)
        {
            // 查看鼠标移动了多少
            var point = Mouse.GetPosition((Canvas) sender);
            var offsetX = point.X - _lastPoint.X;
            var offsetY = point.Y - _lastPoint.Y;

            // 更新位置
            _createdRect.Width += offsetX;
            _createdRect.Height += offsetY;

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
        if (_createdRect != null && (_createdRect.Width <= MinSize || _createdRect.Height <= MinSize))
        {
            RectItems.Remove(_createdRect);
        }

        _dragInProgress = false;
        _createdRect = null;

        var el = (UIElement) sender;
        // 释放强制捕获的鼠标
        el.ReleaseMouseCapture();
    }

    #endregion

    #endregion

    #region IDropTarget

    /// <summary>
    /// 无分组块是否可见
    /// </summary>
    public bool NoneGroupVisibility { get; set; }

    /// <summary>
    /// 拖动
    /// 用于验证
    /// </summary>
    /// <param name="dropInfo"></param>
    void IDropTarget.DragOver(IDropInfo dropInfo)
    {
        // Add to existed group                                     
        if (dropInfo.Data is BoundingBox && dropInfo.TargetItem is BoundingBox ||
            // Add to new / none group
            dropInfo.VisualTarget is Grid {Name: "NewGroup" or "NoneGroup"})
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Move;
        }

        if (RectItems.Any() && RectItems.All(r => r.Group != 0))
        {
            NoneGroupVisibility = true;
        }
    }

    /// <summary>
    /// 放下
    /// </summary>
    /// <param name="dropInfo"></param>
    void IDropTarget.Drop(IDropInfo dropInfo)
    {
        var source = (BoundingBox) dropInfo.Data;

        if (dropInfo.VisualTarget is Grid grid)
        {
            // Insert to a new group
            if (grid.Name == "NewGroup")
            {
                var index = (from item in RectItems select item.Group).Max() + 1;
                source.Group = index;
                source.Color = Colors.MakeDarkColorAsString();
            }
            // Insert to none group
            else if (grid.Name == "NoneGroup")
            {
                source.Group = 0;
                source.Color = "#FFDB7093";
            }
        }
        else
        {
            // Insert to a existed group
            var target = (BoundingBox) dropInfo.TargetItem;
            if (target.Group == source.Group)
            {
                // Change Order
                (source.Sort, target.Sort) = (target.Sort, source.Sort);
            }
            else
            {
                // Change Group
                source.Group = target.Group;
                source.Color = target.Color;
            }
        }

        NoneGroupVisibility = false;
    }

    #endregion
}