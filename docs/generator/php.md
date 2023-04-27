# Php Generator

## Présentation

Le générateur PHP peut générer les fichiers suivants :

- Un fichier de définition de classe pour chaque classe dans le modèle.
- Un fichier de définition de repository héritant `ServiceEntityRepository` pour chacune des classes persistées du modèle.

Le générateur PHP s'appuie sur les frameworks Symphony et Doctrine

## Génération des classes

Le générateur de classes distingue trois cas :

- Les classes persistées : les classes qui possèdent une propriété avec `primaryKey: true`
- Les classes non persistées

Les propriétés sont générées sont `private`, du type défini dans le `domain`. Le commentaire du modèle est ignoré. Les seuls commentaires ajoutés aux classes générées sont ceux qui permettent de préciser le type d'une collection.

Des `getter` et `setter` sont ajoutés automatiquement. Les `setter` renvoient l'instance courante `this`, et ont donc un type de retour `self`.

Des constructeurs sont ajoutés dans les classes qui contiennent des `Collection`, afin de les initialiser.

## Génération des Repositories

Un fichier de classe Repositories est généré pour chacune des classes persistées du modèle. Cette classe hérite de `ServiceEntityRepository`, et est paramétrée pour gérer l'entité correspondante.

**Ce fichier n'est généré qu'une seule fois !!**. Vous pouvez donc le modifier pour ajouter les différentes méthodes d'accès dont vous auriez besoin. C'est tout l'intérêt.
