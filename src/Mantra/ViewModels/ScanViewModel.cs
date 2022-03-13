using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Http;
using System.Windows;
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
    public ObservableCollection<Rect> SourceRectItems { get; set; } = new();

    #endregion

    #region Commands

    /// <summary>
    /// 光学识别命令
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public ICommand OCRCommand => new Command(OnOCR);

    /// <summary>
    /// 移除框命令
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ICommand RemoveCommand => new Command<Rect>(OnRemove);

    #endregion

    #region Private Methods

    /// <summary>
    /// 光学扫描时触发
    /// </summary>
    private void OnOCR()
    {
        if (ImgSource == null)
        {
            MessageBox.Show("图片不存在", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var blocks = Tesseact.GetBlocks(ImgSource);
        foreach (var block in blocks)
        {
            if (block.BoundingBox != null)
            {
                var boundingBox = block.BoundingBox.Value;
                SourceRectItems.Add(new Rect
                {
                    Left = boundingBox.X1, Top = boundingBox.Y1, Height = boundingBox.Height, Width = boundingBox.Width
                });
            }
        }
    }

    /// <summary>
    /// 移除时触发
    /// </summary>
    /// <param name="rect"></param>
    private void OnRemove(Rect rect)
    {
        SourceRectItems.Remove(rect);
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

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    public void Initialize(object? pushValue)
    {
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
    private Rect? _createdRect;

    /// <summary>
    /// 鼠标拖动过程中最后记录的位置
    /// </summary>
    private Point _lastPoint;

    /// <summary>
    /// 鼠标左键按下时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MouseDownHandler(object sender, MouseButtonEventArgs e)
    {
        var el = (UIElement) sender;
        var point = Mouse.GetPosition(el);
        _createdRect = new Rect {Left = point.X, Top = point.Y};
        _dragInProgress = true;
        SourceRectItems.Add(_createdRect);
        
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
            SourceRectItems.Remove(_createdRect);
            SourceRectItems.Add(_createdRect);
            
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
        _dragInProgress = false;
        _createdRect = null;

        var el = (UIElement) sender;
        
        // 释放强制捕获的鼠标
        el.ReleaseMouseCapture();
    }

    #endregion

    #endregion
}