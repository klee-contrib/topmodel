using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Annotation : IVariableContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; set; }

#nullable enable

    public IList<Target> Target { get; set; } = [];

    public bool Global { get; set; }

    public IList<TemplateParameter> TemplateParameters { get; internal set; } = [];

    public IDictionary<string, IList<AnnotationImplementation>> Implementations { get; set; } =
        new Dictionary<string, IList<AnnotationImplementation>>();

#nullable disable
    public ModelFile ModelFile { get; set; }

    public Namespace Namespace { get; set; }

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

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
