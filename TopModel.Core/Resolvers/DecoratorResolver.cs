using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DecoratorResolver(ModelFile modelFile, IDictionary<string, Decorator> referencedDecorators)
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
            foreach (var templateParam in decorator.TemplateParameters.Where((e, i) => decorator.TemplateParameters.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(decorator, $"Le nom '{templateParam.Name}' est déjà utilisé.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
            }

            foreach (var templateParam in decorator.TemplateParameters.Where(p => !p.Required))
            {
                var index = decorator.TemplateParameters.IndexOf(templateParam);
                if (decorator.TemplateParameters.Any(param => param.Required && decorator.TemplateParameters.IndexOf(param) > index))
                {
                    yield return new ModelError(decorator, $"Le paramètre facultatif '{templateParam.Name}' doit être positionné après tous les paramètres obligatoires.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD1037 };
                }
            }
        }

        foreach (var decorator in modelFile.Decorators.Where(c => c.DecoratorReferences.Count > 0))
        {
            decorator.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in decorator.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var targetDecorator))
                {
                    isError = true;
                    yield return new ModelError(decorator, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (decorator.Decorators.Any(d => d.Decorator == targetDecorator))
                    {
                        isError = true;
                        yield return new ModelError(decorator, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs du décorateur '{decorator}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        if (targetDecorator.Implementations.Any(impl => impl.Value.Extends != null && decorator.Implementations.TryGetValue(impl.Key, out var dImpl) && dImpl.Extends != null))
                        {
                            isError = true;
                            yield return new ModelError(decorator, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' au décorateur '{decorator}' : seul un 'extends' peut être spécifié.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1010 };
                        }

                        foreach (var error in CheckDecoratorParameters(decorator, decoratorRef, targetDecorator))
                        {
                            yield return error;
                        }

                        decorator.Decorators.Add((targetDecorator, decoratorRef.ParameterReferences.Select(p => p.ReferenceName).ToArray()));
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }

        foreach (var classe in modelFile.Classes.Where(c => c.DecoratorReferences.Count > 0))
        {
            classe.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in classe.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var decorator))
                {
                    isError = true;
                    yield return new ModelError(classe, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (classe.Decorators.Any(d => d.Decorator == decorator))
                    {
                        isError = true;
                        yield return new ModelError(classe, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs de la classe '{classe}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        if (decorator.Implementations.Concat(decorator.Decorators.SelectMany(d => d.Decorator.Implementations)).Any(impl => impl.Value.Extends != null && (classe.Extends != null || classe.Decorators.Any(d => d.Decorator.Implementations.TryGetValue(impl.Key, out var dImpl) && dImpl.Extends != null))))
                        {
                            isError = true;
                            yield return new ModelError(classe, $"Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à la classe '{classe}' : seul un 'extends' peut être spécifié.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1010 };
                        }

                        foreach (var error in CheckDecoratorParameters(classe, decoratorRef, decorator))
                        {
                            yield return error;
                        }

                        classe.Decorators.Add((decorator, decoratorRef.ParameterReferences.Select(p => p.ReferenceName).ToArray()));
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }

        foreach (var endpoint in modelFile.Endpoints.Where(c => c.DecoratorReferences.Count > 0))
        {
            endpoint.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in endpoint.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.ReferenceName, out var decorator))
                {
                    isError = true;
                    yield return new ModelError(endpoint, $"Le décorateur '{decoratorRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1008 };
                }
                else
                {
                    if (endpoint.Decorators.Any(d => d.Decorator == decorator))
                    {
                        isError = true;
                        yield return new ModelError(endpoint, $"Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs du endpoint '{endpoint}'.", decoratorRef) { ModelErrorType = ModelErrorType.TMD1009 };
                    }
                    else
                    {
                        foreach (var error in CheckDecoratorParameters(endpoint, decoratorRef, decorator))
                        {
                            yield return error;
                        }

                        endpoint.Decorators.Add((decorator, decoratorRef.ParameterReferences.Select(p => p.ReferenceName).ToArray()));
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
        if (decoratorRef.ParameterReferences.Count > decorator.TemplateParameters.Count)
        {
            foreach (var extraParameter in decoratorRef.ParameterReferences.Skip(decorator.TemplateParameters.Count))
            {
                yield return new ModelError(
                    container,
                    decorator.TemplateParameters.Count > 1 ? $"Le décorateur '{decorator.Name}' ne définit que {decorator.TemplateParameters.Count} paramètres." : $"Le décorateur '{decorator.Name}' ne définit qu'un seul paramètre.",
                    extraParameter)
                { ModelErrorType = ModelErrorType.TMD1035 };
            }
        }

        if (decoratorRef.ParameterReferences.Count < decorator.TemplateParameters.Count(p => p.Required))
        {
            var parametres = decorator.TemplateParameters.Skip(decoratorRef.ParameterReferences.Count).Where(p => p.Required).Select(p => $"'{p.Name}'");
            yield return new ModelError(
                container,
                parametres.Count() > 1 ? $"Les paramètres {string.Join(", ", parametres)} du décorateur '{decorator.Name}' sont obligatoires." : $"Le paramètre {string.Join(", ", parametres)} du décorateur '{decorator.Name}' est obligatoire.",
                decoratorRef)
            { ModelErrorType = ModelErrorType.TMD1036 };
        }
    }
}
