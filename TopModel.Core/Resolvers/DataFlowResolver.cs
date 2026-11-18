using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DataFlowResolver(
    IStringLocalizer localizer,
    IList<ModelFile> modelFiles,
    IDictionary<string, DataFlow> referencedDataFlows,
    IDictionary<string, Class> referencedClasses
) : BaseResolver(localizer)
{
    /// <summary>
    /// Résout les flux de données.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDataFlows()
    {
        foreach (var dataFlow in modelFiles.SelectMany(mf => mf.DataFlows))
        {
            if (!referencedClasses.TryGetValue(dataFlow.ClassReference.Value, out var classe))
            {
                yield return Error(
                    ErrorType.TMD0002,
                    [dataFlow.ClassReference.Value],
                    dataFlow,
                    dataFlow.ClassReference
                );
                continue;
            }

            dataFlow.Class = classe;

            if (dataFlow.ActivePropertyReference != null)
            {
                dataFlow.ActiveProperty = classe.ExtendedProperties.FirstOrDefault(fp =>
                    fp.Name == dataFlow.ActivePropertyReference.Value
                );
                if (dataFlow.ActiveProperty == null)
                {
                    yield return Error(
                        ErrorType.TMD0004,
                        [dataFlow.ActivePropertyReference.Value, classe.Name],
                        dataFlow,
                        dataFlow.ActivePropertyReference
                    );
                }
            }

            dataFlow.DependsOn.Clear();

            foreach (var dependsOnReference in dataFlow.DependsOnReference)
            {
                if (!referencedDataFlows.TryGetValue(dependsOnReference.Value, out var referencedDataFlow))
                {
                    yield return new ModelError(
                        ErrorType.TMD4002,
                        dataFlow,
                        "Le flux de données '{0}' est introuvable dans le fichier ou l'une de ses dépendances.",
                        dependsOnReference
                    );
                    continue;
                }

                dataFlow.DependsOn.Add(referencedDataFlow);
            }

            foreach (var source in dataFlow.Sources)
            {
                if (!referencedClasses.TryGetValue(source.ClassReference.Value, out var sourceClass))
                {
                    yield return Error(
                        ErrorType.TMD0002,
                        [source.ClassReference.Value],
                        dataFlow,
                        source.ClassReference
                    );
                    continue;
                }

                source.Class = sourceClass;
                source.JoinProperties.Clear();

                foreach (var joinPropertyReference in source.JoinPropertyReferences)
                {
                    var joinProperty = sourceClass.ExtendedProperties.FirstOrDefault(fp =>
                        fp.Name == joinPropertyReference.Value
                    );
                    if (joinProperty == null)
                    {
                        yield return Error(
                            ErrorType.TMD0004,
                            [joinPropertyReference.Value, classe.Name],
                            dataFlow,
                            joinPropertyReference
                        );
                    }
                    else
                    {
                        source.JoinProperties.Add(joinProperty);
                    }
                }
            }
        }
    }
}
