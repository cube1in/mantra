using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MvvmHelpers.Commands;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class UploadViewModel : BaseViewModel
{
    /// <summary>
    /// 用于验证的图片文件后缀
    /// </summary>
    private readonly string[] _validExtensions = {".png", ".jpg", ".jpeg"};

    /// <summary>
    /// 上传图片命令
    /// </summary>
    public ICommand UploadCommand => new AsyncCommand(OnUploadAsync);

    /// <summary>
    /// 处理拖拽进入的文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void DropHandler(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;

            if (files.Any(file => !_validExtensions.Contains(Path.GetExtension(file))))
            {
                MessageBox.Show("存在非图片格式的文件", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ApplicationViewModel.Current.GoToPage(ApplicationPage.Collection, files);
        }
    }

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
            if (files.Any(file => !_validExtensions.Contains(Path.GetExtension(file))))
            {
                MessageBox.Show("存在非图片格式的文件", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ApplicationViewModel.Current.GoToPage(ApplicationPage.Collection, files);
        }

        await Task.CompletedTask;
    }
}