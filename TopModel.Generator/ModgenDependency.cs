using TopModel.Utils;

namespace TopModel.Generator;

public record ModgenDependency(string ConfigKey, string Version)
{
    public string FullName => $"TopModel.Generator.{ConfigKey.ToFirstUpper()}";
}
