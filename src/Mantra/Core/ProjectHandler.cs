using System.IO;
using Mantra.Core.Abstractions;
using Mantra.Core.Models;
using Newtonsoft.Json;

namespace Mantra.Core;

/// <summary>
/// 项目处理程序
/// </summary>
internal class ProjectHandler : IProjectHandler
{
    private const string FileExtension = ".mtproj";

    public void Set(Project project, string path, string projectName)
    {
        var jsonString = JsonConvert.SerializeObject(project, JsonSettings.SerializerSettings);
        var filename = Path.Combine(path, projectName + FileExtension);
        File.WriteAllText(filename, jsonString);
    }

    public Project? Get(string path, out string projectName)
    {
        projectName = string.Empty;
        if (!Directory.Exists(path)) return null;

        foreach (var file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) == FileExtension)
            {
                projectName = Path.GetFileName(file).Replace(FileExtension, string.Empty);
                var jsonString = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<Project>(jsonString, JsonSettings.SerializerSettings);
            }
        }

        return null;
    }
}