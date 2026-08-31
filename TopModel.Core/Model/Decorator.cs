using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Decorator(Reference location) : IPropertyContainer, IVariableContainer
{
    public LocatedString Name { get; internal set; } = null!;

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; internal set; } = null!;

    public Target? Target { get; internal set; }

    public IDictionary<string, DecoratorImplementation> Implementations { get; internal set; } =
        new Dictionary<string, DecoratorImplementation>();

    public required ModelFile ModelFile { get; init; }

    public required Namespace Namespace { get; init; }

    public IList<DecoratorInstance> Decorators { get; } = [];

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; } = [];

    public IList<AnnotationInstance> PropertyAnnotations { get; } = [];

    public IList<IProperty> Properties { get; } = [];

    public bool PreservePropertyCasing { get; internal set; }

    public IList<TemplateParameter> TemplateParameters { get; internal set; } = [];

    public IEnumerable<ParameterReference> VariableReferences =>
        Implementations
            .Values.SelectMany(i =>
                (IEnumerable<ParameterReference>)
                    [
                        .. i.Extends?.Variables ?? [],
                        .. i.Implements.SelectMany(a => a.Variables),
                        .. i.Imports.SelectMany(a => a.Variables),
                    ]
            )
            .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Variables)))
            .Concat(
                PropertyAnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Variables))
            );

    public IDictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

    public IEnumerable<TransformReference> TransformReferences =>
        Implementations
            .Values.SelectMany(i =>
                (IEnumerable<TransformReference>)
                    [
                        .. i.Extends?.Transforms ?? [],
                        .. i.Implements.SelectMany(a => a.Transforms),
                        .. i.Imports.SelectMany(a => a.Transforms),
                    ]
            )
            .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Transforms)))
            .Concat(
                PropertyAnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Transforms))
            )
            .Where(pr => pr.ReferenceName.IsValidTransform());

    public IList<DecoratorReference> DecoratorReferences { get; internal set; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IList<AnnotationReference> PropertyAnnotationReferences { get; internal set; } = [];

    public IList<PropertySource> PropertySourceOrder { get; internal set; } =
    [PropertySource.Properties, PropertySource.Decorators];

    internal IList<IProperty> OwnProperties { get; } = [];

    internal Reference Location { get; } = location;

    public override string ToString()
    {
        return Name;
    }
}
