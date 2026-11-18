using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DecoratorResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
    ModelConfig config,
    IDictionary<string, Decorator> referencedDecorators
) : BaseResolver(localizer)
{
    /// <summary>
    /// Résout les décorateurs sur les classes, les endpoints et les autres décorateurs.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDecorators()
    {
        var decorators = modelFiles.SelectMany(mf => mf.Decorators);
        var sortedDecorators = CoreUtils
            .Sort(decorators, g =>
                g.DecoratorReferences.Select(c => decorators.FirstOrDefault(d => d.Name == c.Value)!)
            )
            .Where(c => c != null);

        foreach (var decorator in sortedDecorators)
        {
            decorator.Variables.Clear();

            foreach (var varName in decorator.VariableReferences)
            {
                Variable? variable = null;

                if (decorator.Target != Target.Endpoint)
                {
                    varName.Value.TryGetClassVariable(config, decorator.TemplateParameters, out variable);
                }

                if (decorator.Target != Target.Class)
                {
                    varName.Value.TryGetEndpointVariable(config, decorator.TemplateParameters, out variable);
                }

                if (variable != null)
                {
                    decorator.Variables.TryAdd(varName.Value, variable);
                }
                else
                {
                    yield return Warning(ErrorType.TMD0011, [varName.Value], decorator, varName);
                }
            }

            foreach (var templateParam in decorator.TemplateParameters.GetDuplicates(p => p.Name))
            {
                yield return Error(ErrorType.TMD0001, [templateParam.Name], decorator, templateParam.Declaration);
            }
        }

        foreach (
            var container in modelFiles
                .SelectMany(mf => mf.PropertyContainers)
                .Where(c => c.DecoratorReferences.Count > 0)
        )
        {
            container.Decorators.Clear();

            var isError = false;
            foreach (var decoratorRef in container.DecoratorReferences)
            {
                if (!referencedDecorators.TryGetValue(decoratorRef.Value, out var targetDecorator))
                {
                    isError = true;
                    yield return Error(ErrorType.TMD0005, [decoratorRef.Value], container, decoratorRef);
                }
                else
                {
                    if (container.Decorators.Any(d => d.Decorator == targetDecorator))
                    {
                        isError = true;
                        yield return new ModelError(
                            ErrorType.TMD5001,
                            container,
                            $"Le décorateur '{decoratorRef.Value}' est déjà présent dans la liste des décorateurs de l'objet '{container}'.",
                            decoratorRef
                        );
                    }
                    else
                    {
                        if (
                            targetDecorator.Implementations.Any(impl =>
                                impl.Value.Extends != null
                                && container.AllDecorators.Any(d =>
                                    d.Implementations.TryGetValue(impl.Key, out var dImpl) && dImpl.Extends != null
                                )
                            )
                            || container is Class { Extends: not null }
                                && ((IPropertyContainer)targetDecorator).AllDecorators.Any(d =>
                                    d.Implementations.Any(impl => impl.Value.Extends != null)
                                )
                        )
                        {
                            isError = true;
                            yield return new ModelError(
                                ErrorType.TMD5002,
                                container,
                                $"Impossible d'appliquer le décorateur '{decoratorRef.Value}' à '{container}' : seul un 'extends' peut être spécifié.",
                                decoratorRef
                            );
                        }

                        var target = targetDecorator.Target;
                        if (
                            target == Target.Endpoint && container is Class
                            || target == Target.Class && container is Endpoint
                            || container is Decorator d && target != null && target != d.Target
                        )
                        {
                            isError = true;
                            yield return new ModelError(
                                ErrorType.TMD5003,
                                container,
                                $"Impossible d'appliquer le décorateur '{decoratorRef.Value}' à '{container}' : le décorateur ne cible pas le bon type d'objet.",
                                decoratorRef
                            );
                        }

                        foreach (var error in CheckParameters(container, decoratorRef, targetDecorator))
                        {
                            yield return error;
                        }

                        if (!isError)
                        {
                            container.Decorators.Add(
                                new(
                                    targetDecorator,
                                    decoratorRef.ParameterReferences.ToDictionary(
                                        pr => pr.Key.Value,
                                        pr => pr.Value.Value
                                    )
                                )
                            );
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
}
