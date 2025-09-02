#pragma warning disable MA0048

namespace TopModel.Core.FileModel;

public class ModelFileOptions
{
    public EndpointOptions Endpoints { get; set; } = new();
}

public class EndpointOptions
{
#nullable disable
    public string FileName { get; set; }

#nullable enable

    public LocatedString? Prefix { get; set; }
}
