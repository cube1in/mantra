using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// Designer容器
/// </summary>
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class DesignerContainer : ContentControl
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
    public static readonly DependencyProperty ResizeProperty =
        DependencyProperty.Register(nameof(Resize), typeof(bool), typeof(DesignerContainer),
            new FrameworkPropertyMetadata(false, ResizePropertyChanged));

    /// <summary>
    /// 显示调整大小装饰器
    /// </summary>
    public bool Resize
    {
        get => (bool) GetValue(ResizeProperty);
        set => SetValue(ResizeProperty, value);
    }

    /// <summary>
    /// 当 <see cref="ResizeProperty"/> 改变后触发
    /// </summary>
    /// <param name="d">DependencyObject</param>
    /// <param name="e">DependencyPropertyChangedEventArgs</param>
    private static void ResizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var decorator = (DesignerContainer) d;
        var showDecorator = (bool) e.NewValue;

        if (showDecorator) decorator.ShowResizeAdorner();
        else decorator.HideResizeAdorner();
    }

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size),
        typeof(bool), typeof(DesignerContainer), new FrameworkPropertyMetadata(false, SizePropertyChanged));

    /// <summary>
    /// 显示大小装饰器
    /// </summary>
    public bool Size
    {
        get => (bool) GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 当 <see cref="SizeProperty"/> 改变后触发
    /// </summary>
    /// <param name="d">DependencyObject</param>
    /// <param name="e">DependencyPropertyChangedEventArgs</param>
    private static void SizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var decorator = (DesignerContainer) d;
        var showDecorator = (bool) e.NewValue;

        if (showDecorator) decorator.ShowSizeAdorner();
        else decorator.HideSizeAdorner();
    }

    /// <summary>
    /// 外边框颜色
    /// </summary>
    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(string), typeof(DesignerContainer),
            new PropertyMetadata("#FFDB7093", null, CoerceStrokeColor));

    /// <summary>
    /// 外边框颜色
    /// </summary>
    public string StrokeColor
    {
        get => (string) GetValue(StrokeColorProperty);
        set => SetValue(StrokeColorProperty, value);
    }

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="d"></param>
    /// <param name="baseValue"></param>
    /// <returns></returns>
    private static object CoerceStrokeColor(DependencyObject d, object baseValue)
    {
        // 验证颜色值是否正常
        ColorConverter.ConvertFromString((string) baseValue);
        return baseValue;
    }

    #endregion

    #region Construction

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public DesignerContainer()
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
                if (this.GetVisualAncestor<Canvas>() != null)
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
                if (this.GetVisualAncestor<Canvas>() != null)
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