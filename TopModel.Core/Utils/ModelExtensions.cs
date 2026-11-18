using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.Core.Utils;

public static class ModelExtensions
{
    extension(object? objet)
    {
        public DeclarationReference? DeclarationReference =>
            objet switch
            {
                Class c => c.DeclarationReference,
                Endpoint e => e.DeclarationReference,
                RegularProperty p => p.OwnName.DeclarationReference,
                AssociationProperty p => p.DeclarationReference,
                CompositionProperty p => p.OwnName.DeclarationReference,
                AliasProperty p => p.OwnName?.DeclarationReference ?? p.DeclarationReference,
                Domain d => d.DeclarationReference,
                LocatedString l => l.DeclarationReference,
                Decorator d => d.DeclarationReference,
                DecoratorInstance { Decorator: Decorator d } => d.DeclarationReference,
                Annotation a => a.DeclarationReference,
                AnnotationInstance { Annotation: Annotation a } => a.DeclarationReference,
                DataFlow d => d.DeclarationReference,
                FromMapper m => m.Reference.DeclarationReference,
                ClassMappings c => c.Name.DeclarationReference,
                PropertyMapping p => p.Property.DeclarationReference,
                MapperParam p => p switch
                {
                    ClassMappings c => c.DeclarationReference,
                    PropertyMapping pp => pp.DeclarationReference,
                },
                Converter c => c.DeclarationReference,
                TemplateParameter t => t.Name.DeclarationReference,
                Variable { TemplateParameter: TemplateParameter t } => t.Name.DeclarationReference,
                ReverseAssociationDefinition rad => rad.Property.DeclarationReference,
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
