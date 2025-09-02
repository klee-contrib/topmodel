using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.ModelGenerator;

public abstract class TmdGenerator(ILogger<TmdGenerator> logger)
{
    public abstract string Name { get; }

    public int Number { get; init; }

    public required string DirectoryName { get; init; }

    public required string ModelRoot { get; init; }

    public string FullName => $"{Name.PadRight(18, '.')}@{Number}";

    public async Task<IList<string>> Generate(LoggingScope scope, CancellationToken ct = default)
    {
        using var scope1 = logger.BeginScope(FullName);
        using var scope2 = logger.BeginScope(scope);
        try
        {
            var files = new List<string>();

            await foreach (var item in GenerateCore(ct))
            {
                files.Add(item);
            }

            return files;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return [];
        }
    }

    protected abstract IAsyncEnumerable<string> GenerateCore(CancellationToken ct = default);
}
