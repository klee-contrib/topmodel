using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

/// <summary>
/// Implémentation par défaut de IFileWriter.
/// </summary>
public class GeneratedFileWriter : IFileWriter
{
    /// <summary>
    /// Nombre de lignes d'en-tête à ignorer dans le calcul de checksum.
    /// </summary>
    private const int LinesInHeader = 4;

    private readonly ConfigBase _config;
    private readonly Encoding _encoding;
    private readonly ILogger _logger;
    private readonly StringBuilder _sb;

    internal GeneratedFileWriter(
        ConfigBase config,
        string fileName,
        ILogger logger,
        bool encoderShouldEmitUTF8Identifier
    )
        : this(config, fileName, logger, new UTF8Encoding(encoderShouldEmitUTF8Identifier)) { }

    internal GeneratedFileWriter(ConfigBase config, string fileName, ILogger logger, Encoding encoding)
    {
        _config = config;
        _encoding = encoding;
        _logger = logger;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        _sb = new StringBuilder();
    }

    /// <inheritdoc />
    public bool EnableHeader { get; set; } = true;

    /// <inheritdoc />
    public string FileName { get; }

    /// <inheritdoc />
    public string HeaderMessage { get; set; } = "ATTENTION CE FICHIER EST GENERE AUTOMATIQUEMENT !";

    /// <inheritdoc />
    public string IndentValue { get; set; } = "    ";

    /// <inheritdoc />
    public string StartCommentToken { get; set; } = "////";

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    public async ValueTask DisposeAsync()
    {
        if (Marshal.GetExceptionPointers() != IntPtr.Zero)
        {
            return;
        }

        string? currentContent = null;
        var fileExists = File.Exists(FileName.Replace('\\', '/'));
        if (fileExists)
        {
            using var reader = new StreamReader(FileName, _encoding);

            if (EnableHeader)
            {
                for (var i = 0; i < LinesInHeader; i++)
                {
                    await reader.ReadLineAsync();
                }
            }

            currentContent = await reader.ReadToEndAsync();

            var ignoredFile = _config.IgnoredFiles.FirstOrDefault(i =>
                Path.GetFullPath(Path.Combine(_config.ConfigRoot, i.Path)).Replace('\\', '/')
                == FileName.Replace('\\', '/')
            );
            if (ignoredFile != null)
            {
                if (!_config.NoWarn.Contains(ErrorType.TMD1004))
                {
                    _logger.LogWarning(
                        $"{{TMD1004}} - Le fichier '{ignoredFile.Path}' ne sera pas regénéré pour le motif : '{ignoredFile.Comment}'."
                    );
                }

                return;
            }
        }

        var newContent = _sb.ToString();
        if (newContent.ReplaceLineEndings() == currentContent?.ReplaceLineEndings())
        {
            return;
        }

        /* Création du répertoire si inexistant. */
        var dir = new FileInfo(FileName).DirectoryName!;
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        await using (var sw = new StreamWriter(FileName, append: false, _encoding))
        {
            if (EnableHeader && !newContent.StartsWith($"{StartCommentToken}{Environment.NewLine}"))
            {
                await sw.WriteLineAsync(StartCommentToken);
                await sw.WriteLineAsync($"{StartCommentToken} {HeaderMessage}");
                await sw.WriteLineAsync(StartCommentToken);
                await sw.WriteLineAsync();
            }

            await sw.WriteAsync(newContent);
        }

        _logger.LogInformation($"{(fileExists ? "Modifié:  " : "Créé:     ")}{FileName.ToRelative()}");
    }

    /// <inheritdoc cref="IFileWriter.Write" />
    public void Write(string? value)
    {
        _sb.Append(value);
    }
}
