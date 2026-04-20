using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DomainResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
    ModelConfig config,
    IDictionary<string, Domain> domains
)
{
    /// <summary>
    /// Résout les `asDomains` sur les domaines.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveAsDomains()
    {
        foreach (var domain in modelFiles.SelectMany(mf => mf.Domains))
        {
            foreach (var (asName, domainReference) in domain.AsDomainReferences)
            {
                if (!domains.TryGetValue(domainReference.ReferenceName, out var asDomain))
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0003,
                        [domainReference.ReferenceName],
                        domain,
                        domainReference
                    );
                    continue;
                }

                domain.AsDomains[asName] = asDomain;
            }

            foreach (var templateParam in domain.TemplateParameters.GetDuplicates(p => p.Name))
            {
                yield return new ModelError(
                    localizer,
                    ErrorType.TMD0001,
                    [templateParam.Name],
                    domain,
                    templateParam.GetLocation()
                );
            }

            if (domain.Collection && !domain.Generic)
            {
                yield return new ModelError(localizer, ErrorType.TMD6004, [], domain, domain.GetLocation());
            }
        }
    }

    /// <summary>
    /// Résout les convertisseurs.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveConverters()
    {
        foreach (var converter in modelFiles.SelectMany(mf => mf.Converters))
        {
            converter.Variables.Clear();

            foreach (var varName in converter.VariableReferences)
            {
                if (varName.ReferenceName.TryGetConverterVariable(out var variable))
                {
                    converter.Variables.TryAdd(varName.ReferenceName, variable);
                }
                else
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0011,
                        [varName.ReferenceName],
                        converter,
                        varName,
                        isError: false
                    );
                }
            }

            converter.From.Clear();
            converter.To.Clear();

            foreach (var dom in converter.DomainsFromReferences)
            {
                if (!domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(localizer, ErrorType.TMD0003, [dom.ReferenceName], converter, dom);
                    break;
                }

                converter.From.Add(domain);
            }

            foreach (var dom in converter.DomainsToReferences)
            {
                if (!domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(localizer, ErrorType.TMD0003, [dom.ReferenceName], converter, dom);
                    break;
                }

                converter.To.Add(domain);
            }
        }
    }

    /// <summary>
    /// Résout les variables dans les domaines.
    /// </summary>
    /// <param name="config">Config.</param>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDomainVariables()
    {
        foreach (var domain in modelFiles.SelectMany(mf => mf.Domains))
        {
            domain.Variables.Clear();

            foreach (var varName in domain.VariableReferences)
            {
                if (varName.ReferenceName.TryGetPropertyVariable(config, domain.TemplateParameters, out var variable))
                {
                    domain.Variables.TryAdd(varName.ReferenceName, variable);
                }
                else
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0011,
                        [varName.ReferenceName],
                        domain,
                        varName,
                        isError: false
                    );
                }
            }
        }
    }
}
