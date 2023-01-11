using TopModel.Core.FileModel;
using TopModel.Core.Types;
using YamlDotNet.Serialization;

namespace TopModel.Core;

public class Decorator
{
#nullable disable
    public LocatedString Name { get; set; }

    public string Description { get; set; }
#nullable enable

    [YamlMember(Alias = "csharp")]
    public CSharpDecorator? CSharp { get; set; }

    public JavaDecorator? Java { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    public IList<IProperty> Properties { get; } = new List<IProperty>();

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
