using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

public class Reference
{
    internal Reference()
    {
        ReferenceName = string.Empty;
        Scalar = new Scalar(string.Empty);
    }

    internal Reference(Scalar scalar)
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
