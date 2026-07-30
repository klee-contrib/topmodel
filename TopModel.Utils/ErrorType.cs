namespace TopModel.Utils;

public enum ErrorType
{
    #region Erreurs génériques (0)

    /// <summary>
    /// Code d'erreur par défaut.
    /// </summary>
    TMD0000,

    /// <summary>
    /// Nom déjà utilisé.
    /// </summary>
    TMD0001,

    /// <summary>
    /// Classe introuvable.
    /// </summary>
    TMD0002,

    /// <summary>
    /// Domaine introuvable.
    /// </summary>
    TMD0003,

    /// <summary>
    /// Propriété introuvable.
    /// </summary>
    TMD0004,

    /// <summary>
    /// Décorateur introuvable.
    /// </summary>
    TMD0005,

    /// <summary>
    /// Endpoint introuvable.
    /// </summary>
    TMD0006,

    /// <summary>
    /// Paramètre introuvable.
    /// </summary>
    TMD0007,

    /// <summary>
    /// Paramètre obligatoire.
    /// </summary>
    TMD0008,

    /// <summary>
    /// Domaine non utilisé.
    /// </summary>
    TMD0009,

    /// <summary>
    /// Décorateur non utilisé.
    /// </summary>
    TMD0010,

    /// <summary>
    /// Variable introuvable.
    /// </summary>
    TMD0011,

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

    /// <summary>
    /// Fichier en doublon: '{fichier.ToPath()}'.
    /// </summary>
    TMD1006,

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
    /// Classes interfaces ne peuvent être héritées qu'entre elles.
    /// </summary>
    TMD3006,

    /// <summary>
    /// Seule une interface peut être implémentée.
    /// </summary>
    TMD3007,

    /// <summary>
    /// Interface avec implements.
    /// </summary>
    TMD3008,

    /// <summary>
    /// Héritage classe persistée invalide.
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

    /// <summary>
    /// La classe '{classe}' ne peut pas faire partie de la configuration '{genConfig.Name}' car elle hérite de la classe '{classe.Extends}' qui n'y est pas disponible.
    /// </summary>
    TMD3013,

    /// <summary>
    /// Une classe de traduction doit contenir une 'DefaultProperty'.
    /// </summary>
    TMD3014,

    /// <summary>
    /// Si une classe de traduction définit une 'LocaleProperty', elle doit faire partie d'une clé primaire composite avec la clé de traduction.
    /// </summary>
    TMD3015,

    /// <summary>
    /// Une classe de traduction sans 'LocaleProperty' doit avoir une clé primaire simple.
    /// </summary>
    TMD3016,

    /// <summary>
    /// Une classe de traduction doit avoir une 'LocaleProperty' si votre configuration définit plusieurs locales.
    /// </summary>
    TMD3017,

    /// <summary>
    /// Une classe de traduction ne peut pas avoir de propriétés obligatoires autres que sa `DefaultProperty`.
    /// </summary>
    TMD3018,

    /// <summary>
    /// La classe {classe} doit avoir une clé primaire convertible en enum pour être marquée avec `enum: true`.
    /// </summary>
    TMD3019,

    /// <summary>
    /// La classe enum '{classe}' ne peut pas avoir de propriété de composition, ni d'association si la classe cible n'est pas une enum elle-aussi.
    /// </summary>
    TMD3020,

    /// <summary>
    /// Index déjà défini.
    /// </summary>
    TMD3021,

    /// <summary>
    /// Doublon d'interface.
    /// </summary>
    TMD3022,

    /// <summary>
    /// SourcePropertyOrder manquant.
    /// </summary>
    TMD3023,

    /// <summary>
    /// Classe abstraite avec mauvaise stratégie d'héritage.
    /// </summary>
    TMD3024,

    /// <summary>
    /// Distinct-tables sans séquence.
    /// </summary>
    TMD3025,

    /// <summary>
    /// Values sur classe non regular.
    /// </summary>
    TMD3026,

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
    /// Domaine en doublon.
    /// </summary>
    TMD6001,

    /// <summary>
    /// Conversion déjà définie.
    /// </summary>
    TMD6002,

    /// <summary>
    /// Implémentation manquante.
    /// </summary>
    TMD6003,

    /// <summary>
    /// Domaine de collection non générique.
    /// </summary>
    TMD6004,

    #endregion

    #region Erreurs d'endpoints (7)

    /// <summary>
    /// Le préfixe d'endpoint '{file.Options.Endpoints.Prefix}' doit être identique à celui de tous les fichiers de même nom et de même module.
    /// </summary>
    TMD7001,

    /// <summary>
    /// Le fichier définit un préfixe d'endpoint alors qu'il ne contient pas d'endpoint.
    /// </summary>
    TMD7002,

    /// <summary>
    /// Le fichier définit un nom de fichier d'endpoint alors qu'il ne contient pas d'endpoint.
    /// </summary>
    TMD7003,

    /// <summary>
    /// Ordre des paramètres obligatoires.
    /// </summary>
    TMD7004,

    /// <summary>
    /// Paramètre de route introuvable.
    /// </summary>
    TMD7005,

    /// <summary>
    /// L'endpoint '{endpoint}' ne peut pas faire partie de la configuration '{genConfig.Name}' car il dépend la classe '{composition}' qui n'y est pas disponible.
    /// </summary>
    TMD7006,

    /// <summary>
    /// Plusieurs JsonBody ou FormData.
    /// </summary>
    TMD7007,

    /// <summary>
    /// Composition autre que JsonBody ou FormData.
    /// </summary>
    TMD7008,

    /// <summary>
    /// Composition JsonBody alors qu'elle devrait être FormData.
    /// </summary>
    TMD7009,

    /// <summary>
    /// Paramètre dans la route non défini comme paramètre de route.
    /// </summary>
    TMD7010,

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
    /// Propriété référencée en double dans un alias.
    /// </summary>
    TMD9001,

    /// <summary>
    /// Propriété cible d'association manquante (pas de clé primaire simple).
    /// </summary>
    TMD9002,

    /// <summary>
    /// 'as' manquant sur la propriété cible de l'association pour une multiple
    /// </summary>
    TMD9003,

    /// <summary>
    /// 'as' manquant sur la clé primaire de la classe pour calculer la réciproque.
    /// </summary>
    TMD9004,

    /// <summary>
    /// 'as' manquant pour un alias 'as'.
    /// </summary>
    TMD9005,

    /// <summary>
    /// Associtation multiple sur classe sans PK simple.
    /// </summary>
    TMD9006,

    /// <summary>
    /// Association xxxToOne réciproque sur classe sans PK simple.
    /// </summary>
    TMD9007,

    /// <summary>
    /// Référence circulaire manquante pour association réciproque.
    /// </summary>
    TMD9008,

    /// <summary>
    /// Composition sur un alias d'autre chose qu'une composition ou association.
    /// </summary>
    TMD9009,

    /// <summary>
    /// Composition sur une enum: true.
    /// </summary>
    TMD9010,

    /// <summary>
    /// Propriété simple sans domaine non générique.
    /// </summary>
    TMD9011,

    /// <summary>
    /// Alias association multiple impossible.
    /// </summary>
    TMD9012,

    /// <summary>
    /// Valeur sur une propriété interdite.
    /// </summary>
    TMD9013,

    #endregion
}
