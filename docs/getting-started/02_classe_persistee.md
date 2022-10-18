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

> Si vous utilisez l'extension TopModel, vous devriez avoir une erreur due à l'absence de l'attribut `tags` dans les en-têtes. Nous n'évoquerons les `tags` que dans la partie [`Générer du code`](/getting-started/07_generation.md) de ce tutoriel. En attendant, vous pouvez ajouter `tags: []` dans toutes les en-têtes des fichiers que nous allons créer.

## Déclaration des propriétés

Nous souhaitons maintenant ajouter à notre classe `Utilisateur` des **propriétés**. Tout d'abord, notre classe `Utilisateur` doit pouvoir être sauvegardée dans une base de données, c'est pourquoi nous lui ajoutons un identifiant unique.

| Propriété | Description                             |
| --------- | --------------------------------------- |
| **Id**    | **Identifiant unique de l'utilisateur** |

A cette propriété, nous souhaitons associer un **domaine**. Il s'agit du type théorique de la propriété, qui sera par la suite décliné dans les différents langages utilisés ( ex : java - Long, TypeScript - number etc).

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

## Déclaration du domaine

Le domaine représente le type théorique de la propriété, à décliner dans les différents langages utilisés pour la génération. Ici, nous allons prendre un exemple en Java et Typescript (TS). Créons donc un fichier `Domains.tmd` où nous définirons tous nos domaines.

```yaml
# Domains.ts
---
module: Users # Module obligatoire, bien qu'inutile dans le cas des domaines
tags: # tags obligatoires, bien qu'inutiles dans le cas des domaines
  - ""
---
domain:
  name: DO_ID # Nom du domaine utilisé dans la définition des propriétés
  label: ID technique # Description du domaine
  ts:
    type: number # Type TS à utiliser pour ce domaine
  java:
    type: long # Type Java à utiliser pour ce domaine
```

Selon le langage, vous pourrez améliorer la définition des domaines pour les personnaliser totalement à vos besoin. Ci dessous quelques exemples :

```yaml
---
domain:
  name: DO_DATE
  label: Date
  ts:
    type: string
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate # Imports nécessaires au bon fonctionnement de la classe Java
---
domain:
  name: DO_EMAIL
  label: Email
  length: 50 # Taille maximum de la chaine de caractères représentée par ce domaine
  ts:
    type: string
  java:
    type: String
    annotations:
      - "@Email" # Ensemble des annotations à ajouter au dessus de la propriété
    imports:
      - "javax.validation.constraints.Email" # Imports nécessaires au bon fonctionnement de la classe Java
---
domain:
  name: DO_CODE
  label: Code
  length: 3
  ts:
    type: string
  java:
    type: String
  sqlType: varchar
---
domain:
  name: DO_LIBELLE
  label: Libellé
  length: 15
  ts:
    type: string
  java:
    type: String
  sqlType: varchar
```

## Exemple de propriétés

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

Aller plus loin dans la documentation complète de [classes](/model/classes.md)
