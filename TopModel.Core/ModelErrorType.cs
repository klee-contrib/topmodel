namespace TopModel.Core;

public enum ModelErrorType
{
    /// <summary>
    /// Code d'erreur par défaut
    /// </summary>
    TMD0000,

    /// <summary>
    /// La classe doit avoir une seule clé primaire
    /// </summary>
    TMD0001,

    /// <summary>
    /// L'import '{use.ReferenceName}' ne doit être spécifié qu'une seule fois
    /// </summary>
    TMD0002,

    /// <summary>
    /// Le nom '{endpoint.Name}' est déjà utilisé.
    /// </summary>
    TMD0003,

    /// <summary>
    /// La propriété '{propertyReference.Name}' est déjà référencée dans la définition de l'alias.
    /// </summary>
    TMD0004,

    /// <summary>
    /// La classe '{0}' doit avoir une (et une seule) clé primaire pour être référencée dans une association.
    /// </summary>
    TMD1001,

    /// <summary>
    /// La classe est introuvable dans le fichier
    /// </summary>
    TMD1002,

    /// <summary>
    /// Le fichier est introuvable dans les dépendances du fichier
    /// </summary>
    TMD1003,

    /// <summary>
    /// La propriété '{{0}}' est introuvable sur la classe '{aliasedClass}'
    /// </summary>
    TMD1004,

    /// <summary>
    /// Le domaine '{0}' est introuvable.
    /// </summary>
    TMD1005,

    /// <summary>
    /// L'endpoint est introuvable dans le fichier
    /// </summary>
    TMD1006,

    /// <summary>
    /// Le fichier référencé '{use.ReferenceName}' est introuvable
    /// </summary>
    TMD1007,

    /// <summary>
    /// L'import {} n'est pas utilisé.
    /// </summary>
    TMD9001,

    /// <summary>
    /// Le trigram '{classe.Trigram}' est déjà utilisé.
    /// </summary>
    TMD9002,

    /// <summary>
    /// Le paramètre de requête '{queryParam.GetParamName()}' doit suivre tous les paramètres de route ou de body dans un endpoint.
    /// </summary>
    TMD9003,

    /// <summary>
    /// Le domaine '{domain.Name}' n'est pas utilisé.
    /// </summary>
    TMD9004
}
