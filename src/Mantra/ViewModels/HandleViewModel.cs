using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmHelpers.Commands;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Mantra.Core;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using Mantra.Plugins;
using Microsoft.Win32;
using Point = System.Windows.Point;
using Window = Mantra.Core.Models.Window;

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

    /// <summary>
    /// 项目处理程序
    /// </summary>
    private readonly IProjectHandler _projectHandler = new ProjectHandler();

    #endregion

    #region Public Properties

    /// <summary>
    /// 原图片地址
    /// </summary>
    public string? Filename { get; set; }

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
    public ObservableCollection<Window> Windows { get; set; } = new();

    /// <summary>
    /// 选中文字区域
    /// </summary>
    public Window? SelectedWindow { get; set; }

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
    public ICommand SingleOCRCommand => new AsyncCommand<Window>(OnSingleOCRAsync);

    /// <summary>
    /// 移除框命令
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ICommand RemoveWindowCommand => new Command<Window>(OnRemoveWindow);

    /// <summary>
    /// 单个翻译命令
    /// </summary>
    public ICommand SingleTranslateCommand => new AsyncCommand<Window>(OnSingleTranslateAsync);

    /// <summary>
    /// 消除换行
    /// </summary>
    public ICommand RemoveNewLineCommand => new Command<Window>(OnRemoveNewLine);

    /// <summary>
    /// 选中命令
    /// </summary>
    public ICommand SelectWindowCommand => new Command<Window>(OnSelectWindow);

    /// <summary>
    /// 添加文字设置命令
    /// </summary>
    public ICommand AddTextSettingCommand => new Command<Window>(OnAddTextSetting);

    /// <summary>
    /// 移除文字设置命令
    /// </summary>
    public ICommand RemoveTextSettingCommand => new Command<Window>(OnRemoveTextSetting);

    /// <summary>
    /// 保存命令
    /// </summary>
    public ICommand SaveCommand => new Command(OnSave);

    #endregion

    #region Private Methods

    /// <summary>
    /// <see cref="BatchOCRCommand"/> 时触发
    /// </summary>
    private async Task OnBatchOCRAsync()
    {
        if (Filename == null)
        {
            MessageBox.Show("图片不存在", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var boxes = await _computerVision.ReadFileLocalAsync(Filename, "eng");
        Windows = new ObservableCollection<Window>(from box in boxes
            select new Window
            {
                Left = box.Left, Top = box.Top, Width = box.Width, Height = box.Height,
                Text = new Text {OriginalText = box.Text}
            });
    }

    /// <summary>
    /// <see cref="SingleOCRCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private async Task OnSingleOCRAsync(Window value)
    {
        var bitmap = CropImage(Filename!,
            new RectangleF
            {
                X = (float) value.Left,
                Y = (float) value.Top,
                Width = (float) value.Width,
                Height = (float) value.Height
            });

        var converter = new ImageConverter();
        var bytes = (byte[]) converter.ConvertTo(bitmap, typeof(byte[]))!;

        var boxes = await _computerVision.ReadFileStreamAsync(bytes, "eng");
        value.Text.OriginalText = string.Join(string.Empty, from box in boxes select box.Text);

        // Set selected
        SelectedWindow = value;

        // Translate text
        await OnSingleTranslateAsync(value);
    }

    /// <summary>
    /// <see cref="RemoveWindowCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveWindow(Window value)
    {
        Windows.Remove(value);
        if (value == SelectedWindow) SelectedWindow = null;
    }

    /// <summary>
    /// <see cref="SingleTranslateCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private async Task OnSingleTranslateAsync(Window value)
    {
        value.Text.TranslatedText = await _translator.TranslateAsync(value.Text.OriginalText, "en", "zh");
    }

    /// <summary>
    /// <see cref="RemoveNewLineCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveNewLine(Window value)
        => value.Text.OriginalText = value.Text.OriginalText.Replace("\n", " ").Replace("\r", " ");

    /// <summary>
    /// <see cref="SelectWindowCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnSelectWindow(Window value)
        => SelectedWindow = value;

    /// <summary>
    /// <see cref="AddTextSettingCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnAddTextSetting(Window value)
    {
        var bitmap = CropImage(Filename!,
            new RectangleF
            {
                X = (float) value.Left,
                Y = (float) value.Top,
                Width = (float) value.Width,
                Height = (float) value.Height
            });

        var color = bitmap.GetPixel(0, 0);
        value.Text.Setting = new TextSetting
        {
            Background = Colors.AsString(color)
        };
    }

    /// <summary>
    /// <see cref="RemoveTextSettingCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnRemoveTextSetting(Window value)
        => value.Text.Setting = null;

    /// <summary>
    /// <see cref="SaveCommand"/> 时触发
    /// </summary>
    private void OnSave()
    {
        if (Filename == null) return;

        var graph = new Graph
        {
            Filename = Filename,
            Windows = Windows
        };

        var path = Filename.Replace(Path.GetFileName(Filename), string.Empty);
        var project = _projectHandler.Get(path, out var name);
        if (project != null)
        {
            var oldGraph = project.Graphs.FirstOrDefault(g => g.Filename == Filename);
            if (oldGraph != null)
            {
                project.Graphs.Remove(oldGraph);
            }

            project.Graphs.Add(graph);
            _projectHandler.Set(project, path, name);
        }
        else
        {
            _projectHandler.Set(new Project
            {
                Graphs = new List<Graph> {graph}
            }, path, DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }

    /// <summary>
    /// Set image original size
    /// </summary>
    private void SetOriginalSize()
    {
        if (Filename == null) return;

        // Get image original size
        var bitmap = new Bitmap(Filename);
        ImgPixelHeight = bitmap.Height;
        ImgPixelWidth = bitmap.Width;
    }

    /// <summary>
    /// 截图
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cropArea"></param>
    /// <returns></returns>
    private static Bitmap CropImage(string path, RectangleF cropArea)
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
            Filename = path;
            SetOriginalSize();
        }
    }

    /// <summary>
    /// 处理下载
    /// </summary>
    /// <param name="bitmaps"></param>
    internal void DownloadHandler(IEnumerable<Bitmap> bitmaps)
    {
        var windows = (from window in Windows where window.Text.Setting != null select window).ToList();

        var index = 0;
        var source = new Bitmap(Filename!);
        foreach (var bitmap in bitmaps)
        {
            source.Replace(bitmap, (int) windows[index].Left, (int) windows[index].Top);
            index++;
        }

        var dialog = new SaveFileDialog
        {
            FileName = Path.GetFileName(Filename),
            Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp"
        };

        if (dialog.ShowDialog() == true)
        {
            // Or JpegBitmapDecoder, or whichever encoder you want
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(BitmapHelper.ConvertBitmap(source)));
            using var stream = dialog.OpenFile();
            encoder.Save(stream);
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
    private Window? _createdBoundingBox;

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
        _createdBoundingBox = new Window {Left = (float) point.X, Top = (float) point.Y};
        _dragInProgress = true;
        Windows.Add(_createdBoundingBox);

        _lastPoint = point;

        // https://docs.microsoft.com/zh-cn/previous-versions/ms771301(v=vs.100)?redirectedfrom=MSDN
        // 强制捕获鼠标，否则在产生Rectangle后，由于鼠标后续将会在新产生的Rectangle控件上，
        // 导致无法触发 MouseUp 事件。
        // 当一个对象捕获鼠标时，所有与鼠标相关的事件都被视为具有鼠标捕获的对象执行该事件，即使鼠标指针位于另一个对象之上。
        el.CaptureMouse();

        SelectedWindow = _createdBoundingBox;
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
            _createdBoundingBox.Width += (float) offsetX;
            _createdBoundingBox.Height += (float) offsetY;

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
            if (SelectedWindow == _createdBoundingBox) SelectedWindow = null;
            Windows.Remove(_createdBoundingBox);
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