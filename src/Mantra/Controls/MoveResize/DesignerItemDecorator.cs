using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Mantra.Extensions;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// DesignerItem 装饰器
/// </summary>
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class DesignerItemDecorator : ContentControl
{
    #region Private Members

    /// <summary>
    /// 调整大小装饰器
    /// </summary>
    private ResizeAdorner? _resizeAdorner;

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    private SizeAdorner? _sizeAdorner;

    #endregion

    #region Dependency Properties Definitions

    /// <summary>
    /// 显示调整大小装饰器
    /// </summary>
    public static readonly DependencyProperty ResizeProperty = DependencyProperty.Register(nameof(Resize),
        typeof(bool), typeof(DesignerItemDecorator), new FrameworkPropertyMetadata(false, ResizePropertyChanged));

    /// <summary>
    /// 显示调整大小装饰器
    /// </summary>
    public bool Resize
    {
        get => (bool) GetValue(ResizeProperty);
        set => SetValue(ResizeProperty, value);
    }

    /// <summary>
    /// ResizePropertyChanged Callback
    /// </summary>
    /// <param name="d">DependencyObject</param>
    /// <param name="e">DependencyPropertyChangedEventArgs</param>
    private static void ResizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var decorator = (DesignerItemDecorator) d;
        var showDecorator = (bool) e.NewValue;

        if (showDecorator) decorator.ShowResizeAdorner();
        else decorator.HideResizeAdorner();
    }

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size),
        typeof(bool), typeof(DesignerItemDecorator), new FrameworkPropertyMetadata(false, SizePropertyChanged));

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    public bool Size
    {
        get => (bool) GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// SizePropertyChanged Callback
    /// </summary>
    /// <param name="d">DependencyObject</param>
    /// <param name="e">DependencyPropertyChangedEventArgs</param>
    private static void SizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var decorator = (DesignerItemDecorator) d;
        var showDecorator = (bool) e.NewValue;

        if (showDecorator) decorator.ShowSizeAdorner();
        else decorator.HideSizeAdorner();
    }

    #endregion

    #region Construction

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public DesignerItemDecorator()
    {
        Unloaded += HandleUnloaded;
    }

    /// <summary>
    /// 处理卸载
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">RoutedEventArgs</param>
    private void HandleUnloaded(object sender, RoutedEventArgs e)
    {
        if (_resizeAdorner != null)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer?.Remove(_resizeAdorner);

            _resizeAdorner = null;
        }

        if (_sizeAdorner != null)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            adornerLayer?.Remove(_sizeAdorner);

            _sizeAdorner = null;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// 隐藏装饰器
    /// </summary>
    private void HideResizeAdorner()
    {
        if (_resizeAdorner != null)
        {
            _resizeAdorner.Visibility = Visibility.Hidden;
        }
    }

    /// <summary>
    /// 显示调整大小装饰器
    /// </summary>
    private void ShowResizeAdorner()
    {
        if (_resizeAdorner == null)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                if (this.HasParent<Canvas>())
                {
                    _resizeAdorner = new ResizeAdorner(this);
                    adornerLayer.Add(_resizeAdorner);
                }
            }
        }

        if (_resizeAdorner != null) _resizeAdorner.Visibility = Visibility.Visible;
    }

    /// <summary>
    /// 隐藏大小装饰器
    /// </summary>
    private void HideSizeAdorner()
    {
        if (_sizeAdorner != null)
        {
            _sizeAdorner.Visibility = Visibility.Hidden;
        }
    }

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    private void ShowSizeAdorner()
    {
        if (_sizeAdorner == null)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                if (this.HasParent<Canvas>())
                {
                    _sizeAdorner = new SizeAdorner(this);
                    adornerLayer.Add(_sizeAdorner);
                }
            }
        }

        if (_sizeAdorner != null) _sizeAdorner.Visibility = Visibility.Visible;
    }

    #endregion
}