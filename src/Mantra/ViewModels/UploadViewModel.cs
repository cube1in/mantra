using System.IO;
using System.Linq;
using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class UploadViewModel
{
    private readonly string[] _validExtensions = {".png", ".jpg", ".jpeg"};

    public void HandleDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            if (e.Data.GetData(DataFormats.FileDrop) is not string[] files) return;

            if (files.Any(file => !_validExtensions.Contains(Path.GetExtension(file))))
            {
                // TODO: alert warning message
                return;
            }

            ApplicationViewModel.Current.GoToPage(ApplicationPage.ImageList, files);
        }
    }
}