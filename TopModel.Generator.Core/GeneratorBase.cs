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

    public T Config { get; internal set; } = null!;

    public int Number { get; internal set; }

    public virtual IEnumerable<string> GeneratedFiles => [];

    public bool Disabled => Config.Disable?.Contains(Name) ?? false;

    /// <inheritdoc cref="IModelWatcher.OnFilesChanged" />
    public async Task OnFilesChanged(
        IEnumerable<ModelFile> files,
        LoggingScope? storeConfig = null,
        CancellationToken ct = default
    )
    {
        using var scope = logger.BeginScope(((IModelWatcher)this).FullName);
        using var scope2 = logger.BeginScope(storeConfig!);
        await HandleFiles(files.Where(f => Config.Files.ContainsKey(f.Name)), ct);
    }

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

    protected abstract Task HandleFiles(IEnumerable<ModelFile> files, CancellationToken ct = default);
}
