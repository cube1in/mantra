using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 提供拖动手柄以调整的装饰器
/// </summary>
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class ResizeAdorner : Adorner
{
    #region Private Members

    /// <summary>
    /// VisualCollection
    /// </summary>
    private readonly VisualCollection _visuals;

    /// <summary>
    /// 调整大小
    /// </summary>
    private readonly ResizeChrome _chrome;

    #endregion

    #region Constructor

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <param name="adornedElement">UIElement</param>
    public ResizeAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        SnapsToDevicePixels = true;
        _chrome = new ResizeChrome
        {
            DataContext = adornedElement
        };
        _visuals = new VisualCollection(this) {_chrome};
    }

    #endregion

    #region Override Methods

    protected override Size ArrangeOverride(Size finalSize)
    {
        _chrome.Arrange(new System.Windows.Rect(finalSize));
        return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
        return _visuals[index];
    }

    protected override int VisualChildrenCount => _visuals.Count;

    #endregion
}