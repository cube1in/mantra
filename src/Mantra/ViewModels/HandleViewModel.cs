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
    private readonly struct InternalProject
    {
        public string Path { get; }

        public string ProjectName { get; }

        public Project Project { get; }

        public InternalProject(string path, string projectName, Project project)
        {
            Path = path;
            ProjectName = projectName;
            Project = project;
        }
    }

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

    /// <summary>
    /// 原图片地址
    /// </summary>
    private string _filename = null!;

    /// <summary>
    /// 项目
    /// </summary>
    private InternalProject _internalProject;

    #endregion

    #region Public Properties

    /// <summary>
    /// 图片
    /// </summary>
    public Bitmap BitmapFile { get; private set; } = null!;

    /// <summary>
    /// 原文字区域
    /// </summary>
    public ObservableCollection<Window> Windows { get; set; } = new();

    /// <summary>
    /// 选中文字区域
    /// </summary>
    public Window? SelectedWindow { get; set; }

    /// <summary>
    /// 显示侧边栏
    /// </summary>
    public bool SideMenuVisible => SelectedWindow != null;

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
        var boxes = await _computerVision.ReadFileLocalAsync(_filename, "eng");
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
        var bitmap = BitmapFile.Crop(new Rectangle
        {
            X = (int) value.Left,
            Y = (int) value.Top,
            Width = (int) value.Width,
            Height = (int) value.Height
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
    private void OnSelectWindow(Window value) => SelectedWindow = value;

    /// <summary>
    /// <see cref="AddTextSettingCommand"/> 时触发
    /// </summary>
    /// <param name="value"></param>
    private void OnAddTextSetting(Window value)
    {
        var bitmap = BitmapFile.Crop(new Rectangle
        {
            X = (int) value.Left,
            Y = (int) value.Top,
            Width = (int) value.Width,
            Height = (int) value.Height
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
    private void OnRemoveTextSetting(Window value) => value.Text.Setting = TextSetting.Default;

    /// <summary>
    /// <see cref="SaveCommand"/> 时触发
    /// </summary>
    private void OnSave()
    {
        var graph = new Graph
        {
            Filename = _filename,
            Windows = Windows
        };

        var project = _internalProject.Project;
        var oldGraph = project.Graphs.FirstOrDefault(g => g.Filename == _filename);
        if (oldGraph != null)
        {
            project.Graphs.Remove(oldGraph);
        }

        project.Graphs.Add(graph);
        _projectHandler.Set(project, _internalProject.Path, _internalProject.ProjectName);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    internal void Initialize(object? pushValue)
    {
#if DEBUG
        pushValue ??= @"C:\Users\sou1m\Desktop\Mantra\test.png";
#endif
        if (pushValue is not string filename)
        {
            throw new ArgumentException("pushValue is not string", $"{pushValue.GetType()}");
        }

        _filename = filename;
        BitmapFile = new Bitmap(filename);

        // Project
        var path = filename.Replace(Path.GetFileName(_filename), string.Empty);
        var project = _projectHandler.Get(path, out var projectName);
        if (project != null)
        {
            // Set Windows
            var graph = project.Graphs.FirstOrDefault(g => g.Filename == filename);
            if (graph != null && graph.Windows?.Count() > 0)
            {
                Windows = new ObservableCollection<Window>(graph.Windows);
                SelectedWindow = Windows.First();
            }
        }
        else
        {
            project = new Project();
            projectName = DateTime.Now.ToString("yyyy-MM-dd");
        }

        _internalProject = new InternalProject(path, projectName, project);
    }

    /// <summary>
    /// 处理下载
    /// </summary>
    /// <param name="bitmaps"></param>
    internal void DownloadHandler(IEnumerable<Bitmap> bitmaps)
    {
        var windows = (from window in Windows where window.Text.Setting != null select window).ToList();

        var index = 0;
        var source = new Bitmap(_filename);
        foreach (var bitmap in bitmaps)
        {
            source.Replace(bitmap, (int) windows[index].Left, (int) windows[index].Top);
            index++;
        }

        var dialog = new SaveFileDialog
        {
            FileName = Path.GetFileName(_filename),
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
    private Window? _createdWindow;

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
        _createdWindow = new Window {Left = (float) point.X, Top = (float) point.Y};
        _dragInProgress = true;
        Windows.Add(_createdWindow);

        _lastPoint = point;

        // https://docs.microsoft.com/zh-cn/previous-versions/ms771301(v=vs.100)?redirectedfrom=MSDN
        // 强制捕获鼠标，否则在产生Rectangle后，由于鼠标后续将会在新产生的Rectangle控件上，
        // 导致无法触发 MouseUp 事件。
        // 当一个对象捕获鼠标时，所有与鼠标相关的事件都被视为具有鼠标捕获的对象执行该事件，即使鼠标指针位于另一个对象之上。
        el.CaptureMouse();

        SelectedWindow = _createdWindow;
    }

    /// <summary>
    /// 鼠标移动时触发
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void MouseMoveHandler(object sender, MouseEventArgs e)
    {
        if (_dragInProgress && _createdWindow != null)
        {
            // 查看鼠标移动了多少
            var point = Mouse.GetPosition((Canvas) sender);
            var offsetX = point.X - _lastPoint.X;
            var offsetY = point.Y - _lastPoint.Y;

            // 更新位置
            _createdWindow.Width += (float) offsetX;
            _createdWindow.Height += (float) offsetY;

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
        if (_createdWindow != null &&
            (_createdWindow.Width <= MinSize ||
             _createdWindow.Height <= MinSize))
        {
            if (SelectedWindow == _createdWindow) SelectedWindow = null;
            Windows.Remove(_createdWindow);
        }

        _dragInProgress = false;
        _createdWindow = null;

        var el = (UIElement) sender;
        // 释放强制捕获的鼠标
        el.ReleaseMouseCapture();
    }

    #endregion

    #endregion
}