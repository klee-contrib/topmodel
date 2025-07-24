using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DecoratorResolver(ModelFile modelFile, ModelConfig config, IDictionary<string, Decorator> referencedDecorators)
{
    /// <summary>
    /// Recopie les propriétés de décorateurs sur les classes et les endpoints.
    /// </summary>
    public void CopyDecoratorProperties()
    {
        foreach (var decorator in modelFile.Decorators)
        {
            if (decorator.Decorators.Count > 0)
            {
                foreach (var prop in decorator.Properties.Where(p => p.SourceDecorator is not null).ToList())
                {
                    decorator.Properties.Remove(prop);
                }

                foreach (var prop in decorator.Decorators.SelectMany(d => d.Decorator.Properties))
                {
                    decorator.Properties.Add(prop.CloneForDecorator(decorator: decorator));
                }
            }
        }

        foreach (var classe in modelFile.Classes)
        {
            if (classe.Decorators.Count > 0)
            {
                foreach (var prop in classe.Properties.Where(p => p.SourceDecorator is not null).ToList())
                {
                    classe.Properties.Remove(prop);
                }

                foreach (var prop in classe.Decorators.SelectMany(d => d.Decorator.Properties))
                {
                    classe.Properties.Add(prop.CloneForDecorator(classe: classe));
                }
            }
        }

        foreach (var endpoint in modelFile.Endpoints)
        {
            if (endpoint.Decorators.Count > 0)
            {
                foreach (var prop in endpoint.Params.Where(p => p.SourceDecorator is not null).ToList())
                {
                    endpoint.Params.Remove(prop);
                }

                foreach (var prop in endpoint.Decorators.SelectMany(d => d.Decorator.Properties))
                {
                    endpoint.Params.Add(prop.CloneForDecorator(endpoint: endpoint));
                }
            }
        }
    }

    /// <summary>
    /// Résout les décorateurs sur les classes et les endpoints.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDecorators()
    {
        foreach (var decorator in modelFile.Decorators)
        {
            decorator.Variables.Clear();

            foreach (var varName in decorator.VariableReferences)
            {
                if (varName.ReferenceName.TryGetClassVariable(config, decorator.TemplateParameters, out var cVariable))
                {
                    decorator.Variables.TryAdd(varName.ReferenceName, cVariable);
                }

                if (varName.ReferenceName.TryGetEndpointVariable(config, decorator.TemplateParameters, out var eVariable))
                {
                    decorator.Variables.TryAdd(varName.ReferenceName, eVariable);
                }
            }

            foreach (var templateParam in decorator.TemplateParameters.Where((e, i) => decorator.TemplateParameters.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(decorator, $"Le nom '{templateParam.Name}' est déjà utilisé.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
            }
        }

        foreach (var container in modelFile.PropertyContainers.Where(c => c.DecoratorReferences.Count > 0))
        {
            container.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in container.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var targetDecorator))
                {
                    isError = true;
                    yield return new ModelError(container, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (container.Decorators.Any(d => d.Decorator == targetDecorator))
                    {
                        isError = true;
                        yield return new ModelError(container, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs de l'objet '{container}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        if (targetDecorator.Implementations.Any(impl => impl.Value.Extends != null && container.AllDecorators.Any(d => d.Implementations.TryGetValue(impl.Key, out var dImpl) && dImpl.Extends != null))
                            || container is Class { Extends: not null } && ((IPropertyContainer)targetDecorator).AllDecorators.Any(d => d.Implementations.Any(impl => impl.Value.Extends != null)))
                        {
                            isError = true;
                            yield return new ModelError(container, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à '{container}' : seul un 'extends' peut être spécifié.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1010 };
                        }

                        var target = targetDecorator.Target;
                        if (target == Target.Endpoint && container is Class || target == Target.Class && container is Endpoint || container is Decorator d && target != null && target != d.Target)
                        {
                            isError = true;
                            yield return new ModelError(container, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à '{container}' : le décorateur ne cible pas le bon type d'objet.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1043 };
                        }

                        foreach (var error in CheckDecoratorParameters(container, decoratorRef, targetDecorator))
                        {
                            yield return error;
                        }

                        if (!isError)
                        {
                            container.Decorators.Add(new(targetDecorator, decoratorRef.ParameterReferences.ToDictionary(pr => pr.Key.ReferenceName, pr => pr.Value.Value)));
                        }
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }
    }

    private static IEnumerable<ModelError> CheckDecoratorParameters(IPropertyContainer container, DecoratorReference decoratorRef, Decorator decorator)
    {
        foreach (var extraParameter in decoratorRef.ParameterReferences.Keys.Where(pr => !decorator.TemplateParameters.Any(tp => tp.Name == pr.ReferenceName)))
        {
            yield return new ModelError(
                container,
                $"Le paramètre '{extraParameter.ReferenceName}' n'existe pas sur le décorateur '{decorator.Name}'.",
                extraParameter)
            { ModelErrorType = ModelErrorType.TMD1035 };
        }

        foreach (var missingParameter in decorator.TemplateParameters.Where(tp => tp.Required && !decoratorRef.ParameterReferences.Any(pr => pr.Key.ReferenceName == tp.Name)))
        {
            yield return new ModelError(
                container,
                $"Le paramètre '{missingParameter.Name}' du décorateur '{decorator.Name}' est obligatoire.",
                decoratorRef)
            { ModelErrorType = ModelErrorType.TMD1036 };
        }
    }
}
