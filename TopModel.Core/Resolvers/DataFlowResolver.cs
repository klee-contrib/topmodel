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
)
{
    /// <summary>
    /// Résout les flux de données.
    /// </summary>
    /// <returns>Erreurs.</returns>
    public IEnumerable<ModelError> ResolveDataFlows()
    {
        foreach (var dataFlow in modelFiles.SelectMany(mf => mf.DataFlows))
        {
            if (!referencedClasses.TryGetValue(dataFlow.ClassReference.ReferenceName, out var classe))
            {
                yield return new ModelError(
                    localizer,
                    ErrorType.TMD0002,
                    [dataFlow.ClassReference.ReferenceName],
                    dataFlow,
                    dataFlow.ClassReference
                );
                continue;
            }

            dataFlow.Class = classe;

            if (dataFlow.ActivePropertyReference != null)
            {
                dataFlow.ActiveProperty = classe.ExtendedProperties.FirstOrDefault(fp =>
                    fp.Name == dataFlow.ActivePropertyReference.ReferenceName
                );
                if (dataFlow.ActiveProperty == null)
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0004,
                        [dataFlow.ActivePropertyReference.ReferenceName, classe.Name],
                        dataFlow,
                        dataFlow.ActivePropertyReference
                    );
                }
            }

            dataFlow.DependsOn.Clear();

            foreach (var dependsOnReference in dataFlow.DependsOnReference)
            {
                if (!referencedDataFlows.TryGetValue(dependsOnReference.ReferenceName, out var referencedDataFlow))
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
                if (!referencedClasses.TryGetValue(source.ClassReference.ReferenceName, out var sourceClass))
                {
                    yield return new ModelError(
                        localizer,
                        ErrorType.TMD0002,
                        [source.ClassReference.ReferenceName],
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
                        fp.Name == joinPropertyReference.ReferenceName
                    );
                    if (joinProperty == null)
                    {
                        yield return new ModelError(
                            localizer,
                            ErrorType.TMD0004,
                            [joinPropertyReference.ReferenceName, classe.Name],
                            dataFlow,
                            joinPropertyReference
                        );
                    }

                    source.JoinProperties.Add(joinProperty);
                }
            }
        }
    }
}
