using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Core.Resolvers;

internal class DataFlowResolver(
    IList<ModelFile> modelFiles,
    IDictionary<string, DataFlow> referencedDataFlows,
    IDictionary<string, Class> referencedClasses
) : BaseResolver
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
                yield return ModelError.Error(
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
                    yield return ModelError.Error(
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
                    yield return ModelError.Error(
                        ErrorType.TMD4002,
                        [dependsOnReference.Value],
                        dataFlow,
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
                    yield return ModelError.Error(
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
                        yield return ModelError.Error(
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
