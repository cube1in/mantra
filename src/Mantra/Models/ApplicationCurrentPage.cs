// ReSharper disable once CheckNamespace
namespace Mantra;

/// <summary>
/// 当前页
/// </summary>
internal struct ApplicationCurrentPage
{
    /// <summary>
    /// 页
    /// </summary>
    public ApplicationPage ApplicationPage { get; }

    /// <summary>
    /// 是否使用缓存
    /// </summary>
    public bool UseCache { get; }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    /// <param name="applicationPage">页</param>
    /// <param name="useCache">是否使用缓存</param>
    public ApplicationCurrentPage(ApplicationPage applicationPage, bool useCache)
    {
        ApplicationPage = applicationPage;
        UseCache = useCache;
    }
}