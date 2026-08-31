using TopModel.Core.FileModel;

namespace TopModel.Core.Model;

public class PropertyMapping : IPropertyContainer
{
    public IProperty Property { get; internal set; } = null!;

    public IProperty TargetProperty { get; internal set; } = null!;

    public Reference TargetPropertyReference { get; internal set; } = null!;

    public required FromMapper FromMapper { get; init; }

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

    public IList<AnnotationInstance> ExcludedAnnotations => throw new NotSupportedException();

    public IList<AnnotationReference> ExcludedAnnotationReferences => throw new NotSupportedException();

    public IList<AnnotationInstance> PropertyAnnotations => throw new NotSupportedException();

    public IList<AnnotationReference> PropertyAnnotationReferences => throw new NotSupportedException();

    public IList<PropertySource> PropertySourceOrder => [PropertySource.Properties];
}
