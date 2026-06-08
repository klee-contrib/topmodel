using System.Collections.Immutable;
using System.Reflection;

namespace TopModel.Generator;

public static class AssembliesUtils
{
    private static readonly object _lock = new();

    private static readonly Dictionary<string, Assembly> loadedAssemblies = AppDomain
        .CurrentDomain.GetAssemblies()
        .ToDictionary(a => a.ManifestModule.Name);
    public static ISet<string> LoadedAssemblies => loadedAssemblies.Keys.ToImmutableHashSet();

    public static IEnumerable<Assembly> LoadAssemblies(IEnumerable<FileInfo> fileInfos)
    {
        var assembliesToLoad = fileInfos.Where(f => !f.Name.EndsWith(".resources.dll")).DistinctBy(a => a.Name);

        lock (_lock)
        {
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
    }

    private static Assembly LoadAssemblyFromFileInfo(FileInfo fileInfo)
    {
        var assemblyLoaded = Assembly.LoadFrom(fileInfo.FullName);
        loadedAssemblies.Add(assemblyLoaded.ManifestModule.Name, assemblyLoaded);
        return assemblyLoaded;
    }
}
