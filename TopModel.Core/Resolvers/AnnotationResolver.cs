using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public class AnnotationResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
    ModelConfig config,
    IDictionary<string, Annotation> referencedAnnotations
) : BaseResolver(localizer)
{
    /// <summary>
    /// Résout les annotations.
    /// </summary>
    /// <returns>Annotations.</returns>
    public IEnumerable<ModelError> ResolveAnnotations()
    {
        foreach (var annotation in modelFiles.SelectMany(mf => mf.Annotations))
        {
            annotation.Variables.Clear();

            foreach (var varName in annotation.VariableReferences)
            {
                Variable? variable = null;

                if (
                    annotation.Target.Count == 0
                    || annotation.Target.Contains(Target.Property)
                    || annotation.Target.Contains(Target.AssociationProperty)
                    || annotation.Target.Contains(Target.CompositionProperty)
                    || annotation.Target.Contains(Target.RegularProperty)
                )
                {
                    varName.ReferenceName.TryGetPropertyVariable(config, annotation.TemplateParameters, out variable);
                }

                if (annotation.Target.Count == 0 || annotation.Target.Contains(Target.Class))
                {
                    varName.ReferenceName.TryGetClassVariable(config, annotation.TemplateParameters, out variable);
                }

                if (
                    annotation.Target.Count == 0
                    || annotation.Target.Contains(Target.Endpoint)
                    || annotation.Target.Contains(Target.ClientEndpoint)
                    || annotation.Target.Contains(Target.ServerEndpoint)
                )
                {
                    varName.ReferenceName.TryGetEndpointVariable(config, annotation.TemplateParameters, out variable);
                }

                if (variable != null)
                {
                    annotation.Variables.TryAdd(varName.ReferenceName, variable);
                }
                else
                {
                    yield return Warning(ErrorType.TMD0011, [varName.ReferenceName], annotation, varName);
                }
            }

            foreach (var templateParam in annotation.TemplateParameters.GetDuplicates(p => p.Name))
            {
                yield return Error(ErrorType.TMD0001, [templateParam.Name], annotation, templateParam.GetLocation());
            }

            if (annotation.TemplateParameters.Any() && annotation.Global)
            {
                yield return new ModelError(
                    ErrorType.TMD2005,
                    annotation,
                    "Une annotation globale ne peut pas définir de paramètres."
                );
            }
        }

        foreach (
            var container in modelFiles
                .SelectMany<ModelFile, IAnnotationContainer>(mf =>
                    [.. mf.Domains, .. mf.Decorators, .. mf.Classes, .. mf.Endpoints, .. mf.OwnProperties]
                )
                .OrderBy(ac => ac is Domain or Decorator ? 0 : 1)
        )
        {
            var isError = false;

            var annotationsToResolve = container is AliasProperty alp ? alp.OwnAnnotations : container.Annotations;
            annotationsToResolve.Clear();
            foreach (
                var error in ResolveAnnotationReferences(
                    container,
                    container.AnnotationReferences,
                    annotationsToResolve
                )
            )
            {
                isError = true;
                yield return error;
            }

            container.ExcludedAnnotations.Clear();
            foreach (
                var error in ResolveAnnotationReferences(
                    container,
                    container.ExcludedAnnotationReferences,
                    container.ExcludedAnnotations
                )
            )
            {
                isError = true;
                yield return error;
            }

            var globalExclusions = container.ExcludedAnnotations.Select(a => a.Annotation).ToList();

            void AddDecoratorExclusions(Decorator decorator)
            {
                globalExclusions.AddRange(decorator.ExcludedAnnotations.Select(a => a.Annotation));
                foreach (var subD in decorator.Decorators)
                {
                    AddDecoratorExclusions(subD.Decorator);
                }
            }

            switch (container)
            {
                case IProperty { Domain.ExcludedAnnotations: var dea }:
                    globalExclusions.AddRange(dea.Select(a => a.Annotation));
                    break;
                case Class c:
                    foreach (var d in c.Decorators)
                    {
                        AddDecoratorExclusions(d.Decorator);
                    }
                    break;
                case Endpoint e:
                    foreach (var d in e.Decorators)
                    {
                        AddDecoratorExclusions(d.Decorator);
                    }
                    break;
            }

            foreach (var annotation in referencedAnnotations.Values.Where(a => a.Global))
            {
                if (
                    !globalExclusions.Contains(annotation)
                    && !annotationsToResolve.Any(a => a.Annotation == annotation)
                    && (
                        annotation.Target.Count == 0
                            && container is not Decorator
                            && container is not Domain
                            && container is not AliasProperty
                        || container is Class && annotation.Target.Contains(Target.Class)
                        || container is Endpoint
                            && (
                                annotation.Target.Contains(Target.Endpoint)
                                || annotation.Target.Contains(Target.ClientEndpoint)
                                || annotation.Target.Contains(Target.ServerEndpoint)
                            )
                        || container is AssociationProperty or ReverseAssociationDefinition
                            && (
                                annotation.Target.Contains(Target.Property)
                                || annotation.Target.Contains(Target.AssociationProperty)
                            )
                        || container is CompositionProperty
                            && (
                                annotation.Target.Contains(Target.Property)
                                || annotation.Target.Contains(Target.CompositionProperty)
                            )
                        || container is RegularProperty
                            && (
                                annotation.Target.Contains(Target.Property)
                                || annotation.Target.Contains(Target.RegularProperty)
                            )
                    )
                )
                {
                    annotationsToResolve.Add(new(annotation, new Dictionary<string, string>()));
                }
            }

            if (container is IPropertyContainer pContainer)
            {
                pContainer.PropertyAnnotations.Clear();

                foreach (
                    var error in ResolveAnnotationReferences(
                        pContainer,
                        pContainer.PropertyAnnotationReferences,
                        pContainer.PropertyAnnotations,
                        isProperty: true
                    )
                )
                {
                    isError = true;
                    yield return error;
                }
            }

            if (isError)
            {
                continue;
            }
        }
    }

    private IEnumerable<ModelError> ResolveAnnotationReferences(
        IAnnotationContainer container,
        IEnumerable<AnnotationReference> annotationReferences,
        IList<AnnotationInstance> annotationsToResolve,
        bool isProperty = false
    )
    {
        var isError = false;

        foreach (var annotationRef in annotationReferences)
        {
            if (!referencedAnnotations.TryGetValue(annotationRef.ReferenceName, out var annotation))
            {
                isError = true;
                yield return new ModelError(
                    ErrorType.TMD2001,
                    container,
                    $"L'annotation '{annotationRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.",
                    annotationRef
                );
            }
            else
            {
                if (annotationsToResolve.Any(d => d.Annotation == annotation))
                {
                    isError = true;
                    yield return new ModelError(
                        ErrorType.TMD2002,
                        container,
                        $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations de l'objet.",
                        annotationRef
                    );
                }
                else
                {
                    if (
                        annotation.Target.Any()
                        && (
                            container is Class or Decorator { Target: Target.Class }
                                && !annotation.Target.Contains(Target.Class)
                                && !isProperty
                            || container is Endpoint or Decorator { Target: Target.Endpoint }
                                && !annotation.Target.Contains(Target.Endpoint)
                                && !annotation.Target.Contains(Target.ClientEndpoint)
                                && !annotation.Target.Contains(Target.ServerEndpoint)
                                && !isProperty
                            || (container is Domain or IProperty or ReverseAssociationDefinition || isProperty)
                                && !annotation.Target.Contains(Target.Property)
                                && !annotation.Target.Contains(Target.AssociationProperty)
                                && !annotation.Target.Contains(Target.CompositionProperty)
                                && !annotation.Target.Contains(Target.RegularProperty)
                        )
                    )
                    {
                        isError = true;
                        yield return new ModelError(
                            ErrorType.TMD2004,
                            container,
                            $"Impossible d'appliquer l'annotation '{annotationRef.ReferenceName}' à '{container}' : l'annotation ne cible pas le bon type d'objet.",
                            annotationRef
                        );
                    }

                    foreach (var error in CheckParameters(container, annotationRef, annotation))
                    {
                        yield return error;
                    }

                    if (!isError)
                    {
                        annotationsToResolve.Add(
                            new(
                                annotation,
                                annotationRef.ParameterReferences.ToDictionary(
                                    pr => pr.Key.ReferenceName,
                                    pr => pr.Value.Value
                                )
                            )
                        );
                    }
                }
            }
        }
    }
}
