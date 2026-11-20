using TopModel.Core.Model.Implementation;

namespace TopModel.Core.Model;

public record DomainInstance(Domain Domain, IDictionary<string, string> Parameters) : ParametersInstance(Parameters)
{
    public string Name => Domain.Name;

    public string? Label => Domain.Label;

    public ParamLocation? ParamLocation => Domain.ParamLocation;

    public string? MediaType => Domain.MediaType;

    public IDictionary<string, Domain> AsDomains => Domain.AsDomains;

    public GeneratedValueDefinition? GeneratedValue => Domain.GeneratedValue;

    public int? Length => Domain.Length;

    public int? Scale => Domain.Scale;

    public bool Collection => Domain.Collection;

    public string? DefaultValue => Domain.DefaultValue;

    public IList<AnnotationInstance> Annotations => Domain.Annotations;

    public IList<AnnotationInstance> ExcludedAnnotations => Domain.ExcludedAnnotations;

    public IDictionary<string, DomainImplementation> Implementations => Domain.Implementations;

    public IList<TemplateParameter> TemplateParameters => Domain.TemplateParameters;

    public Converter? GetConverter(DomainInstance? toDomain)
    {
        if (toDomain != null && Domain != toDomain.Domain)
        {
            return Domain.ConvertersFrom.FirstOrDefault(c => c.From.Contains(Domain) && c.To.Contains(toDomain.Domain));
        }

        return null;
    }
}
