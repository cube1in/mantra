using System.Collections.Generic;
using System.Threading.Tasks;
using Mantra.Core.Models;

namespace Mantra.Core.Abstractions;

/// <summary>
/// 计算机视觉
/// </summary>
public interface IComputerVision
{
    /// <summary>
    /// 读取本地文件
    /// </summary>
    /// <param name="localFile"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    Task<IEnumerable<BoundingBox>> ReadFileLocalAsync(string localFile, string language);

    /// <summary>
    /// 读取网络文件
    /// </summary>
    /// <param name="urlFile"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    Task<IEnumerable<BoundingBox>> ReadFileUrlAsync(string urlFile, string language);

    /// <summary>
    /// 读取流文件
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    Task<IEnumerable<BoundingBox>> ReadFileStreamAsync(byte[] bytes, string language);
}