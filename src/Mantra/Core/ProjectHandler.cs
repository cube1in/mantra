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

    public void Set(Project project, string path, string name)
    {
        var jsonString = JsonConvert.SerializeObject(project, JsonSettings.SerializerSettings);
        var filename = Path.Combine(path, name + FileExtension);
        File.WriteAllText(filename, jsonString);
    }

    public Project? Get(string path, out string name)
    {
        name = string.Empty;
        if (!Directory.Exists(path)) return null;

        foreach (var file in Directory.GetFiles(path))
        {
            if (Path.GetExtension(file) == FileExtension)
            {
                name = Path.GetFileName(file);
                var jsonString = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<Project>(jsonString, JsonSettings.SerializerSettings);
            }
        }

        return null;
    }
}