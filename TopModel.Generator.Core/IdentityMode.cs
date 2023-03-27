namespace TopModel.Generator.Core;

public enum IdentityMode
{
    /// <summary>
    /// Pas d'identité.
    /// </summary>
    NONE,

    /// <summary>
    /// Identité sous forme de séquence.
    /// </summary>
    SEQUENCE,

    /// <summary>
    /// Identité avec identity de la BDD.
    /// </summary>
    IDENTITY
}
