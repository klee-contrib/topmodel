using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

public static class ModelExtensions
{
    extension(object? objet)
    {
        public Declaration? Declaration =>
            objet switch
            {
                Class c => c.Declaration,
                Endpoint e => e.Declaration,
                RegularProperty p => p.OwnName.Declaration,
                AssociationProperty p => p.Declaration,
                CompositionProperty p => p.OwnName.Declaration,
                AliasProperty p => p.OwnName?.Declaration ?? p.Declaration,
                Domain d => d.Declaration,
                LocatedString l => l.Declaration,
                Decorator d => d.Declaration,
                DecoratorInstance { Decorator: Decorator d } => d.Declaration,
                Annotation a => a.Declaration,
                AnnotationInstance { Annotation: Annotation a } => a.Declaration,
                DataFlow d => d.Declaration,
                FromMapper m => m.Reference.Declaration,
                ClassMappings c => c.Name.Declaration,
                PropertyMapping p => p.Property.Declaration,
                MapperParam p => p switch
                {
                    ClassMappings c => c.Declaration,
                    PropertyMapping pp => pp.Declaration,
                },
                Converter c => c.Declaration,
                TemplateParameter t => t.Name.Declaration,
                Variable { TemplateParameter: TemplateParameter t } => t.Name.Declaration,
                ReverseAssociationDefinition rad => rad.Property.Declaration,
                _ => null,
            };

        public ModelFile ModelFile =>
            objet switch
            {
                ModelFile file => file,
                Class classe => classe.ModelFile,
                Endpoint endpoint => endpoint.ModelFile,
                IProperty { SourceContainer: IPropertyContainer container } => container.ModelFile,
                IProperty { Decorator: Decorator decorator } => decorator.ModelFile,
                IProperty { Class: Class classe } => classe.ModelFile,
                IProperty { Endpoint: Endpoint endpoint } => endpoint.ModelFile,
                IProperty { PropertyMapping: PropertyMapping param } => param.FromMapper.Class.ModelFile,
                Domain domain => domain.ModelFile,
                DomainInstance { Domain: Domain domain } => domain.ModelFile,
                Converter converter => converter.ModelFile,
                Decorator decorator => decorator.ModelFile,
                DecoratorInstance { Decorator: Decorator decorator } => decorator.ModelFile,
                Annotation annotation => annotation.ModelFile,
                AnnotationInstance { Annotation: Annotation annotation } => annotation.ModelFile,
                DataFlow dataFlow => dataFlow.ModelFile,
                Keyword keyword => keyword.ModelFile,
                ClassValue classValue => classValue.Class.ModelFile,
                TemplateParameter templateParameter => templateParameter.Domain?.ModelFile
                    ?? templateParameter.Decorator?.ModelFile
                    ?? templateParameter.Annotation!.ModelFile,
                Variable { TemplateParameter: TemplateParameter templateParameter } => templateParameter
                    .Domain
                    ?.ModelFile
                    ?? templateParameter.Decorator?.ModelFile
                    ?? templateParameter.Annotation!.ModelFile,
                Variable => new ModelFile { Name = string.Empty, Path = string.Empty },
                ReverseAssociationDefinition { Property.Decorator: Decorator decorator } => decorator.ModelFile,
                ReverseAssociationDefinition { Property.Class: Class classe } => classe.ModelFile,
                ReverseAssociationDefinition { Property.Endpoint: Endpoint endpoint } => endpoint.ModelFile,
                ReverseAssociationDefinition { Property.PropertyMapping: PropertyMapping param } => param
                    .FromMapper
                    .Class
                    .ModelFile,
                _ => throw new InvalidOperationException("Type d'objet non supporté."),
            };
    }
}
