#pragma warning disable SA1402

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

    public string? Prefix { get; set; }
}