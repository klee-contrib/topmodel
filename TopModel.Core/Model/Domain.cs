using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Model;

public class Domain : IAnnotationContainer, IVariableContainer
{
#nullable disable
    public LocatedString Name { get; set; }

    public string Label { get; set; }

#nullable enable

    // Liste des converter pour lesquels le converter est présent dans la liste des domains From
    public ISet<Converter> ConvertersFrom { get; set; } = new HashSet<Converter>();

    public ISet<Converter> ConvertersTo { get; set; } = new HashSet<Converter>();

    public int? Length { get; set; }

    public int? Scale { get; set; }

    public GeneratedValueDefinition? GeneratedValue { get; set; }

    [Obsolete("Utiliser ParamLocation == ParamLocation.JsonBody")]
    public bool BodyParam => ParamLocation == Model.ParamLocation.JsonBody;

    public ParamLocation? ParamLocation { get; set; }

    public bool Collection { get; set; }

    public bool NonGeneric => Implementations.Values.All(i => i.Type != null);

    public bool Generic => Implementations.Values.All(i => i.GenericType != null);

    public IDictionary<string, Domain> AsDomains { get; set; } = new Dictionary<string, Domain>();

    public IList<AnnotationInstance> Annotations { get; } = [];

    public IList<AnnotationReference> AnnotationReferences { get; internal set; } = [];

    public IList<AnnotationInstance> ExcludedAnnotations { get; internal set; } = [];

    public IList<AnnotationReference> ExcludedAnnotationReferences { get; internal set; } = [];

    public IDictionary<string, DomainReference> AsDomainReferences { get; internal set; } =
        new Dictionary<string, DomainReference>();

    public IDictionary<string, DomainImplementation> Implementations { get; internal set; } =
        new Dictionary<string, DomainImplementation>();

    public IList<TemplateParameter> TemplateParameters { get; internal set; } = [];

    public string? MediaType { get; set; }

    [Obsolete("Utiliser ParamLocation == ParamLocation.FormData")]
    public bool IsMultipart => ParamLocation == Model.ParamLocation.FormData;

    public string CSharpName => Name.Replace("DO_", string.Empty).ToPascalCase(strict: true);

    public IEnumerable<ParameterReference> VariableReferences =>
        Implementations
            .Values.SelectMany(i =>
                (IEnumerable<ParameterReference>)
                    [
                        .. i.Type?.Variables ?? [],
                        .. i.GenericType?.Variables ?? [],
                        .. i.Imports.SelectMany(a => a.Variables),
                        .. i.ValueTemplates.Values.SelectMany(a => a.Value.Variables),
                        .. i.ValueTemplates.Values.SelectMany(a => a.Imports.SelectMany(vi => vi.Variables)),
                    ]
            )
            .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Variables)))
            .Concat(GeneratedValue?.SequenceName?.Variables ?? []);

    public IDictionary<string, Variable> Variables { get; } = new Dictionary<string, Variable>();

    public IEnumerable<TransformReference> TransformReferences =>
        Implementations
            .Values.SelectMany(i =>
                (IEnumerable<TransformReference>)
                    [
                        .. i.Type?.Transforms ?? [],
                        .. i.GenericType?.Transforms ?? [],
                        .. i.Imports.SelectMany(a => a.Transforms),
                        .. i.ValueTemplates.Values.SelectMany(a => a.Value.Transforms),
                        .. i.ValueTemplates.Values.SelectMany(a => a.Imports.SelectMany(vi => vi.Transforms)),
                    ]
            )
            .Concat(AnnotationReferences.SelectMany(a => a.ParameterReferences.Values.SelectMany(v => v.Transforms)))
            .Concat(GeneratedValue?.SequenceName?.Transforms ?? [])
            .Where(pr => pr.ReferenceName.IsValidTransform());

#nullable disable
    public ModelFile ModelFile { get; set; }

    internal Reference Location { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
