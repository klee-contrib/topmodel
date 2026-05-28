using System.Text;
using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class GeneratorBase<T>(ILogger logger, IFileWriterProvider writerProvider) : IModelWatcher
    where T : GeneratorConfigBase
{
    public abstract string Name { get; }

#nullable disable
    public T Config { get; internal set; }

#nullable enable

    public int Number { get; internal set; }

    public virtual IEnumerable<string> GeneratedFiles => [];

    public bool Disabled => Config.Disable?.Contains(Name) ?? false;

    public CancellationToken? CancellationToken { get; set; }

    /// <inheritdoc cref="IModelWatcher.OnErrors" />
    public void OnErrors(IDictionary<ModelFile, IEnumerable<ModelError>> errors) { }

    /// <inheritdoc cref="IModelWatcher.OnFilesChanged" />
    public void OnFilesChanged(IEnumerable<ModelFile> files, LoggingScope? storeConfig = null)
    {
        using var scope = logger.BeginScope(((IModelWatcher)this).FullName);
        using var scope2 = logger.BeginScope(storeConfig!);
        HandleFiles(files.Where(f => Config.Files.ContainsKey(f.Name)));
    }

    /// <inheritdoc cref="IModelWatcher.OnFilesDeleted" />
    public void OnFilesDeleted(IEnumerable<string> fileNames) { }

    public IFileWriter OpenFileWriter(string fileName, bool encoderShouldEmitUTF8Identifier = true)
    {
        return writerProvider.OpenFileWriter(
            Path.Combine(Config.OutputDirectory, fileName).Replace('\\', '/'),
            logger,
            encoderShouldEmitUTF8Identifier
        );
    }

    public IFileWriter OpenFileWriter(string fileName, Encoding encoding)
    {
        return writerProvider.OpenFileWriter(
            Path.Combine(Config.OutputDirectory, fileName).Replace('\\', '/'),
            logger,
            encoding
        );
    }

    protected abstract void HandleFiles(IEnumerable<ModelFile> files);
}
