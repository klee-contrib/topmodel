using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core;

public class Annotation : IVariableContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; set; }
#nullable enable

    public IList<TemplateParameter> TemplateParameters { get; set; } = [];

    public Dictionary<string, IList<AnnotationImplementation>> Implementations { get; set; } = [];

#nullable disable
    public ModelFile ModelFile { get; set; }

    public Namespace Namespace { get; set; }

    public IEnumerable<ParameterReference> VariableReferences => Implementations.Values
        .SelectMany(i =>
            (IEnumerable<ParameterReference>)[
                ..i.SelectMany(a => a.Text.Variables),
                ..i.SelectMany(a => a.Imports.SelectMany(ai => ai.Variables))
            ]);

    public Dictionary<string, Variable> Variables { get; } = [];

    public IEnumerable<TransformReference> TransformReferences => Implementations.Values
        .SelectMany(i =>
            (IEnumerable<TransformReference>)[
                ..i.SelectMany(a => a.Text.Transforms),
                ..i.SelectMany(a => a.Imports.SelectMany(ai => ai.Transforms))
            ])
        .Where(pr => pr.ReferenceName.IsValidTransform());

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
