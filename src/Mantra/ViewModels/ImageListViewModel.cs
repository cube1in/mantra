using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using MvvmHelpers.Commands;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class ImageListViewModel : BaseViewModel
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

    #endregion

    #region Private Methods

    /// <summary>
    /// 上传图片时触发
    /// </summary>
    private async Task OnUploadAsync()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "图片|*.jpg;*.png;*.bmp;*.gif"
        };
        if (dialog.ShowDialog() == true)
        {
            ImageList.Add(dialog.FileName);
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 移除图片时触发
    /// </summary>
    /// <param name="item"></param>
    private async Task OnRemoveAsync(string item)
    {
        await Task.Run(() => ImageList.Remove(item));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pushValue"></param>
    public void Initialized(object? pushValue)
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