using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core;

public class Decorator : IPropertyContainer, IVariableContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; set; }

    public Target? Target { get; set; }
#nullable enable

    public Dictionary<string, DecoratorImplementation> Implementations { get; set; } = [];

#nullable disable
    public ModelFile ModelFile { get; set; }

    public Namespace Namespace { get; set; }

    public IList<DecoratorInstance> Decorators { get; } = [];

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<IProperty> Properties { get; } = [];

    public bool PreservePropertyCasing { get; set; }

    public IList<TemplateParameter> TemplateParameters { get; internal set; } = [];

    public IEnumerable<ParameterReference> VariableReferences => Implementations.Values
        .SelectMany(i =>
            (IEnumerable<ParameterReference>)[
                ..i.Extends?.Variables ?? [],
                ..i.Implements.SelectMany(a => a.Variables),
                ..i.Imports.SelectMany(a => a.Variables)
            ])
        .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Variables)));

    public Dictionary<string, Variable> Variables { get; } = [];

    public IEnumerable<TransformReference> TransformReferences => Implementations.Values
       .SelectMany(i =>
           (IEnumerable<TransformReference>)[
                ..i.Extends?.Transforms ?? [],
                ..i.Implements.SelectMany(a => a.Transforms),
                ..i.Imports.SelectMany(a => a.Transforms)
           ])
        .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Transforms)))
        .Where(pr => pr.ReferenceName.IsValidTransform());

    public IList<DecoratorReference> DecoratorReferences { get; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; set; } = [];

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
