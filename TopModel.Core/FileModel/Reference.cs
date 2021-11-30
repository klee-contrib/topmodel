#nullable disable

using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace TopModel.Core.FileModel;

internal class Reference
{
    public Reference()
    {
    }

    public Reference(Scalar scalar)
    {
        Start = scalar.Start;
        End = scalar.End;
        ReferenceName = scalar.Value;
    }

    public Mark Start { get; set; }

    public Mark End { get; set; }

    public string ReferenceName { get; set; }

    public string Position => $"[{Start.Line},{Start.Column}]";
}