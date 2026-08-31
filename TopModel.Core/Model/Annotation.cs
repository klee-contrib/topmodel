using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Annotation(Reference location) : IVariableContainer
{
    public LocatedString Name { get; internal set; } = null!;

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; internal set; } = null!;

    public IList<Target> Target { get; internal set; } = [];

    public bool Global { get; internal set; }

    public IList<TemplateParameter> TemplateParameters { get; internal set; } = [];

    public IDictionary<string, IList<AnnotationImplementation>> Implementations { get; internal set; } =
        new Dictionary<string, IList<AnnotationImplementation>>();

    public required ModelFile ModelFile { get; init; }

    public required Namespace Namespace { get; init; }

    public IEnumerable<ParameterReference> VariableReferences =>
        Implementations.Values.SelectMany(i =>
            (IEnumerable<ParameterReference>)
                [.. i.SelectMany(a => a.Text.Variables), .. i.SelectMany(a => a.Imports.SelectMany(ai => ai.Variables))]
        );

    public IDictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

    public IEnumerable<TransformReference> TransformReferences =>
        Implementations
            .Values.SelectMany(i =>
                (IEnumerable<TransformReference>)
                    [
                        .. i.SelectMany(a => a.Text.Transforms),
                        .. i.SelectMany(a => a.Imports.SelectMany(ai => ai.Transforms)),
                    ]
            )
            .Where(pr => pr.ReferenceName.IsValidTransform());

    internal Reference Location { get; } = location;

    public override string ToString()
    {
        return Name;
    }
}
