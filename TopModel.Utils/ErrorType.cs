namespace TopModel.Utils;

public enum ErrorType
{
    #region Erreurs génériques (0)

    /// <summary>
    /// Code d'erreur par défaut
    /// </summary>
    TMD0000,

    /// <summary>
    /// Le nom '{0}' est déjà utilisé.
    /// </summary>
    TMD0001,

    /// <summary>
    /// La classe '{0}' est introuvable.
    /// </summary>
    TMD0002,

    /// <summary>
    /// Le domaine '{0}' est introuvable.
    /// </summary>
    TMD0003,

    /// <summary>
    /// La propriété '{0}' est introuvable.
    /// </summary>
    TMD0004,

    /// <summary>
    /// Le décorateur '{0}' est introuvable.
    /// </summary>
    TMD0005,

    /// <summary>
    /// L'endpoint est introuvable dans le fichier
    /// </summary>
    TMD0006,

    /// <summary>
    /// Le paramètre '{0}' n'existe pas.
    /// </summary>
    TMD0007,

    /// <summary>
    /// Le paramètre '{0}' est obligatoire.
    /// </summary>
    TMD0008,

    /// <summary>
    /// Le domaine '{domain.Name}' n'est pas utilisé.
    /// </summary>
    TMD0009,

    /// <summary>
    /// Le décorateur '{decorateur.Name}' n'est pas utilisé.
    /// </summary>
    TMD0010,

    #endregion

    #region Erreurs de fichiers (1)

    /// <summary>
    /// Le fichier référencé '{use.ReferenceName}' est introuvable
    /// </summary>
    TMD1001,

    /// <summary>
    /// L'import '{use.ReferenceName}' ne doit être spécifié qu'une seule fois
    /// </summary>
    TMD1002,

    /// <summary>
    /// L'import {} n'est pas utilisé.
    /// </summary>
    TMD1003,

    /// <summary>
    /// Le fichier '{relativePath}' ne sera pas regénéré pour le motif : '{motif}'
    /// </summary>
    TMD1004,

    /// <summary>
    /// Le fichier '{ignoredFile}' dans `ignoredFiles` est introuvable.
    /// </summary>
    TMD1005,

    #endregion

    #region Erreurs d'annotations (2)

    /// <summary>
    /// L'annotation est introuvable dans le fichier.
    /// </summary>
    TMD2001,

    /// <summary>
    /// L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine '{domaine}'.
    /// </summary>
    TMD2002,

    /// <summary>
    /// L'annotation '{annotationRef.ReferenceName}' est déjà présente dans la liste des annotations du domaine de la propriété '{property}'.
    /// </summary>
    TMD2003,

    /// <summary>
    /// Impossible d'appliquer l'annotation '{annotationRef.ReferenceName}' à '{container}' : l'annotation ne cible pas le bon type d'objet.
    /// </summary>
    TMD2004,

    /// <summary>
    /// Une annotation globale ne peut pas définir de paramètres.
    /// </summary>
    TMD2005,

    #endregion

    #region Erreurs de classes (3)

    /// <summary>
    /// La classe '{classe}' est définie plusieurs fois dans le fichier ou une de ses dépendences.
    /// </summary>
    TMD3001,

    /// <summary>
    /// La classe doit avoir au moins une propriété non composée et au plus une clé primaire pour être définie comme `reference`.
    /// </summary>
    TMD3002,

    /// <summary>
    /// La classe '{classe}' doit avoir au moins une propriété non composée, au plus une clé primaire et au moins une `value` pour être définie comme `enum`.
    /// </summary>
    TMD3003,

    /// <summary>
    /// Le trigram '{classe.Trigram}' est déjà utilisé.
    /// </summary>
    TMD3004,

    /// <summary>
    /// Cette association sur la classe '{classe}' doit définir un rôle.
    /// </summary>
    TMD3005,

    /// <summary>
    /// Une association doit être de type 'oneToOne' pour être la clé primaire d'une classe.
    /// </summary>
    TMD3006,

    /// <summary>
    /// Les associations d'une clé primaire composite doivent être de type 'manyToOne'.
    /// </summary>
    TMD3007,

    /// <summary>
    /// Impossible de définir un 'extends' sur la classe '{classe}' abstraite.
    /// </summary>
    TMD3008,

    /// <summary>
    /// Impossible de définir un 'extends' sur la classe '{classe}' car elle a une clé primaire composite.
    /// </summary>
    TMD3009,

    /// <summary>
    /// La classe '{classe}' et sa classe parente '{classe.Extends}' doivent toutes les deux être des `enum`.
    /// </summary>
    TMD3010,

    /// <summary>
    /// La valeur '{valueRef.Key.ReferenceName}' n'initialise pas les propriétés obligatoires suivantes.
    /// </summary>
    TMD3011,

    /// <summary>
    /// La valeur viole la contrainte d'unicité [{string.Join(", ", uk.Select(u => u.Name))}]
    /// </summary>
    TMD3012,

    #endregion

    #region Erreurs de dataflows (4)

    /// <summary>
    /// Le flux de données '{dataFlow}' est défini plusieurs fois dans le fichier ou une de ses dépendences.
    /// </summary>
    TMD4001,

    /// <summary>
    /// Le flux de données est introuvable dans le fichier ou l'une de ses références.
    /// </summary>
    TMD4002,

    #endregion

    #region Erreurs de décorateurs (5)

    /// <summary>
    /// Le décorateur '{decoratorRef.ReferenceName}' est déjà présent dans la liste des décorateurs de la classe '{classe}'.
    /// </summary>
    TMD5001,

    /// <summary>
    /// Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à la classe '{classe}' : seul un 'extends' peut être spécifié.
    /// </summary>
    TMD5002,

    /// <summary>
    /// Impossible d'appliquer le décorateur '{decoratorRef.ReferenceName}' à '{container}' : le décorateur ne cible pas le bon type d'objet.
    /// </summary>
    TMD5003,

    #endregion

    #region Erreurs de domaines (6)

    /// <summary>
    /// Le domaine '{domain}' est déjà défini.
    /// </summary>
    TMD6001,

    /// <summary>
    /// La définition de la conversion entre {df.Name} et {dt.Name} est déjà définie dans un autre converter.
    /// </summary>
    TMD6002,

    #endregion

    #region Erreurs d'endpoints (7)

    /// <summary>
    /// Le préfixe d'endpoint '{file.Options.Endpoints.Prefix}' doit être identique à celui de tous les fichiers de même nom et de même module.
    /// </summary>
    TMD7001,

    /// <summary>
    /// Le paramètre de requête '{queryParam.GetParamName()}' doit suivre tous les paramètres de route ou de body dans un endpoint.
    /// </summary>
    TMD7002,

    /// <summary>
    /// Le endpoint '{endpoint.Name}' définit un paramètre '{routeParamName}' dans sa route qui n'existe pas dans la liste des paramètres.
    /// </summary>
    TMD7003,

    #endregion

    #region Erreurs de mappers (8)

    /// <summary>
    /// La propriété '{mappedProperty.Name}' ne peut pas être mappée à '{currentProperty.Name}' car elle n'a pas le même domaine.
    /// </summary>
    TMD8001,

    /// <summary>
    /// La propriété '{mapping.Key.ReferenceName}' est déjà initialisée dans ce mapper.
    /// </summary>
    TMD8002,

    /// <summary>
    /// Plusieurs propriétés de la classe peuvent être mappées sur '{mapping.Value.Name}'.
    /// </summary>
    TMD8003,

    /// <summary>
    /// La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car ce n'est pas une association.
    /// </summary>
    TMD8004,

    /// <summary>
    /// L'association '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car l'association et la composition doivent toutes les deux être simples.
    /// </summary>
    TMD8005,

    /// <summary>
    /// La propriété '{mappedProperty.Name}' ne peut pas être mappée à la composition '{currentProperty.Name}' car elle n'a pas le même domaine que la clé primaire de la classe '{cp.Composition.Name}' composée.
    /// </summary>
    TMD8006,

    /// <summary>
    /// La classe '{mappedClass.Name}' ne peut pas être mappée sur la propriété '{currentProperty.Name}' car ce n'est pas une composition de cette classe.
    /// </summary>
    TMD8007,

    /// <summary>
    /// La propriété '{property}' ne peut pas être la cible d'un mapping car elle a été marquée comme 'readonly'.
    /// </summary>
    TMD8008,

    /// <summary>
    /// Aucun mapping n'a été trouvé sur ce mapper.
    /// </summary>
    TMD8009,

    /// <summary>
    /// La propriété '{mapping.Property.Name}' doit être une composition de la même classe que '{mapping.TargetProperty.Name}' pour définir un mapping entre les deux.
    /// </summary>
    TMD8010,

    /// <summary>
    /// La propriété '{mapping.Property.Name}' ne peut pas être une composition pour définir un mapping vers '{mapping.TargetProperty.Name}'.
    /// </summary>
    TMD8011,

    /// <summary>
    /// Le paramètre '{param.GetName()}' du mapper ne peut pas être obligatoire si l'un des paramètres précédents ne l'est pas.
    /// </summary>
    TMD8012,

    #endregion

    #region Erreurs de propriétés (9)

    /// <summary>
    /// La propriété '{propertyReference.Name}' est déjà référencée.
    /// </summary>
    TMD9001,

    /// <summary>
    /// La classe '{0}' doit avoir une (et une seule) clé primaire pour être référencée dans une association.
    /// </summary>
    TMD9002,

    /// <summary>
    /// Cette association ne peut pas avoir le type {ap.Type} car le domain {ap.Class.PrimaryKey.Single().Domain} ne contient pas de définition de AsDomain
    /// </summary>
    TMD9003,

    /// <summary>
    /// Le domaine '{prop.Domain}' doit définir un domaine de liste pour définir un alias liste sur la propriété '{prop.OriginalProperty}' de la classe '{prop.OriginalProperty?.Class}'.
    /// </summary>
    TMD9004

    #endregion
}
