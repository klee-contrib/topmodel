namespace TopModel.Core;

public enum ModelErrorType
{
    /// <summary>
    /// Code d'erreur par défaut
    /// </summary>
    TMD_0000,
    /// <summary>
    /// L'import {} n'est pas utilisé
    /// </summary>
    TMD_0001,
    /// <summary>
    /// La classe doit avoir une seule clé primaire
    /// </summary>
    TMD_0002,
    /// <summary>
    /// L'import '{use.ReferenceName}' ne doit être spécifié qu'une seule fois
    /// </summary>
    TMD_0003,
    /// <summary>
    /// La classe '{0}' doit avoir une (et une seule) clé primaire pour être référencée dans une association.
    /// </summary>
    TMD_1001,
    /// <summary>
    /// La classe est introuvable dans le fichier
    /// </summary>
    TMD_1002,
    /// <summary>
    /// Le fichier est introuvable dans les dépendances du fichier
    /// </summary>
    TMD_1003,
    /// <summary>
    /// La propriété '{{0}}' est introuvable sur la classe '{aliasedClass}'
    /// </summary>
    TMD_1004,
    /// <summary>
    /// Le domaine '{0}' est introuvable.
    /// </summary>
    TMD_1005,
    /// <summary>
    /// L'endpoint est introuvable dans le fichier
    /// </summary>
    TMD_1006,
    /// <summary>
    /// Le fichier référencé '{use.ReferenceName}' est introuvable
    /// </summary>
    TMD_1007
}
