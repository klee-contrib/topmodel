using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

public abstract class BaseResolver(IStringLocalizer localizer)
{
    protected IEnumerable<ModelError> CheckParameters(
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
            yield return Error(ErrorType.TMD0007, [extraParameter.Value, container.Name], objet, extraParameter);
        }

        foreach (
            var missingParameter in container.TemplateParameters.Where(tp =>
                tp.Required && !reference.ParameterReferences.Any(pr => pr.Key.Value == tp.Name)
            )
        )
        {
            yield return Error(ErrorType.TMD0008, [missingParameter.Name, container.Name], objet, reference);
        }
    }

    protected ModelError Error(ErrorType errorType, string[] messageArgs, object objet, Location? location = null)
    {
        return new ModelError(localizer, errorType, messageArgs, objet, location, isError: true);
    }

    protected ModelError Warning(ErrorType errorType, string[] messageArgs, object objet, Location? location = null)
    {
        return new ModelError(localizer, errorType, messageArgs, objet, location, isError: false);
    }
}
