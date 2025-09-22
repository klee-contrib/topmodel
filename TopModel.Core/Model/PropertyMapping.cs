#nullable disable
using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class PropertyMapping : IPropertyContainer
{
    public IProperty Property { get; set; }

    public IProperty TargetProperty { get; set; }

    public Reference TargetPropertyReference { get; set; }

    public FromMapper FromMapper { get; set; }

    public ModelFile ModelFile => throw new NotSupportedException();

    public LocatedString Name => throw new NotSupportedException();

    public string NamePascal => Property.NamePascal;

    public string NameCamel => Property.NameCamel;

    public Namespace Namespace => throw new NotSupportedException();

    public IList<IProperty> Properties => [Property];

    public bool PreservePropertyCasing => false;

    public IList<DecoratorInstance> Decorators => throw new NotSupportedException();

    public IList<DecoratorReference> DecoratorReferences => throw new NotSupportedException();

    public IList<AnnotationInstance> Annotations => throw new NotSupportedException();

    public IList<AnnotationReference> AnnotationReferences => throw new NotSupportedException();

    public IList<AnnotationInstance> PropertyAnnotations => throw new NotSupportedException();

    public IList<AnnotationReference> PropertyAnnotationReferences => throw new NotSupportedException();
}
