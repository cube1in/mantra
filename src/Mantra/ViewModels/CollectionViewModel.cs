using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Input;
using Mantra.Core;
using Mantra.Core.Abstractions;
using Microsoft.Win32;
using MvvmHelpers.Commands;
using Ookii.Dialogs.Wpf;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class CollectionViewModel : BaseViewModel
{
    #region Private Members

    /// <summary>
    /// 项目处理程序
    /// </summary>
    private readonly IProjectHandler _projectHandler = new ProjectHandler();

    #endregion

    #region Public Properties

    /// <summary>
    /// 图片集合
    /// </summary>
    public ObservableCollection<string> ImageCollection { get; set; } = new();

    #endregion

    #region Commands

    /// <summary>
    /// 上传图片命令
    /// </summary>
    public ICommand UploadCommand => new Command(OnUpload);

    /// <summary>
    /// 移除图片命令
    /// </summary>
    public ICommand RemoveCommand => new Command<string>(OnRemove);

    /// <summary>
    /// 导出命令
    /// </summary>
    public ICommand ExportCommand => new Command(OnExport);

    /// <summary>
    /// 下一个页面命令
    /// </summary>
    public ICommand GoToCommand =>
        new Command<string>(item => ApplicationViewModel.Current.GoToPage(ApplicationPage.Handle, item));

    #endregion

    #region Private Methods

    /// <summary>
    /// 上传图片时触发
    /// </summary>
    private void OnUpload()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "图片|*.jpg;*.png;*.bmp;*.gif",
            Multiselect = true
        };

        if (dialog.ShowDialog() == true)
        {
            var files = dialog.FileNames;
            foreach (var file in files)
            {
                if (ImageCollection.Contains(file))
                {
                    MessageBox.Show("该图片已经存在", "信息");
                    return;
                }

                ImageCollection.Add(file);
            }
        }
    }

    /// <summary>
    /// <see cref="RemoveCommand"/> 时触发
    /// </summary>
    /// <param name="item"></param>
    private void OnRemove(string item) => ImageCollection.Remove(item);

    /// <summary>
    /// <see cref="ExportCommand"/> 时触发
    /// </summary>
    private void OnExport()
    {
        var dialog = new VistaFolderBrowserDialog();

        if (dialog.ShowDialog() == true)
        {
            var path = dialog.SelectedPath;
            if (_projectHandler.TryGet(Settings.ProjectPath, out var project))
            {
                foreach (var graph in project.Graphs)
                {
                    var source = new Bitmap(graph.Filename);
                    foreach (var window in graph.Windows)
                    {
                        // var border = CreateBorder(window.Text);
                        var textPadding = new TextPadding
                        {
                            Text = window.Text
                        };
                        var bitmap = BitmapHelper.InternalRender(textPadding,
                            new System.Windows.Size(window.Width, window.Height));

                        source.Replace(bitmap, (int) window.Left, (int) window.Top);
                    }

                    var filename = Path.GetFileName(graph.Filename);

                    // Save zip
                    var fullname = Path.Combine(path, Settings.ProjectName) + ".zip";
                    using var zip = ZipFile.Open(fullname, ZipArchiveMode.Create);

                    var entry = zip.CreateEntry(filename, CompressionLevel.Optimal);
                    using var stream = entry.Open();
                    source.Save(stream, ImageFormat.Png);
                    
                    // Open folder
                    OpenFolder(path);
                }
            }
        }
    }

    /// <summary>
    /// 打开目录
    /// </summary>
    /// <param name="folderPath"></param>
    private void OpenFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = folderPath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    public void Initialize(object? pushValue)
    {
        if (pushValue is string[] paths)
        {
            foreach (var path in paths)
            {
                ImageCollection.Add(path);
            }
        }
    }

    #endregion
}