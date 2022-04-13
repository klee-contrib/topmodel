using TopModel.Core;

namespace TopModel.Generator.Jpa;

public class AliasClass
{
    public string? Prefix { get; set; }

    public string? Suffix { get; set; }

    public Class Class { get; set; }

    public string Name => $"{(Prefix is not null ? Prefix : string.Empty)}{Class}{(Suffix is not null ? Suffix : string.Empty)}";
}