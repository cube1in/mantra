using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// The NoFrameHistory attached property for creating a <see cref="Frame"/> that never shows navigation
/// and keeps the navigation history empty
/// </summary>
internal class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
{
    protected override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        // Get the frame
        var frame = (Frame) sender;

        // Hide navigation bar
        frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

        // Clear history on navigate
        frame.Navigated += (ss, ee) => ((Frame) ss).NavigationService.RemoveBackEntry();
    }
}