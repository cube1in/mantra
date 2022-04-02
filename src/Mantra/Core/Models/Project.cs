using System.Collections.Generic;

namespace Mantra.Core.Models;

/// <summary>
/// 项目文件定义
/// </summary>
internal class Project
{
    public string Version { get; set; } = "v1.0.0";

    public ICollection<Graph> Graphs { get; set; } = null!;
}