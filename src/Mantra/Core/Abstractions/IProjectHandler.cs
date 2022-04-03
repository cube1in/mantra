using Mantra.Core.Models;

namespace Mantra.Core.Abstractions;

/// <summary>
/// 项目处理程序
/// </summary>
internal interface IProjectHandler
{
    /// <summary>
    /// 设置项目文件
    /// </summary>
    /// <param name="project"></param>
    /// <param name="path"></param>
    /// <param name="projectName"></param>
    public void Set(Project project, string path, string projectName);

    /// <summary>
    /// 获取项目文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="projectName"></param>
    /// <returns></returns>
    public Project? Get(string path, out string projectName);
}