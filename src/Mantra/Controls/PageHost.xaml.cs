using System.Windows;

// ReSharper disable once CheckNamespace
namespace Mantra;

internal partial class PageHost
{
    #region Dependency Properties Definitions

    /// <summary>
    /// The current page to show in the page host
    /// </summary>
    public BasePage CurrentPage
    {
        get => (BasePage) GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    /// <summary>
    /// Registers <see cref="CurrentPage"/> as a dependency property
    /// </summary>
    public static readonly DependencyProperty CurrentPageProperty =
        DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHost),
            new UIPropertyMetadata(null, CurrentPagePropertyChanged, CoerceCurrentPage));

    /// <summary>
    /// Push any data to new page
    /// </summary>
    public object PushValue
    {
        get => GetValue(PushValueProperty);
        set => SetValue(PushValueProperty, value);
    }

    /// <summary>
    /// Registers <see cref="PushValue"/> as a dependency property
    /// </summary>
    public static readonly DependencyProperty PushValueProperty =
        DependencyProperty.Register(nameof(PushValue), typeof(object), typeof(PageHost),
            new UIPropertyMetadata(PushValuePropertyChanged));

    #endregion

    #region Property Changed Events

    /// <summary>
    /// Called when the <see cref="CurrentPage"/> value has changed
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // Get the frames
        var newPageFrame = ((PageHost) d).NewPage;
        var oldPageFrame = ((PageHost) d).OldPage;

        // Store the current page content as the old page
        var oldPageContent = newPageFrame.Content;

        // Remove current page from new page frame
        newPageFrame.Content = null;

        // Move the previous page into the old page frame
        oldPageFrame.Content = oldPageContent;

        // Animate out previous page when the Loaded event fires
        // right after this call due to moving frames
        if (oldPageContent is BasePage oldPage)
            oldPage.ShouldAnimateOut = true;

        // Set the new page content
        newPageFrame.Content = e.NewValue;
    }

    /// <summary>
    /// Set push value to new page
    /// </summary>
    /// <param name="d"></param>
    /// <param name="baseValue"></param>
    /// <returns></returns>
    private static object CoerceCurrentPage(DependencyObject d, object baseValue)
    {
        if (baseValue is BasePage newPage)
        {
            newPage.PushValue = ((PageHost) d).PushValue;
        }

        return baseValue;
    }

    /// <summary>
    /// Called when the <see cref="PushValueProperty"/> value has changed
    /// </summary>
    /// <param name="d"></param>
    /// <param name="e"></param>
    private static void PushValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((PageHost) d).CoerceValue(CurrentPageProperty);
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public PageHost()
    {
        InitializeComponent();
    }

    #endregion
}