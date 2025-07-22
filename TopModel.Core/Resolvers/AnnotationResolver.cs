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

            foreach (var annotation in (alp.Domain?.Annotations ?? []).Intersect(alp.Annotations))
            {
                var annotationRef = alp.AnnotationReferences.FirstOrDefault(ar => ar.ReferenceName == annotation.Annotation.Name);
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

                        foreach (var error in CheckAnnotationParameters(container, annotationRef, annotation))
                        {
                            yield return error;
                        }

                        annotationsToResolve.Add((annotation, annotationRef.ParameterReferences.Select(p => new StringWithVariables(p)).ToArray()));
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
        if (annotationRef.ParameterReferences.Count > annotation.TemplateParameters.Count)
        {
            foreach (var extraParameter in annotationRef.ParameterReferences.Skip(annotation.TemplateParameters.Count))
            {
                yield return new ModelError(
                    target,
                    annotation.TemplateParameters.Count > 1 ? $"L'annotation '{annotation.Name}' ne définit que {annotation.TemplateParameters.Count} paramètres." : $"L'annotation '{annotation.Name}' ne définit qu'un seul paramètre.",
                    extraParameter)
                { ModelErrorType = ModelErrorType.TMD1035 };
            }
        }

        if (annotationRef.ParameterReferences.Count < annotation.TemplateParameters.Count(p => p.Required))
        {
            var parametres = annotation.TemplateParameters.Skip(annotationRef.ParameterReferences.Count).Where(p => p.Required).Select(p => $"'{p.Name}'");
            yield return new ModelError(
                target,
                parametres.Count() > 1 ? $"Les paramètres {string.Join(", ", parametres)} de l'annotation '{annotation.Name}' sont obligatoires." : $"Le paramètre {string.Join(", ", parametres)} de l'annotation '{annotation.Name}' est obligatoire.",
                annotationRef)
            { ModelErrorType = ModelErrorType.TMD1036 };
        }
    }
}
