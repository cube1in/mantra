using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class HandlePage
{
    private HandleViewModel ViewModel => (DataContext as HandleViewModel)!;

    public HandlePage()
    {
        InitializeComponent();
        DataContext = new HandleViewModel();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        ViewModel.Initialize(PushValue);
    }

    private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        ViewModel.MouseDownHandler(sender, e);
    }

    private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
    {
        ViewModel.MouseUpHandler(sender, e);
    }

    private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
    {
        ViewModel.MouseMoveHandler(sender, e);
    }

    private void DownloadButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (FindName("CanvasItemsControl") is not ItemsControl itemsControl) return;

        ViewModel.DownloadHandler(items =>
        {
            var list = items.ToList();
            var index = 0;
            var bitmaps = new List<(float, float, Bitmap)>();
            foreach (var grid in itemsControl.GetVisualDescendents<Grid>())
            {
                if (grid.Name == "ScreenShot" && list[index].Item2)
                {
                    var item = list[index].Item1;
                    var bitmap = BitmapHelper.InternalRender(grid, new System.Windows.Size(item.Width, item.Height));
                    bitmaps.Add((item.Left, item.Top, bitmap));

                    index++;
                }
            }

            return bitmaps;
        });
    }
}