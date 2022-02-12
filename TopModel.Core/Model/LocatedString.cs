#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core;

public class LocatedString
{
    public string Value { get; set; }

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Value;
    }
}