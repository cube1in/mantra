using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MvvmHelpers.Commands;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class CollectionViewModel : BaseViewModel
{
    #region Public Properties

    /// <summary>
    /// 图片集合
    /// </summary>
    public ObservableCollection<string> ImageList { get; set; } = new();

    #endregion

    #region Commands

    /// <summary>
    /// 上传图片命令
    /// </summary>
    public ICommand UploadCommand => new AsyncCommand(OnUploadAsync);

    /// <summary>
    /// 移除图片命令
    /// </summary>
    public ICommand RemoveCommand => new AsyncCommand<string>(OnRemoveAsync);

    /// <summary>
    /// 下一个页面命令
    /// </summary>
    public ICommand GoToCommand => new AsyncCommand<string>(OnGoToAsync);

    #endregion

    #region Private Methods

    /// <summary>
    /// 上传图片时触发
    /// </summary>
    private async Task OnUploadAsync()
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
                if (ImageList.Contains(file))
                {
                    MessageBox.Show("该图片已经存在", "信息");
                    return;
                }

                ImageList.Add(file);
            }
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 移除图片时触发
    /// </summary>
    /// <param name="item"></param>
    private async Task OnRemoveAsync(string item)
    {
        ImageList.Remove(item);
        await Task.CompletedTask;
    }

    /// <summary>
    /// GoTo时触发
    /// </summary>
    /// <param name="item"></param>
    private async Task OnGoToAsync(string item)
    {
        ApplicationViewModel.Current.GoToPage(ApplicationPage.Handle, item);
        await Task.CompletedTask;
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
                ImageList.Add(path);
            }
        }
    }

    #endregion
}