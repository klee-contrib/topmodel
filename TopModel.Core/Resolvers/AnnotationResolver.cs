using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public class AnnotationResolver(ModelFile modelFile, ModelConfig config, IDictionary<string, Annotation> referencedAnnotations)
{
    public IEnumerable<ModelError> CheckAliasAnnotations()
    {
        foreach (var alp in modelFile.Properties.OfType<AliasProperty>())
        {
            foreach (var g in alp.Annotations.GroupBy(a => a.Annotation.Name).Where(g => g.Count() > 1))
            {
                var annotationRef = alp.AnnotationReferences.FirstOrDefault(ar => ar.ReferenceName == g.Key);
                if (annotationRef != null)
                {
                    yield return new ModelError(alp, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations de la propriété aliasée.", annotationRef) { ModelErrorType = ModelErrorType.TMD1042 };
                }
            }

            foreach (var (annotation, _) in (alp.Domain?.Annotations ?? []).Intersect(alp.Annotations))
            {
                var annotationRef = alp.AnnotationReferences.FirstOrDefault(ar => ar.ReferenceName == annotation.Name);
                if (annotationRef != null)
                {
                    yield return new ModelError(alp, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine de la propriété '{alp}'.", annotationRef) { ModelErrorType = ModelErrorType.TMD1042 };
                }
            }
        }
    }

    /// <summary>
    /// Résout les annotations.
    /// </summary>
    /// <returns>Annotations.</returns>
    public IEnumerable<ModelError> ResolveAnnotations()
    {
        foreach (var annotation in modelFile.Annotations)
        {
            annotation.Variables.Clear();

            foreach (var varName in annotation.VariableReferences)
            {
                if (varName.ReferenceName.TryGetPropertyVariable(config, annotation.TemplateParameters, out var variable))
                {
                    annotation.Variables.TryAdd(varName.ReferenceName, variable);
                }
            }

            foreach (var templateParam in annotation.TemplateParameters.Where((e, i) => annotation.TemplateParameters.Where((p, j) => p.Name == e.Name && j < i).Any()))
            {
                yield return new ModelError(annotation, $"Le nom '{templateParam.Name}' est déjà utilisé.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD0003 };
            }

            foreach (var templateParam in annotation.TemplateParameters.Where(p => !p.Required))
            {
                var index = annotation.TemplateParameters.IndexOf(templateParam);
                if (annotation.TemplateParameters.Any(param => param.Required && annotation.TemplateParameters.IndexOf(param) > index))
                {
                    yield return new ModelError(annotation, $"Le paramètre facultatif '{templateParam.Name}' doit être positionné après tous les paramètres obligatoires.", templateParam.GetLocation()) { ModelErrorType = ModelErrorType.TMD1037 };
                }
            }
        }

        foreach (var container in modelFile.AnnotationContainers.Where(c => c.AnnotationReferences.Count > 0))
        {
            var annotationsToResolve = container is AliasProperty alp ? alp.OwnAnnotations : container.Annotations;

            annotationsToResolve.Clear();

            var isError = false;
            foreach (var annotationRef in container.AnnotationReferences)
            {
                if (!referencedAnnotations.TryGetValue(annotationRef.ReferenceName, out var annotation))
                {
                    isError = true;
                    yield return new ModelError(container, $"L'annotation '{annotationRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", annotationRef) { ModelErrorType = ModelErrorType.TMD1040 };
                }
                else
                {
                    if (annotationsToResolve.Any(d => d.Annotation == annotation))
                    {
                        isError = true;
                        yield return new ModelError(container, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations de l'objet.", annotationRef) { ModelErrorType = ModelErrorType.TMD1041 };
                    }
                    else
                    {
                        if (container is IPropertyContainer propertyContainer && propertyContainer.AllDecorators.Any(d => d.Annotations.Any(a => a.Annotation == annotation)))
                        {
                            isError = true;
                            yield return new ModelError(propertyContainer, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations d'un des décorateurs de l'objet '{propertyContainer}'.", annotationRef) { ModelErrorType = ModelErrorType.TMD1042 };
                        }

                        if (container is IProperty property && property is not AliasProperty && (property.Domain?.Annotations.Any(d => d.Annotation == annotation) ?? false))
                        {
                            isError = true;
                            yield return new ModelError(property, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine de la propriété '{property}'.", annotationRef) { ModelErrorType = ModelErrorType.TMD1042 };
                        }

                        if (annotation.Target.Any())
                        {
                            if (
                                container is Class && !annotation.Target.Contains(Target.Class)
                                || container is Endpoint && !annotation.Target.Contains(Target.Endpoint)
                                || container is Decorator { Target: Target dt } && !annotation.Target.Contains(dt)
                                || container is Domain or IProperty && !annotation.Target.Contains(Target.Property) && !annotation.Target.Contains(Target.AssociationProperty) && !annotation.Target.Contains(Target.CompositionProperty) && !annotation.Target.Contains(Target.RegularProperty))
                            {
                                isError = true;
                                yield return new ModelError(container, $"Impossible d'appliquer l'annotation '{annotationRef.ReferenceName}' à '{container}' : l'annotation ne cible pas le bon type d'objet.", annotationRef) { ModelErrorType = ModelErrorType.TMD1044 };
                            }
                        }

                        foreach (var error in CheckAnnotationParameters(container, annotationRef, annotation))
                        {
                            yield return error;
                        }

                        if (!isError)
                        {
                            annotationsToResolve.Add(new(annotation, annotationRef.ParameterReferences.ToDictionary(pr => pr.Key.ReferenceName, pr => pr.Value.Value)));
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

    private static IEnumerable<ModelError> CheckAnnotationParameters(object target, AnnotationReference annotationRef, Annotation annotation)
    {
        foreach (var extraParameter in annotationRef.ParameterReferences.Keys.Where(pr => !annotation.TemplateParameters.Any(tp => tp.Name == pr.ReferenceName)))
        {
            yield return new ModelError(
                target,
                $"Le paramètre '{extraParameter.ReferenceName}' n'existe pas sur l'annotation '{annotation.Name}'.",
                extraParameter)
            { ModelErrorType = ModelErrorType.TMD1035 };
        }

        foreach (var missingParameter in annotation.TemplateParameters.Where(tp => tp.Required && !annotationRef.ParameterReferences.Any(pr => pr.Key.ReferenceName == tp.Name)))
        {
            yield return new ModelError(
                target,
                $"Le paramètre '{missingParameter.Name}' de l'annotation '{annotation.Name}' est obligatoire.",
                annotationRef)
            { ModelErrorType = ModelErrorType.TMD1036 };
        }
    }
}
