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
      
    - name: DateInscriptoin
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

## Cardinalité

Par défaut, l'association est de type `manyToOne`. Dans notre exemple, cela veut dire que chaque instance de la classe `Utilisateur` ne peut référencer qu'une seule fois maximum la classe `TypeUtilisateur`, mais que chaque instance de la classe `TypeUtilisateur` peut être référencée par un nombre indéfini d'instances de la classe `Utilisateur`.

Il est également possible de définir des relations de type `oneToOne`, `oneToMany` et `manyToMany`. Leurs spécificités ne sont pas implémentées dans tous les générateurs, mais permettent de gérer plus de cas. Un exemple avec la classe `Profil`  et la cardinalité `manyToMany` que nous ajouterons à notre fichier `"Utilisateur.tmd"` :

```yaml
# Utilisateur.tmd
---
module: Users
uses:
  - References
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
      
    - name: DateInscriptoin
      comment: Date d'inscription
      domain: DO_DATE
      label: Inscrit depuis le

    - association: TypeUtilisateur
      comment: Type de l'utilisateur
      required: true
      label: Type
      type: manyToOne # Précision facultative, ce paramétrage étant celui par défaut

    - association: Profil
      comment: Profil de l'utilisateur
      required: false
      type: manyToMany

---
class:
  name: Profil
  comment: Profil
  properties:
    - name: Id
      comment: Id technique du profil
      label: Profil
      required: true
      primaryKey: true
      domain: DO_ID

    - name: Nom
      comment: Nom du profil
      label: Profil
      domain: DO_LIBELLE
```

Attention : Etant donné que l'on crée une association `manyToMany`, nous devons modifier la définition de notre clé primaire pour lui permettre de traiter des listes.
A cet effet, on par modifier le fichier `"Domains.tmd"` par l'ajout du champ `asDomains` au domaine `DO_ID` et en définissant le domaine `DO_LIST`. Voici notre fichier `"Domains.tmd"` après les modifications :

```yaml
# Domains.tmd
---
module: Users 
tags: 
  - ""
---
domain:
  name: DO_ID 
  label: ID technique 
  ts:
    type: number 
  java:
    type: long 
  sql:
    type: int8
  asDomains:
    list: DO_LIST
---
domain:
  name: DO_DATE
  label: Date
  ts:
    type: string
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate 
  sql:
    type: timestamp
---
domain:
  name: DO_EMAIL
  label: Email
  length: 50 
  ts:
    type: string
  java:
    type: String
    annotations:
      - text: "@Email" 
        imports:
          - "javax.validation.constraints.Email" 
  sql:
    type: varchar
---
domain:
  name: DO_CODE
  label: Code
  length: 3
  ts:
    type: string
  java:
    type: String
  sql: 
    type: varchar
---
domain:
  name: DO_LIBELLE
  label: Libellé
  length: 15
  ts:
    type: string
  java:
    type: String
  sql: 
    type: varchar
---
domain:
  name: DO_LIST
  label: list
  ts:
    genericType: "{T}[]"
  java:
    type: List<{T}>
    imports:
      - java.util.list
  sql: 
    type: varchar
```


Aller plus loin dans la documentation complète des [associations](/model/properties?id=association)

> **Attention** : Une refonte de la gestion des types d'association est prévue pour une prochaine version. Nous vous recommandons de ne pas utiliser les types d'association `oneToMany` et `manyToMany`. Les remplacer par des `manyToOne` dans l'autre sens, ou par des `manyToMany` explicites (classe contenant deux associations qui ont toutes les deux `primaryKey: true`)

## Répertoire Projet
A ce stade du tutoriel, notre répertoire "Projet" devrait contenir les fichiers suivants:
- Projet
  - topmodel.config
  - Utilisateur.tmd
  - Domains.tmd
  - References.tmd