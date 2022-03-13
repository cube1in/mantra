using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 显示大小的装饰器
/// </summary>
[SuppressMessage("ReSharper", "IdentifierTypo")]
internal class SizeAdorner : Adorner
{
    #region Private Members

    /// <summary>
    /// VisualCollection
    /// </summary>
    private readonly VisualCollection _visuals;

    /// <summary>
    /// 显示大小
    /// </summary>
    private readonly SizeChrome _chrome;

    #endregion

    #region Constructor

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <param name="adornedElement">UIElement</param>
    public SizeAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        SnapsToDevicePixels = true;
        _chrome = new SizeChrome {DataContext = adornedElement};
        _visuals = new VisualCollection(this) {_chrome};
    }

    #endregion

    #region Override Methods

    protected override Size ArrangeOverride(Size finalSize)
    {
        _chrome.Arrange(new System.Windows.Rect(new Point(0.0, 0.0), finalSize));
        return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
        return _visuals[index];
    }

    protected override int VisualChildrenCount => _visuals.Count;

    #endregion
}