using System.Collections.Immutable;
using System.Reflection;
using NuGet.Frameworks;

namespace TopModel.Generator;

public static class AssembliesUtils
{
    private static readonly Dictionary<string, Assembly> loadedAssemblies = AppDomain
        .CurrentDomain.GetAssemblies()
        .ToDictionary(a => a.ManifestModule.Name);

    public static ISet<string> LoadedAssemblies => loadedAssemblies.Keys.ToImmutableHashSet();

    public static IEnumerable<Assembly> LoadAssemblies(IEnumerable<FileInfo> fileInfos)
    {
        var assembliesToLoad = fileInfos
            .Where(file => !file.Name.EndsWith(".resources.dll"))
            .GroupBy(file => file.Name)
            .Select(SelectNearestFrameworkAssembly);

        foreach (var assembly in assembliesToLoad)
        {
            if (LoadedAssemblies.Contains(assembly.Name))
            {
                yield return loadedAssemblies[assembly.Name];
                continue;
            }
            yield return LoadAssemblyFromFileInfo(assembly);
        }
    }

    private static NuGetFramework? GetAssemblyFramework(FileInfo file)
    {
        var directory = file.Directory;
        while (directory != null)
        {
            if (directory.Name.StartsWith("net", StringComparison.OrdinalIgnoreCase))
            {
                var framework = NuGetFramework.ParseFolder(directory.Name);
                if (!framework.IsUnsupported)
                {
                    return framework;
                }
            }

            directory = directory.Parent;
        }

        return null;
    }

    private static Assembly LoadAssemblyFromFileInfo(FileInfo fileInfo)
    {
        var assemblyLoaded = Assembly.LoadFrom(fileInfo.FullName);
        loadedAssemblies.Add(assemblyLoaded.ManifestModule.Name, assemblyLoaded);
        return assemblyLoaded;
    }

    private static FileInfo SelectNearestFrameworkAssembly(IEnumerable<FileInfo> files)
    {
        var filesWithFramework = files
            .Select(file => (File: file, Framework: GetAssemblyFramework(file)))
            .Where(item => item.Framework != null)
            .ToList();
        var nearestFramework = VersionUtils.GetNearestFramework(
            filesWithFramework.Select(item => item.Framework!),
            framework => framework
        );

        return nearestFramework == null
            ? files.First()
            : filesWithFramework.First(item => item.Framework!.Equals(nearestFramework)).File;
    }
}
