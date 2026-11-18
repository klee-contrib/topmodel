using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DomainResolver(IList<ModelFile> modelFiles, ModelConfig config, IDictionary<string, Domain> domains)
    : BaseResolver
{
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
                if (varName.Value.TryGetConverterVariable(out var variable))
                {
                    converter.Variables.TryAdd(varName.Value, variable);
                }
                else
                {
                    yield return ModelError.Warning(ErrorType.TMD0011, [varName.Value], converter, varName);
                }
            }

            converter.From.Clear();
            converter.To.Clear();

            foreach (var dom in converter.DomainsFromReferences)
            {
                if (!domains.TryGetValue(dom.Value, out var domain))
                {
                    yield return ModelError.Error(ErrorType.TMD0003, [dom.Value], converter, dom);
                    break;
                }

                converter.From.Add(domain);
            }

            foreach (var dom in converter.DomainsToReferences)
            {
                if (!domains.TryGetValue(dom.Value, out var domain))
                {
                    yield return ModelError.Error(ErrorType.TMD0003, [dom.Value], converter, dom);
                    break;
                }

                converter.To.Add(domain);
            }
        }
    }

    /// <summary>
    /// Résout les domaines (as et variables).
    /// </summary>
    /// <param name="config">Config.</param>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDomains()
    {
        foreach (var domain in modelFiles.SelectMany(mf => mf.Domains))
        {
            foreach (var (asName, domainReference) in domain.AsDomainReferences)
            {
                if (!domains.TryGetValue(domainReference.Value, out var asDomain))
                {
                    yield return ModelError.Error(ErrorType.TMD0003, [domainReference.Value], domain, domainReference);
                    continue;
                }

                domain.AsDomains[asName] = asDomain;
            }

            foreach (var templateParam in domain.TemplateParameters.GetDuplicates(p => p.Name))
            {
                yield return ModelError.Error(
                    ErrorType.TMD0001,
                    [templateParam.Name],
                    domain,
                    templateParam.Declaration
                );
            }

            if (domain.Collection && !domain.Generic)
            {
                yield return ModelError.Error(ErrorType.TMD6004, [], domain, domain.Declaration);
            }

            domain.Variables.Clear();

            foreach (var varName in domain.VariableReferences)
            {
                if (varName.Value.TryGetPropertyVariable(config, domain.TemplateParameters, out var variable))
                {
                    domain.Variables.TryAdd(varName.Value, variable);
                }
                else
                {
                    yield return ModelError.Warning(ErrorType.TMD0011, [varName.Value], domain, varName);
                }
            }
        }
    }
}
