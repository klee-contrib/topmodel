using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core;

public class Decorator : IPropertyContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string NamePascal => Name.Value.ToPascalCase();

    public string NameCamel => Name.Value.ToCamelCase();

    public string Description { get; set; }
#nullable enable

    public Dictionary<string, DecoratorImplementation> Implementations { get; set; } = [];

#nullable disable
    public ModelFile ModelFile { get; set; }

    public Namespace Namespace { get; set; }

    public List<(Decorator Decorator, string[] Parameters)> Decorators { get; } = [];

    public IList<IProperty> Properties { get; } = [];

    public bool PreservePropertyCasing { get; set; }

    public IList<TemplateParameter> TemplateParameters { get; set; } = [];

    public IEnumerable<ParameterReference> VariableReferences => Implementations.Values
        .SelectMany(i =>
            (IEnumerable<ParameterReference>)[
                ..i.Extends?.Variables ?? [],
                ..i.Implements.SelectMany(a => a.Variables),
                ..i.Annotations.SelectMany(a => a.Variables),
                ..i.Imports.SelectMany(a => a.Variables)
            ]);

    public Dictionary<string, Variable> Variables { get; } = [];

    public IEnumerable<TransformReference> TransformReferences => Implementations.Values
       .SelectMany(i =>
           (IEnumerable<TransformReference>)[
                ..i.Extends?.Transforms ?? [],
                ..i.Implements.SelectMany(a => a.Transforms),
                ..i.Annotations.SelectMany(a => a.Transforms),
                ..i.Imports.SelectMany(a => a.Transforms)
           ])
       .Where(pr => pr.ReferenceName.IsValidTransform());

    public List<DecoratorReference> DecoratorReferences { get; } = [];

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
