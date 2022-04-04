using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
        var jsonString = JsonConvert.SerializeObject(project, Settings.SerializerSettings);
        var filename = Path.Combine(path, projectName + FileExtension);

        var oldFile = InternalGet(path);
        if (oldFile != null) File.Delete(oldFile);

        File.WriteAllText(filename, jsonString);
    }

    public Project Get(string path)
    {
        if (TryGet(path, out var project))
        {
            return project;
        }

        throw new NullReferenceException();
    }

    public bool TryGet(string path, [NotNullWhen(true)] out Project? project)
    {
        project = null;
        if (!Directory.Exists(path)) return false;

        var file = InternalGet(path);
        if (file == null) return false;

        var jsonString = File.ReadAllText(file);
        project = JsonConvert.DeserializeObject<Project>(jsonString, Settings.SerializerSettings);

        return project != null;
    }

    private string? InternalGet(string path)
    {
        return Directory.GetFiles(path).FirstOrDefault(file => Path.GetExtension(file) == FileExtension);
    }
}