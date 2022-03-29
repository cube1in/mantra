using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 提供拖动手柄以调整
/// </summary>
internal class ResizeChrome : Control
{
    static ResizeChrome()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeChrome), new FrameworkPropertyMetadata(typeof(ResizeChrome)));
    }
}