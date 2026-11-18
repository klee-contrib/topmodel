using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public abstract class BaseResolver
{
    protected static IEnumerable<ModelError> CheckParameters(
        object objet,
        ReferenceWithParameters reference,
        IParameterContainer container
    )
    {
        foreach (
            var extraParameter in reference.ParameterReferences.Keys.Where(pr =>
                !container.TemplateParameters.Any(tp => tp.Name == pr.Value)
            )
        )
        {
            yield return ModelError.Error(
                ErrorType.TMD0007,
                [extraParameter.Value, container.Name],
                objet,
                extraParameter
            );
        }

        foreach (
            var missingParameter in container.TemplateParameters.Where(tp =>
                tp.Required && !reference.ParameterReferences.Any(pr => pr.Key.Value == tp.Name)
            )
        )
        {
            yield return ModelError.Error(ErrorType.TMD0008, [missingParameter.Name, container.Name], objet, reference);
        }
    }
}
