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

    /// <summary>Paramètres d'un domaine (clés de "parameters" sous "domain: { name, parameters }").</summary>
    DomainParameter,

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

    /// <summary>Paramètres d'un décorateur (clés sous une instance de décorateur).</summary>
    DecoratorParameter,

    /// <summary>Annotations.</summary>
    Annotation,

    /// <summary>Paramètres d'une annotation (clés sous une instance d'annotation).</summary>
    AnnotationParameter,

    /// <summary>Flux de données.</summary>
    DataFlow,

    /// <summary>Propriétés d'une autre classe (alias, association, flux de données, valeur d'un mapping...).</summary>
    TargetProperty,

    /// <summary>Propriétés de la classe courante (defaultProperty, unique, indexes, values, clés de mappings...).</summary>
    OwnProperty,
}
