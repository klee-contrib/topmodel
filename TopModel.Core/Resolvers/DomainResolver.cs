using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DomainResolver(ModelFile modelFile, ModelConfig config, IDictionary<string, Domain> domains, IEnumerable<Converter> converters)
{
    /// <summary>
    /// Résout les `asDomains` sur les domaines.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveAsDomains()
    {
        foreach (var domain in modelFile.Domains)
        {
            foreach (var (asName, domainReference) in domain.AsDomainReferences)
            {
                if (!domains.TryGetValue(domainReference.ReferenceName, out var asDomain))
                {
                    yield return new ModelError(domain, "Le domaine '{0}' est introuvable.", domainReference) { ModelErrorType = ModelErrorType.TMD1005 };
                    continue;
                }

                domain.AsDomains[asName] = asDomain;
            }

            foreach (var templateParam in domain.TemplateParameters.Where((e, i) => domain.TemplateParameters.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(domain, $"Le nom '{templateParam.Name}' est déjà utilisé.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
            }
        }
    }

    /// <summary>
    /// Résout les convertisseurs.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveConverters()
    {
        foreach (var converter in converters)
        {
            converter.Variables.Clear();

            foreach (var varName in converter.VariableReferences)
            {
                if (varName.ReferenceName.TryGetConverterVariable(out var variable))
                {
                    converter.Variables.TryAdd(varName.ReferenceName, variable);
                }
            }

            converter.From.Clear();
            converter.To.Clear();

            foreach (var domain in domains.Values)
            {
                domain.ConvertersFrom.Remove(converter);
                domain.ConvertersTo.Remove(converter);
            }

            foreach (var dom in converter.DomainsFromReferences)
            {
                if (!domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(converter, "Le domaine '{0}' est introuvable.", dom) { ModelErrorType = ModelErrorType.TMD1005 };
                    break;
                }

                converter.From.Add(domain);
            }

            foreach (var dom in converter.DomainsToReferences)
            {
                if (!domains.TryGetValue(dom.ReferenceName, out var domain))
                {
                    yield return new ModelError(converter, "Le domaine '{0}' est introuvable.", dom) { ModelErrorType = ModelErrorType.TMD1005 };
                    break;
                }

                converter.To.Add(domain);
            }

            foreach (var f in converter.From)
            {
                f.ConvertersFrom.Add(converter);
            }

            foreach (var f in converter.To)
            {
                f.ConvertersTo.Add(converter);
            }
        }
    }

    /// <summary>
    /// Résout les variables dans les domaines.
    /// </summary>
    /// <param name="config">Config.</param>
    public void ResolveDomainVariables()
    {
        foreach (var domain in modelFile.Domains)
        {
            domain.Variables.Clear();

            foreach (var varName in domain.VariableReferences)
            {
                if (varName.ReferenceName.TryGetPropertyVariable(config, domain.TemplateParameters, out var variable))
                {
                    domain.Variables.TryAdd(varName.ReferenceName, variable);
                }
            }
        }
    }
}
