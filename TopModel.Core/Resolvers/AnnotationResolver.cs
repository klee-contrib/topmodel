using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public class AnnotationResolver(
    IList<ModelFile> modelFiles,
    ModelConfig config,
    IDictionary<string, Annotation> referencedAnnotations
)
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

                if (annotation.Target.Count == 0 || annotation.Target.Contains(Target.Endpoint))
                {
                    varName.ReferenceName.TryGetEndpointVariable(config, annotation.TemplateParameters, out variable);
                }

                if (variable != null)
                {
                    annotation.Variables.TryAdd(varName.ReferenceName, variable);
                }
                else
                {
                    yield return new ModelError(
                        ErrorType.TMD0011,
                        annotation,
                        $"La variable '{varName.ReferenceName}' est introuvable.",
                        varName,
                        isError: false
                    );
                }
            }

            foreach (
                var templateParam in annotation.TemplateParameters.Where(
                    (e, i) => annotation.TemplateParameters.Where((p, j) => p.Name == e.Name && j < i).Any()
                )
            )
            {
                yield return new ModelError(
                    ErrorType.TMD0001,
                    annotation,
                    $"Le nom '{templateParam.Name}' est déjà utilisé.",
                    templateParam.GetLocation()
                );
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
                .SelectMany(mf => mf.AnnotationContainers)
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
                        || container is Endpoint && annotation.Target.Contains(Target.Endpoint)
                        || container is AssociationProperty
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

    private static IEnumerable<ModelError> CheckAnnotationParameters(
        object target,
        AnnotationReference annotationRef,
        Annotation annotation
    )
    {
        foreach (
            var extraParameter in annotationRef.ParameterReferences.Keys.Where(pr =>
                !annotation.TemplateParameters.Any(tp => tp.Name == pr.ReferenceName)
            )
        )
        {
            yield return new ModelError(
                ErrorType.TMD0007,
                target,
                $"Le paramètre '{extraParameter.ReferenceName}' n'existe pas sur l'annotation '{annotation.Name}'.",
                extraParameter
            );
        }

        foreach (
            var missingParameter in annotation.TemplateParameters.Where(tp =>
                tp.Required && !annotationRef.ParameterReferences.Any(pr => pr.Key.ReferenceName == tp.Name)
            )
        )
        {
            yield return new ModelError(
                ErrorType.TMD0008,
                target,
                $"Le paramètre '{missingParameter.Name}' de l'annotation '{annotation.Name}' est obligatoire.",
                annotationRef
            );
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
                            container is Class && !annotation.Target.Contains(Target.Class) && !isProperty
                            || container is Endpoint && !annotation.Target.Contains(Target.Endpoint) && !isProperty
                            || container is Decorator { Target: Target dt }
                                && !annotation.Target.Contains(dt)
                                && !isProperty
                            || (container is Domain or IProperty || isProperty)
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

                    foreach (var error in CheckAnnotationParameters(container, annotationRef, annotation))
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
