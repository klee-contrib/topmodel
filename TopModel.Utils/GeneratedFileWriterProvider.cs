using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

/// <summary>
/// Implémentation par défaut de IFileWriterProvider.
/// </summary>
public class GeneratedFileWriterProvider(ConfigBase config) : IFileWriterProvider
{
    /// <inheritdoc cref="IFileWriterProvider.OpenFileWriter" />
    public IFileWriter OpenFileWriter(string fileName, ILogger logger, FileWriterOptions options)
    {
        return new GeneratedFileWriter(config, fileName, logger, options);
    }
}
