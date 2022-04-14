using TopModel.Core;

namespace TopModel.Generator.Jpa;

public class AliasClass
{
    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

    #nullable disable
    public Class Class { get; init; }

    #nullable enable
    public string Name => $"{Prefix ?? string.Empty}{Class}{Suffix ?? string.Empty}";
}
