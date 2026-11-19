using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

/// <summary>
/// Implémentation par défaut de IFileWriter.
/// </summary>
public class GeneratedFileWriter : IFileWriter
{
    private readonly ConfigBase _config;
    private readonly ILogger _logger;
    private readonly StringBuilder _sb = new();

    internal GeneratedFileWriter(ConfigBase config, string fileName, ILogger logger, FileWriterOptions options)
    {
        _config = config;
        _logger = logger;

        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        Options = options;

        if (options.EnableHeader)
        {
            var headerMessage = options.HeaderMessage
                ?? UtilsMessage.GeneratedFileHeader.GetLocalizedMessage(config.Language);

            _sb.AppendLine(options.StartCommentToken);
            _sb.AppendLine($"{options.StartCommentToken} {headerMessage}");
            _sb.AppendLine(options.StartCommentToken);
            _sb.AppendLine();
        }
    }

    /// <inheritdoc />
    public string FileName { get; }

    /// <inheritdoc />
    public string? Language => _config.Language;

    public FileWriterOptions Options { get; }

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync" />
    public async ValueTask DisposeAsync()
    {
        if (Marshal.GetExceptionPointers() != IntPtr.Zero)
        {
            return;
        }

        var newContent = _sb.ToString();

        var normalizedFileName = FileName.Replace('\\', '/');

        var fileExists = File.Exists(normalizedFileName);
        if (fileExists)
        {
            var ignoredFile = _config.IgnoredFiles.FirstOrDefault(i =>
                Path.GetFullPath(Path.Combine(_config.ConfigRoot, i.Path)).Replace('\\', '/') == normalizedFileName
            );
            if (ignoredFile != null)
            {
                if (!_config.NoWarn.Contains(ErrorType.TMD1004))
                {
                    _logger.LogWarning(ErrorType.TMD1004, ignoredFile.Path, ignoredFile.Comment);
                }

                return;
            }

            using var currentReader = new StreamReader(FileName, Options.Encoding);
            using var newReader = new StringReader(newContent);
            var contentIsEqual = true;

            while (true)
            {
                var currentLine = await currentReader.ReadLineAsync();
                var newLine = await newReader.ReadLineAsync();

                if (currentLine == null || newLine == null)
                {
                    contentIsEqual = currentLine == null && newLine == null;
                    break;
                }

                if (currentLine != newLine)
                {
                    contentIsEqual = false;
                    break;
                }
            }

            if (contentIsEqual)
            {
                return;
            }
        }

        var dir = new FileInfo(FileName).DirectoryName!;
        Directory.CreateDirectory(dir);

        await using (var writer = new StreamWriter(FileName, append: false, Options.Encoding))
        {
            await writer.WriteAsync(newContent);
        }

        _logger.LogInformation(
            fileExists ? UtilsMessage.FileModified : UtilsMessage.FileCreated,
            FileName.ToRelative()
        );
    }

    /// <inheritdoc cref="IFileWriter.Write" />
    public void Write(string? value)
    {
        _sb.Append(value);
    }
}
