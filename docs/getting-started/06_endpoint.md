# Créer un endpoint

TopModel définit un formalisme permettant de représenter un modèle de données. Le modèle décrit pourra ensuite être décliné dans plusieurs langages de programmation différents.

Mais qu'en est-il des interactions entre ces différents langages ? TopModel permet de définir ces points d'interaction. Concrètement, il permet de décrire des API, qui seront ensuite déclinés en points d'entrée (API server) ou points de sortie (API client) dans les différents langages de programmation.

Qui dit endpoint dit route, et dans le cadre de ce tutoriel, nous utiliserons la propriété `Id` définie dans la classe utilisateur comme principal point d'entré. Une bonne pratique consiste à personnaliser cette propriété par l'ajout d'un `trigram` sur la classe. Nous pouvons réaliser cela en modifiant le fichier `"Utilisateur.tmd"`par l'ajout du champ trigram (nous vous invitons à faire de même) :

```yaml
# Utilisateur.tmd
---
class:
  name: Utilisateur
  trigram : UTI
  comment: Utilisateur de l'application
  properties:
    |
    |
    |
    |
    |........
```




## CRUD : Suppression

Commençons par créer un fichier `"Endpoints.tmd"` qui sera, comme son nom l'indique, `l'endpoint` permettant la suppression d'un utilisateur.


```yaml
# Endpoints.tmd
---
module: UtilisateurEndpoint
uses:
  - Utilisateur
tags: []
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

Nous souhaitons ajouter à notre modèle un `endpoint` pour récupérer des instances de la classe `Utilisateur`. Créons d'abord le Dto correspondant dans `"Dto.tmd"` :

```yaml
# Dto.tmd
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

Notre `endpoint` pourra donc s'écrire de la manière suivante (Ajoutez ces lignes au fichier `"Endpoints.tmd"`) :

```yaml
# Endpoint.tmd
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
    comment: Le détail d'un Utilisateur
```
> **N.B.** : Etant donné que l'on fait appel à un Objet Dto défini dans notre fichier `"Dto.tmd"`, pensez bien à rajouter l'import de ce dernier dans le fichier `"Endpoint.tmd"` :

```yaml
# Endpoints.tmd
---
module: UtilisateurEndpoint
uses:
  - Utilisateur
  - Dto
tags: []
```


## CRUD : Création

Ajoutons maintenant un `endpoint` de création d'utilisateur. Nous pouvons reprendre notre Dto `UtilisateurCreateDto`. Le `endpoint` utilisera le mot clé `POST`.

Le paramètre d'entrée sera maintenant une composition. TopModel comprendra qu'il s'agira du `body` de la requête (Ajoutez ces lignes au fichier `"Endpoints.tmd"`) :

```yaml
# Endpoint.tmd
---
endpoint: # Description du EndPoint
  name: CreateUtilisateur # Nom du endpoint
  method: POST # Méthode Http utilisée
  route: Utilisateur # Route pour accéder à ce endpoint
  description: Créé un nouvel Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme des propriétés
    - composition : UtilisateurCreateDto
      name: detail
      comment: Le détail de l'utilisateur à créer
  returns: # Retour du endpoint. Se décrit comme une composition
    composition: UtilisateurDetailDto
    name: detail
    comment: Le détail de l'utilisateur créé
```

## CRUD : Modification

Ajoutons maintenant un `endpoint` de modification d'utilisateur. Pour cela, on crée dans un premier temps un Dto dans notre fichier `"Dto.tmd"`.  
Dans notre exemple d'application, on ne peut modifier que le nom de l'utilisateur. On pourra donc prendre la classe `UtilisateurUpdateDto` :

```yaml
# Dto.tmd
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

Il ne nous reste plus qu'à ajouter le endpoint dans notre fichier `"Endpoints.tmd"` :
```yaml
# Endpoints.tmd
---
endpoint: # Description du EndPoint
  name: UpdateUtilisateur # Nom du endpoint
  method: PATCH # Méthode Http utilisée
  route: Utilisateur/{utiId} # Route pour accéder à ce endpoint
  description: Modifie un Utilisateur # Description du endpoint
  params: # Paramètres, se décrivent comme des propriétés
    - composition : UtilisateurUpdateDto
      name: detail
      comment: Le détail de l'utilisateur à modifier
    - alias:
        class: Utilisateur
        property: Id 
  returns: # Retour du endpoint. Se décrit comme une composition
    composition: UtilisateurDetailDto
    name: detail
    comment: Le détail de l'utilisateur modifié
```

On voit ici l'utilité de créer un dto par usage. La définition pertinente du dto, du endpoint et du mapping nous permet de nous éviter des vérifications côté serveur. Les propriétés `Email` et `DateInscription` ne devraient pas être modifiées par ce `endpoint`, si les mappers sont bien utilisés. L'évolutivité et la maintenabilité sont assurés !

Aller plus loin dans la documentation complète des [endpoints](/model/endpoints)

## Répertoire projet
Nous venons de couvrir beacoup de notions essentielles. Au début du chapitre, notre répertoire projet était constitué des éléments suivants:
- Projet
  - topmodel.config
  - Utilisateur.tmd
  - Domains.tmd
  - References.tmd
  - Dto.tmd
  - Endpoints.tmd
