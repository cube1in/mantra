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
internal class ResizableContainer : RadioButton
{
    #region Private Members

    /// <summary>
    /// 调整大小装饰器
    /// </summary>
    private ResizeAdorner? _resizeAdorner;

    #endregion

    #region Dependency Properties Definitions

    /// <summary>
    /// 外边框颜色
    /// </summary>
    public static readonly DependencyProperty StrokeColorProperty =
        DependencyProperty.Register(nameof(StrokeColor), typeof(string), typeof(ResizableContainer),
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
    public ResizableContainer()
    {
        Unloaded += HandleUnloaded;
    }

    /// <summary>
    /// Override OnApplyTemplate
    /// </summary>
    public override void OnApplyTemplate()
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
    }

    #endregion
}