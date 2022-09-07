# Créer un endpoint

TopModel définit un formalisme permettant de représenter un modèle de données. Le modèle décrit pourra ensuite être décliné dans plusieurs langages de programmation différents.

Mais qu'en est-il des interactions entre ces différents langages ? TopModel permet de définir ces points d'interaction. Concrètement, il permet de décrire des API, qui seront ensuite déclinés en points d'entrée (API server) ou points de sortie (API client) dans les différents langages de programmation.

## CRUD : Suppression

Commençons par créer un `endpoint` permettant la suppression d'un utilisateur.

```yaml
# Utilisateur.tmd
---
uses:
  - 02_Entities
module: Utilisateur
tags:
  - dto
---
endpoint: # Description du EndPoint
  name: DeleteUtilisateur # Nom du endpoint
  method: DELETE # Méthode Http utilisée
  route: Utilisateur/{utiId} # Route pour accéder à ce endpoint
  description: Supprime un Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme des propriétés
    - alias: # L'avantage d'utiliser un alias est de récupérer les méta-données de cette propiété (commentaire, domaine...) gratuitement
        class: Utilisateur
        property: Id 
```

TopModel comprendra tout seul que la propriété de la route correspond au params `Id` défini ensuite.

## CRUD : Lecture

Nous souhaitons ajouter à notre modèle un `endpoint` pour récupérer des instances de la classe `Utilisateur`. Créons d'abord le Dto correspondant :

```yaml
---
class:
  name: UtilisateurDetailDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas de la consultation de la page de détail
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
    - alias:
        class: Profil
        include:
          - Nom
      suffix: true
  mappers:
    from:
      - params:
        - class: Utilisateur
        - class: Profil
```

> **N.B.** : Vous remarquerez que la facilité de création d'une nouvelle classe non persistée nous pousse à en créer une par usage. Cette pratique permet une meilleure maîtrise des données qui transitent à chaque appel serveur.

Notre `enpoint` pourra donc s'écrire de la manière suivante :

```yaml
---
endpoint: # Description du EndPoint
  name: GetUtilisateur # Nom du endpoint
  method: GET # Méthode Http utilisée
  route: Utilisateur/{utiId} # Route pour accéder à ce endpoint
  description: Charge le détail d'un Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme une liste de propriétés
    - alias: # L'avantage d'utiliser un alias est de récupérer les méta-données de cette propiété (commentaire, domaine) gratuitement
        class: Utilisateur
        property: Id 
  returns: # Retour du endpoint. Se décrit comme une composition
    composition: UtilisateurDetailDto
    name: detail
    kind: object
    comment: Le détail d'un Utilisateur
```

## CRUD : Création

Ajoutons maintenant un `endpoint` de création d'utilisateur. Nous pouvons reprendre notre Dto `UtilisateurCreateDto`. Le `endpoint` utilisera le mot clé `POST`.

Le paramètre d'entrée sera maintenant une composition. TopModel comprendra qu'il s'agira du `body` de la requête :

```yaml
---
endpoint: # Description du EndPoint
  name: CreateUtilisateur # Nom du endpoint
  method: POST # Méthode Http utilisée
  route: Utilisateur # Route pour accéder à ce endpoint
  description: Créé un nouvel Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme des propriétés
    - composition : UtilisateurCreateDto
      name: detail
      kind: object
      comment: Le détail de l'utilisateur à créer
  returns: # Retour du endpoint. Se décrit comme une composition
    composition: UtilisateurDetailDto
    name: detail
    kind: object
    comment: Le détail de l'utilisateur créé
```

## CRUD : Modification

Ajoutons maintenant un `endpoint` de modification d'utilisateur.

```yaml
---
endpoint: # Description du EndPoint
  name: UpdateUtilisateur # Nom du endpoint
  method: PATCH # Méthode Http utilisée
  route: Utilisateur/{utiId} # Route pour accéder à ce endpoint
  description: Modifie un Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme des propriétés
    - composition : UtilisateurUpdateDto
      name: detail
      kind: object
      comment: Le détail de l'utilisateur à modifier
  returns: # Retour du endpoint. Se décrit comme une composition
    composition: UtilisateurDetailDto
    name: detail
    kind: object
    comment: Le détail de l'utilisateur modifié
```

Dans notre exemple d'application, on ne peut modifier que le nom de l'utilisateur. On pourra donc prendre la classe `UtilisateurUpdateDto` :

```yaml
---
class:
  name: UtilisateurUpdateDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas de la modification de celui-ci
  properties:
    - alias:
        class: Utilisateur
        include:
          - Nom
  mappers:
    to:
      - class: Utilisateur
```

On voit ici l'utilité de créer un dto par usage. La définition pertinente du dto, du endpoint et du mapping nous permet de nous éviter des vérifications côté serveur. Le propriétés `Email` et `DateInscription` ne devraient pas être modifiées par ce `endpoint`, si les mappers sont bien utilisés. L'évolutivité et la maintenabilité sont assurés !

Aller plus loin dans la documentation complète des [endpoints](/model/endpoints)
