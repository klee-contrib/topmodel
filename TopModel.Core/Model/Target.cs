using System.Runtime.Serialization;

namespace TopModel.Core.Model;

public enum Target
{
    /// <summary>
    /// Classe.
    /// </summary>
    [EnumMember(Value = "class")]
    Class,

    /// <summary>
    /// Endpoint.
    /// </summary>
    [EnumMember(Value = "endpoint")]
    Endpoint,

    /// <summary>
    /// Propriété.
    /// </summary>
    [EnumMember(Value = "property")]
    Property,

    /// <summary>
    /// Propriété standard.
    /// </summary>
    [EnumMember(Value = "regular-property")]
    RegularProperty,

    /// <summary>
    /// Propriété d'association.
    /// </summary>
    [EnumMember(Value = "association-property")]
    AssociationProperty,

    /// <summary>
    /// Propriété de composition.
    /// </summary>
    [EnumMember(Value = "composition-property")]
    CompositionProperty
}
