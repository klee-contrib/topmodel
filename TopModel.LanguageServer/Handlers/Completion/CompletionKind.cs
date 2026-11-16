namespace TopModel.LanguageServer.Handlers;

/// <summary>
/// Type de complétion à proposer à une position donnée du document.
/// </summary>
public enum CompletionKind
{
    /// <summary>Aucune complétion.</summary>
    None,

    /// <summary>Domaines.</summary>
    Domain,

    /// <summary>Classes.</summary>
    Class,

    /// <summary>Endpoints.</summary>
    Endpoint,

    /// <summary>Tags.</summary>
    Tag,

    /// <summary>Fichiers (section "uses").</summary>
    File,

    /// <summary>Décorateurs.</summary>
    Decorator,

    /// <summary>Annotations.</summary>
    Annotation,

    /// <summary>Flux de données.</summary>
    DataFlow,

    /// <summary>Propriétés (analyse propre à CompleteProperty).</summary>
    Property,
}
