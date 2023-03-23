using Microsoft.Extensions.Logging;
using TopModel.Utils;

namespace TopModel.ModelGenerator;

public abstract class ModelGenerator
{
    private readonly ILogger<ModelGenerator> _logger;

#pragma warning disable CS8618 
    protected ModelGenerator(ILogger<ModelGenerator> logger)
    {
        _logger = logger;
    }
#pragma warning restore CS8618

    public abstract string Name { get; }

    public int Number { get; init; }

    public string DirectoryName { get; init; }

    public string ModelRoot { get; init; }

    string FullName => $"{Name.PadRight(18, '.')}@{Number}";

    public async Task<List<string>> Generate(LoggingScope scope)
    {

        using var scope1 = _logger.BeginScope(FullName);
        using var scope2 = _logger.BeginScope(scope);
        try
        {
            var files = new List<string>();

            await foreach (var item in GenerateCore())
            {
                files.Add(item);
            }

            return files;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new List<string>();
        }
    }

    protected abstract IAsyncEnumerable<string> GenerateCore();
}
