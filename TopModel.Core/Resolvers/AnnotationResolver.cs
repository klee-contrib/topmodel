using TopModel.Core.FileModel;
using TopModel.Core.Model.Implementation;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public class AnnotationResolver(ModelFile modelFile, ModelConfig config, IDictionary<string, Annotation> referencedAnnotations)
{
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

        foreach (var domain in modelFile.Domains.Where(c => c.AnnotationReferences.Count > 0))
        {
            domain.Annotations.Clear();

            var isError = false;
            foreach (var annotationRef in domain.AnnotationReferences)
            {
                if (!referencedAnnotations.TryGetValue(annotationRef.ReferenceName, out var annotation))
                {
                    isError = true;
                    yield return new ModelError(domain, $"L'annotation '{annotationRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", annotationRef) { ModelErrorType = ModelErrorType.TMD1040 };
                }
                else
                {
                    if (domain.Annotations.Any(d => d.Annotation == annotation))
                    {
                        isError = true;
                        yield return new ModelError(domain, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine '{domain}'.", annotationRef) { ModelErrorType = ModelErrorType.TMD1041 };
                    }
                    else
                    {
                        foreach (var error in CheckAnnotationParameters(domain, annotationRef, annotation))
                        {
                            yield return error;
                        }

                        domain.Annotations.Add((annotation, annotationRef.ParameterReferences.Select(p => new StringWithVariables(p)).ToArray()));
                    }
                }
            }

            if (isError)
            {
                continue;
            }
        }

        foreach (var decorator in modelFile.Decorators.Where(c => c.AnnotationReferences.Count > 0))
        {
            decorator.Annotations.Clear();

            var isError = false;
            foreach (var annotationRef in decorator.AnnotationReferences)
            {
                if (!referencedAnnotations.TryGetValue(annotationRef.ReferenceName, out var annotation))
                {
                    isError = true;
                    yield return new ModelError(decorator, $"L'annotation '{annotationRef.ReferenceName}' est introuvable dans le fichier ou l'une de ses dépendances.", annotationRef) { ModelErrorType = ModelErrorType.TMD1040 };
                }
                else
                {
                    if (decorator.Annotations.Any(d => d.Annotation == annotation))
                    {
                        isError = true;
                        yield return new ModelError(decorator, $"L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine '{decorator}'.", annotationRef) { ModelErrorType = ModelErrorType.TMD1041 };
                    }
                    else
                    {
                        foreach (var error in CheckAnnotationParameters(decorator, annotationRef, annotation))
                        {
                            yield return error;
                        }

                        decorator.Annotations.Add((annotation, annotationRef.ParameterReferences.Select(p => new StringWithVariables(p)).ToArray()));
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
