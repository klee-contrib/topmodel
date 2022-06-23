# Associer les classes

Nous souhaitons maintenant ajouter à la classe `Utilisateur` un lien vers la classe `TypeUtilisateur`. Nous pouvons le faire dans la définition de la classe `Utilisateur` en ajoutant une propriété de type `association`, qui s'écrit comme suit : 

```yaml
    - association: TypeUtilisateur # Classe destination de l'association
      comment: Type de l'utilisateur # Commentaire relié à cette association
      label: Type # Libellé d'affichage du champs correspondant le cas échéant
```

Dans la classe `Utilisateur` nous avons donc :

```yaml
---
module: Utilisateur
tags:
  - entity
---
class:
  name: Utilisateur
  comment: Utilisateur de l'application
  trigram: UTI
  properties:
    - name: Id
      comment: Id technique de l'utilisateur
      label: Utilisateur
      required: true
      primaryKey: true
      domain: DO_ID

    - association: TypeUtilisateur
      comment: Type de l'utilisateur
      label: Type
```

Si vous utilisez l'extension TopModel de VsCode, vous devriez voir apparaître un message d'erreur `La classe 'TypeUtilisateur' est introuvable dans le fichier ou l'une de ses dépendances.`. En effet, la classe `TypeUtilisateur` n'étant pas défini dans le fichier `Utilisateur.tmd`, nous devons l'**importer**. Pour cela, rendez-vous dans le premier bloc du fichier `Utilisateur`, ajouter l'entrée `uses`, qui contient une liste, et référencez le fichier contenant la classe `TypeUtilisateur`, comme ceci :

```yaml
---
module: Utilisateur
uses:
  - References
```

Si vous utilisez l'extension `TopModel` de VsCode, une action rapide vous sera proposée pour ajouter automatiquement tous les imports manquants.

## Cardinalité

Par défaut, l'association est de type `manyToOne`. Dans notre exemple, cela veut dire que chaque instance de la classe `Utilisateur` ne peut référencer qu'une seule fois maximum la classe `TypeUtilisateur`, mais que chaque instance de la classe `TypeUtilisateur` peut être référencée par un nombre indéfini d'instances de la classe `Utilisateur`.

Il est également possible de définir des relations de type `oneToOne`, `oneToMany` et `manyToMany`. Leurs spécificités ne sont pas implémentées dans tous les générateurs, mais permettent de gérer plus de cas. Un exemple avec la classe `Profil` et la cardinalité `manyToMany` :

```yaml
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
      type: manyToOne

    - association: Profil
      comment: Profil de l'utilisateur
      required: false
      type: manyToMany

---
class:
  name: Profil
  comment: Profil
  trigram: PRO
  properties:
    - name: Id
      comment: Id technique du profil
      label: Profil
      required: true
      primaryKey: true
      domain: DO_ID

    - name: Libelle
      comment: Nom du profil
      label: Profil
      domain: DO_LIBELLE
```
