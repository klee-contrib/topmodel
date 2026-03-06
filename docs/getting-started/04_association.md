# Associer les classes

Nous souhaitons maintenant ajouter à la classe `Utilisateur` un lien vers la classe `TypeUtilisateur`. Nous pouvons le faire dans la définition de la classe `Utilisateur` en ajoutant une propriété de type `association`, qui s'écrit comme suit :

```yaml
- association: TypeUtilisateur # Classe destination de l'association
  comment: Type de l'utilisateur # Commentaire relié à cette association
  label: Type # Libellé d'affichage du champ correspondant le cas échéant
```

Nous ajoutons donc Dans la classe `Utilisateur` de notre fichier `"Utilisateur.tmd"` l'association :

```yaml
# Utilisateur.tmd
---
module: Users
tags: []
---
class:
  name: Utilisateur
  comment: Utilisateur de l'application
  properties:
    - name: Id
      comment: Identifiant unique de l'utilisateur
      primaryKey: true
      domain: DO_ID

    - name: Email
      comment: Adresse mail de l'utilisateur
      domain: DO_EMAIL
      required: true
      label: Adresse mail

    - name: Nom
      comment: Nom de l'utilisateur
      domain: DO_LIBELLE
      label: Nom

    - name: DateInscription
      comment: Date d'inscription
      domain: DO_DATE
      label: Inscrit depuis le

    - association: TypeUtilisateur
      comment: Type de l'utilisateur
      label: Type
```

Si vous utilisez l'extension TopModel de VsCode, vous devriez voir apparaître un message d'erreur `La classe 'TypeUtilisateur' est introuvable dans le fichier ou l'une de ses dépendances.`. En effet, la classe `TypeUtilisateur` n'étant pas défini dans le fichier `"Utilisateur.tmd"`, nous devons l'**importer**. Pour cela, rendez-vous dans le premier bloc du fichier `"Utilisateur.tmd"`, ajoutez l'entrée `uses`, qui contient une liste, et référencez le fichier contenant la classe `TypeUtilisateur`, comme ceci :

```yaml
# Utilisateur.tmd
---
module: Users
uses:
  - References
tags: []
```

Si vous utilisez l'extension `TopModel` de VsCode, une action rapide vous sera proposée pour ajouter automatiquement tous les imports manquants.

> Comme il existe plusieurs modes de génération pour les classes contenant des valeurs, les associations avec ces classes aussi sont impactées en conséquences. Plus d'informations dans la documentation des [classes](/model/classes.md)

## Répertoire Projet

A ce stade du tutoriel, notre répertoire "Projet" devrait contenir les fichiers suivants:

- Projet
  - topmodel.config
  - Utilisateur.tmd
  - Domains.tmd
  - References.tmd

## Exemple de code généré

<!-- tabs:start -->

### **Java**

```java
 /**
  * Type de l'utilisateur.
  */
 @ManyToOne(fetch = FetchType.LAZY, optional = false, targetEntity = TypeUtilisateur.class)
 @JoinColumn(name = "CODE", referencedColumnName = "CODE")
 private TypeUtilisateur typeUtilisateur;
```

### **C#**

```csharp
    /// <summary>
    /// Type de l'utilisateur.
    /// </summary>
    [Column("code")]
    [Required]
    [ReferencedType(typeof(TypeUtilisateur))]
    [Domain(Domains.Code)]
    public TypeUtilisateur.Codes? TypeUtilisateurCode { get; set; }
```

### **SQL**

```sql

/**
  * Création de la table UTILISATEUR
 **/
create table UTILISATEUR (
 ID int not null,
 EMAIL varchar(50) not null,
 NOM varchar(100),
 DATE_INSCRIPTION date,
 CODE varchar(10) not null,
 constraint PK_UTILISATEUR primary key (ID)
);

/**
  * Création de l'index de clef étrangère pour TYPE_UTILISATEUR.LIBELLE
 **/
create index IDX_TYPE_UTILISATEUR_LIBELLE_FK on TYPE_UTILISATEUR (
 LIBELLE ASC
);
```

<!-- tabs:end -->
