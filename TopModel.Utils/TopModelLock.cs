using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

public class TopModelLock
{
#nullable disable
    public string Version { get; set; }
#nullable enable

    public List<string> GeneratedFiles { get; set; } = new();

    public void Init(ILogger logger)
    {
        var assembly = Assembly.GetEntryAssembly()!.GetName()!;
        var version = $"{assembly.Version!.Major}.{assembly.Version!.Minor}.{assembly.Version!.Build}";

        if (Version != null && version != Version)
        {
            logger.LogWarning($"Ce modèle a été généré pour la dernière fois avec {assembly.Name} v{Version}, qui n'est pas la version actuellement installée (v{version})");
        }

        Version = version;
    }

    public void Update(string modelRoot, string lockFileName, ILogger logger, IEnumerable<string> generatedFiles)
    {
        GeneratedFiles ??= new();

        var generatedFilesList = generatedFiles
            .Select(f => f.ToRelative(modelRoot))
            .Distinct()
            .OrderBy(f => f)
            .ToList();

        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var filesToPrune = GeneratedFiles
            .Select(f => f.Replace("\\", "/"))
            .Where(f => !generatedFilesList.Select(gf => isWindows ? gf.ToLowerInvariant() : gf).Contains(isWindows ? f.ToLowerInvariant() : f))
            .Select(f => Path.Combine(modelRoot, f));

        foreach (var fileToPrune in filesToPrune.Where(File.Exists))
        {
            File.Delete(fileToPrune);
            logger.LogInformation($"Supprimé: {fileToPrune.ToRelative()}");
        }

        GeneratedFiles = generatedFilesList;

        using var fw = new FileWriter(Path.Combine(modelRoot, lockFileName), logger)
        {
            StartCommentToken = "#"
        };

        fw.WriteLine($"version: {Version}");
        if (GeneratedFiles.Count > 0)
        {
            fw.WriteLine("generatedFiles:");
            foreach (var genFile in GeneratedFiles)
            {
                fw.WriteLine($"  - {genFile}");
            }
        }
    }
}