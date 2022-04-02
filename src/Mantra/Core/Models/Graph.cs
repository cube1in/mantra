using System.Collections.Generic;

namespace Mantra.Core.Models;

/// <summary>
/// 图
/// </summary>
internal class Graph
{
    /// <summary>
    /// 文字名
    /// </summary>
    public string Filename { get; set; } = null!;

    /// <summary>
    /// 窗口
    /// </summary>
    public IEnumerable<Window>? Windows { get; set; }
}