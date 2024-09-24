using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TopModel.Utils;

public class TopModelLock : TopModelLockFile
{
    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    private readonly string _lockFileName;
    private readonly ILogger _logger;
    private readonly string _modelRoot;

    private readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .WithIndentedSequences()
        .Build();

    [SetsRequiredMembers]
    public TopModelLock(ILogger logger, string modelRoot, string lockFileName)
    {
        _lockFileName = lockFileName;
        _logger = logger;
        _modelRoot = modelRoot;

        var lockFile = new FileInfo(Path.Combine(modelRoot, lockFileName));

        if (lockFile.Exists)
        {
            try
            {
                using var file = lockFile.OpenText();
                var lf = _deserializer.Deserialize<TopModelLockFile>(file);
                Version = lf.Version;
                GeneratedFiles = lf.GeneratedFiles;
                Modules = lf.Modules;
                Custom = lf.Custom;
            }
            catch
            {
                _logger.LogError($"Erreur à la lecture du fichier {lockFileName}. Merci de rétablir la version générée automatiquement.");
                throw;
            }
        }

        var assembly = Assembly.GetEntryAssembly()!.GetName()!;
        var version = $"{assembly.Version!.Major}.{assembly.Version!.Minor}.{assembly.Version!.Build}";

        if (Version != null && version != Version)
        {
            logger.LogWarning($"Ce modèle a été généré pour la dernière fois avec {assembly.Name} v{Version}, qui n'est pas la version actuellement installée (v{version})");
        }

        Version = version;
    }

    public void UpdateFiles(IEnumerable<string> generatedFiles)
    {
        GeneratedFiles ??= [];

        var generatedFilesList = generatedFiles
            .Select(f => f.ToRelative(_modelRoot))
            .Distinct()
            .OrderBy(f => f)
            .ToList();

        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var filesToPrune = GeneratedFiles
            .Select(f => f.Replace("\\", "/"))
            .Where(f => !generatedFilesList.Select(gf => isWindows ? gf.ToLowerInvariant() : gf).Contains(isWindows ? f.ToLowerInvariant() : f))
            .Select(f => Path.Combine(_modelRoot, f));

        Parallel.ForEach(filesToPrune.Where(File.Exists), fileToPrune =>
        {
            File.Delete(fileToPrune);
            _logger.LogInformation($"Supprimé: {fileToPrune.ToRelative()}");
        });

        GeneratedFiles = generatedFilesList;

        Write();
    }

    public void Write()
    {
        if (Modules.Count > 0 || GeneratedFiles.Count > 0)
        {
            using var fw = new FileWriter(Path.Combine(_modelRoot, _lockFileName), _logger)
            {
                StartCommentToken = "#"
            };

            fw.Write(_serializer.Serialize(this));
        }
    }
}