using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public abstract class Reference
{
    private protected Reference()
    {
        ReferenceName = string.Empty;
        Scalar = new Scalar(string.Empty);
    }

    private protected Reference(Scalar scalar)
    {
        Start = scalar.Start;
        End = scalar.End;
        ReferenceName = scalar.Value;
        Scalar = scalar;
    }

    public Mark Start { get; init; }

    public Mark End { get; init; }

    public string ReferenceName { get; init; }

    public string Position => $"[{Start.Line},{Start.Column}]";

    internal Scalar Scalar { get; }
}
