using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal class AnimateBaseProperty<TParent> : BaseAttachedProperty<TParent, bool>
    where TParent : BaseAttachedProperty<TParent, bool>, new()
{
    #region Protected Properties

    protected Dictionary<WeakReference, bool> AlreadyLoaded = new();

    protected Dictionary<WeakReference, bool> FirstLoadValue = new();

    #endregion

    protected override void OnValueUpdated(DependencyObject sender, object value)
    {
        // Get the framework element
        if (sender is not FrameworkElement element) return;

        // Try and get the already load reference
        var alreadyLoadedReference = AlreadyLoaded.FirstOrDefault(f => Equals(f.Key.Target, sender));

        // Try and get the first load reference
        var firstLoadReference = FirstLoadValue.FirstOrDefault(f => Equals(f.Key.Target, sender));

        // Do not fire if the value doesn't change
        if ((bool) sender.GetValue(ValueProperty) == (bool) value && alreadyLoadedReference.Key != null) return;

        // On first load...
        if (alreadyLoadedReference.Key == null)
        {
            // Create weak reference
            var weakReference = new WeakReference(sender);

            // Flag that we are in first load but not finished it
            AlreadyLoaded[weakReference] = false;

            // Start off hidden before we decide how to animate
            element.Visibility = Visibility.Hidden;

            // Hook into the loaded event of the element
            element.Loaded += OnLoaded;

            // Create a single self-unhookable event
            async void OnLoaded(object ss, RoutedEventArgs ee)
            {
                // Unhook ourselves
                element.Loaded -= OnLoaded;

                // Slight delay after load is needed for some element to get laid out
                // and their width/heights correctly calculated
                await Task.Delay(5);

                // Refresh the first load value in case it changed
                // since the 5ms delay
                firstLoadReference = FirstLoadValue.FirstOrDefault(f => Equals(f.Key.Target, sender));

                // Do desired animation
                DoAnimation(element, firstLoadReference.Key != null ? firstLoadReference.Value : (bool) value, true);

                // Flag that we have finished first load
                AlreadyLoaded[weakReference] = true;
            }
        }
        // If we have started a first load but not fired the animation yet, update the property
        else if (alreadyLoadedReference.Value == false)
            FirstLoadValue[new WeakReference(sender)] = (bool) value;
        else
            // Do desired animation
            DoAnimation(element, (bool) value, false);
    }

    /// <summary>
    /// The animation method is fired when the value changes
    /// </summary>
    /// <param name="element">The element</param>
    /// <param name="value">The new value</param>
    /// <param name="firstLoad">Indicates if this is the first load</param>
    protected virtual void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
    }
}

internal class FadeInImageOnLoadProperty : AnimateBaseProperty<FadeInImageOnLoadProperty>
{
    protected override void OnValueUpdated(DependencyObject sender, object value)
    {
        // Make sure we have an image
        if (sender is not Image image) return;

        // If we want to animate in...
        if ((bool) value)
            // Listen for target change
            image.TargetUpdated += Image_TargetUpdatedAsync;
        // Otherwise
        else
            // Make sure we unhooked
            image.TargetUpdated -= Image_TargetUpdatedAsync;
    }

    private async void Image_TargetUpdatedAsync(object? sender, DataTransferEventArgs e)
    {
        // Fade in image
        await (sender as Image)!.FadeInAsync(false);
    }
}

internal class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Left, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Left, firstLoad ? 0 : 0.3f, false);
    }
}

internal class AnimateSlideInFromRightProperty : AnimateBaseProperty<AnimateSlideInFromRightProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Right, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Right, firstLoad ? 0 : 0.3f, false);
    }
}

internal class AnimateSlideInFromBottomProperty : AnimateBaseProperty<AnimateSlideInFromBottomProperty>
{
    protected override async void DoAnimation(FrameworkElement element, bool value, bool firstLoad)
    {
        if (value)
            // Animate in
            await element.SlideAndFadeInAsync(AnimationSlideInDirection.Bottom, firstLoad, firstLoad ? 0 : 0.3f, false);
        else
            // Animate out
            await element.SlideAndFadeOutAsync(AnimationSlideInDirection.Bottom, firstLoad ? 0 : 0.3f, false);
    }
}