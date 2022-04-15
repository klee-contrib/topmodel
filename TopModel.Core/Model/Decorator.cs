using TopModel.Core.FileModel;
using TopModel.Core.Types;
using YamlDotNet.Serialization;

namespace TopModel.Core.Model;

public class Decorator
{
#nullable disable
    public string Name { get; set; }
#nullable enable

    [YamlMember(Alias = "csharp")]
    public CSharpDecorator? CSharp { get; set; }

    public JavaDecorator? Java { get; set; }

#nullable disable
    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }
}
