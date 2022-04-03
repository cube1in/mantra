// ReSharper disable once CheckNamespace

namespace Mantra;

/// <summary>
/// 应用视图模型
/// </summary>
internal class Application : BaseViewModel
{
    #region Singleton

    /// <summary>
    /// 创建锁
    /// </summary>
    private static readonly object CreateLock = new();

    /// <summary>
    /// 单例实例
    /// </summary>
    private static Application? _instance;

    /// <summary>
    /// 当前单例
    /// </summary>
    public static Application Current
    {
        get
        {
            // For Performance
            if (_instance == null)
            {
                lock (CreateLock)
                {
                    _instance ??= new Application();
                }
            }

            return _instance;
        }
    }

    #endregion

    /// <summary>
    /// The current page of the application
    /// </summary>
    public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.Handle;

    /// <summary>
    /// Push value to go to page
    /// </summary>
    public object? PushValue { get; private set; }

    /// <summary>
    /// Navigates to the specified page
    /// </summary>
    /// <param name="page">The page to go to</param>
    /// <param name="pushValue">The value push to go to page</param>
    public void GoToPage(ApplicationPage page, object? pushValue = null)
    {
        // Set the current page
        CurrentPage = page;

        // Set push value
        PushValue = pushValue;
    }
}