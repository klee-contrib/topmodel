#pragma warning disable MA0048

namespace TopModel.Core.FileModel;

public class ModelFileOptions
{
    public EndpointOptions Endpoints { get; } = new();
}

public class EndpointOptions
{
    public string FileName { get; internal set; } = null!;

    public LocatedString? Prefix { get; internal set; }
}
