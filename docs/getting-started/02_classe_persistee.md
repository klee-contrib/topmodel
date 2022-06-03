# Création d'une classe persistée

## Déclaration de la classe sans propriétés

Commençons par créer une **classe** `Utilisateur`, qui représenterait les utilisateurs de notre application. Cette classe peut être organisée dans un module, ici ce sera `Users`.
Dans TopModel, le modèle est décrit dans des fichiers au format `yaml`, mais avec l'extension `.tmd`. Ce format est pris en charge par VSCode et l'extension [TopModel](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel). La classe proposée sera représentée de la manière suivante :

```yaml
# Utilisateur.tmd
---
module: Users # Module commun de toutes classes du fichier
---
class:
  name: Utilisateur # Nom de la classe
  comment: Utilisateur de l'application # Commentaire associé à la classe (il est obligatoire)
```

Nous avons donc déclaré notre classe `Utilisateur`, dans le module `Users`, et nous lui avons associé un commentaire.

## Déclaration des propriétés

Nous souhaitons maintenant ajouter à notre classe `Utilisateur` des **propriétés**. Tout d'abord, notre classe `Utilisateur` doit pouvoir être sauvegardée dans une base de données, c'est pourquoi nous lui ajoutons un identifiant unique.

| Propriété | Description                             |
| --------- | --------------------------------------- |
| **Id**    | **Identifiant unique de l'utilisateur** |

A cette propriété, nous souhaitons associer un **domaine**. Il s'agit du type théorique de la propriété, qui sera par la suite décliné dans les différents langages utilisés ( ex : java - Long, TypeScript - number etc). Nous reviendrons vers la configuration des domaines plus tard dans ce tutoriel.

Ajoutons donc un identifiant à la classe `Utilisateur`

```yaml
# Utilisateur.tmd
---
module: Users # Module commun de toutes classes du fichier
---
class:
  name: Utilisateur # Nom de la classe
  comment: Utilisateur de l'application # Commentaire associé à la classe (il est obligatoire)
  properties:
    - name: Id # Nom de la propriété
      comment: Identifiant unique de l'utilisateur # Commentaire associé à la propriété (il est obligatoire)
      primaryKey: true # Indique qu'il s'agit de la clé primaire de la classe
      domain: DO_ID # Type théorique de la propriété. Il sera décliné dans différents langages par la suite

```

Ajoutons maintenant quelques propriétés à la classe utilisateur, qui nous servirons par la suite : un nom, une date d'inscription, et une adresse mail. Nous pouvons compléter ces propriétés avec des informations facultatives. Les plus communes :

- `required` : Indique que la propriété est **obligatoire**
- `label` : Libelle d'affichage de la propriété

```yaml
# Utilisateur.tmd
---
module: Users # Module commun de toutes classes du fichier
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
      required: true # Indique que la propriété Email est obligatoire dans le modèle de données
      label: Adresse mail # Libelle d'affichage de la propriété

    - name: Nom
      comment: Nom de l'utilisateur
      domain: DO_LIBELLE
      label: Nom # Libelle d'affichage de la propriété
      
    - name: DateInscriptoin
      comment: Date d'inscription
      domain: DO_DATE
      label: Inscrit depuis le # Libelle d'affichage de la propriété
```

Nous avons donc créé une classe `Utilisateur` dans le module `users` contenant quatre propriétés.
