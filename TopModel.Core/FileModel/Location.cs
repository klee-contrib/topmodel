using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public abstract class Location
{
    private protected Location()
    {
        Value = string.Empty;
        Scalar = new Scalar(string.Empty);
    }

    private protected Location(Scalar scalar)
    {
        Start = scalar.Start;
        End = scalar.End;
        Value = scalar.Value;
        Scalar = scalar;
    }

    public Mark Start { get; init; }

    public Mark End { get; init; }

    public string Value { get; init; }

    public string Position => $"[{Start.Line},{Start.Column}]";

    internal Scalar Scalar { get; }
}
