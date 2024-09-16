using TopModel.Utils;

namespace TopModel.Generator;

public record ModgenDependency(string ConfigKey, TopModelLockModule Version)
{
    public string FullName => $"TopModel.Generator.{ConfigKey.ToFirstUpper()}";

    public string? LatestVersion { get; set; }
}
