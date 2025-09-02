using System.Runtime.Serialization;

namespace TopModel.Core.Model.Implementation;

public enum AnnotationConstraint
{
    /// <summary>
    /// Objet non persisté.
    /// </summary>
    [EnumMember(Value = "non-persisted")]
    NonPersisted,

    /// <summary>
    /// Objet persisté.
    /// </summary>
    [EnumMember(Value = "persisted")]
    Persisted,

    /// <summary>
    /// Propriété de classe.
    /// </summary>
    [EnumMember(Value = "class-property")]
    ClassProperty,

    /// <summary>
    /// Paramètre d'endpoint.
    /// </summary>
    [EnumMember(Value = "endpoint-param")]
    EndpointParam,

    /// <summary>
    /// Clé primaire.
    /// </summary>
    [EnumMember(Value = "primary-key")]
    PrimaryKey,
}
