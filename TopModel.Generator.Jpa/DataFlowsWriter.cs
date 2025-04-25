namespace TopModel.Generator.Jpa;

public enum DataFlowsWriter
{
    /// <summary>
    /// Génération du modèle utilisant spring-batch-bulk
    /// </summary>
    Bulk,

    /// <summary>
    /// Génération du modèle utilisant jpa-writer
    /// </summary>
    Jpa
}