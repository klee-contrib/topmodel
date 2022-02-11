using TopModel.Core.FileModel;

namespace TopModel.Core;

public class LocatedString
{
    public string? Value { get; set; }

#nullable disable
    internal Reference Location { get; set; }

    public override string ToString()
    {
        return this.Value;
    }

}