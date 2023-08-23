# Classes

Une classe se définit comme un document YAML, dans un fichier de modèle.

Une classe doit au minimum avoir un **nom** (`name`), un **commentaire** (`comment`) et une **liste de propriétés** (`properties`).

## Définition d'une classe

- `name` : Nom de la classe, qui identifie la classe de manière unique (au moins dans le fichier en cours). Toute référence vers une classe utilisera son nom.
- `pluralName` : Nom de la classe au pluriel, qui pourra être utilisé par certains générateurs pour représenter une collection de cette classe. Si non renseigné, TopModel prendra le nom de la classe et le suffixera par un "s".
- `sqlName` : Surcharge du nom de la classe en SQL, à la place de la transformation en `CONSTANT_CASE` par défaut.
- `comment` : Description de la classe, qui est un champ libre. Comme tous les commentaires dans TopModel, il est obligatoire pour encourager la documentation du modèle. En plus de son utilisation dans les divers générateurs pour des commentaires dans le code, il est aussi utilisé dans l'IDE au survol d'une référence de classe.
- `label` : Libellé de la classe, largement inutilisé.
- `trigram` : Code, à priori de 3 lettres, qui préfixera si renseigné tous les noms de propriétés de la classe en SQL (hors associations, qui utiliseront le trigramme de la classe ciblée).
- `extends` : Référence vers une autre classe pour indiquer que la classe en cours hérite de celle-ci. Il s'agit d'un héritage "classique" qui n'a pas d'impact particulier dans la modélisation.
- `reference` : Indique que la classe peut être considérée comme une classe de référence, pour laquelle on suppose que l'ensemble des valeurs ne change pas souvent (voir jamais), et donc qu'il est souhaitable de mettre en cache. Cet attribut est généralement associé à l'ajout d'informations pour la mise en cache et la récupération des éléments depuis ce cache dans les divers générateurs.

## Propriétés d'une classe

- `properties` : Liste des [**propriétés**](/model/properties.md) de la classe.
- `defaultProperty` : Propriété "par défaut" de la classe, parfois utilisée comme libellé ou pour le tri. Doit référencer une propriété existante de la classe. Si non renseignée et qu'il existe une propriété nommée `Label` ou `Libelle`, elle sera automatiquement ajoutée comme `defaultProperty`.
- `orderProperty` : Propriété de tri de la classe, remplace `defaultProperty` pour cet usage. Si non renseignée et qu'il existe une propriété nommée `Order` ou `Ordre`, elle sera automatiquement ajoutée comme `orderProperty`.
- `flagProperty` : Propriété de la classe à utiliser comme flag binaire (ayant des valeurs comme 1, 10, 100, 1000...). Si non renseignée et qu'il existe une propriété nommée `Flag`, elle sera automatiquement ajoutée comme `flagProperty`.
- `unique` : Clés d'unicité de la classe. Une clé d'unicité est définie comme la liste des propriétés qui la compose (il peut bien évidemment y en avoir qu'une seule). Cela se présente donc comme une liste de liste de propriétés, qu'il vaut mieux représenter de la façon suivante pour que l'autocomplétion fonctionne correctement :
  ```yaml
  unique:
    - [Code]
    - [TypeProfilCode, TypeDroitCode]
  ```
- `preservePropertyCasing` : Par défaut, TopModel converti les noms de propriétés dans la casse du langage cible. Cela veut dire par exemple qu'en C#, tous les noms de propriétés vont être convertis en `PascalCase`, même si la propriété a été déclarée en `camelCase` dans TopModel, et inversement en Java (ce qui était déjà le cas en revanche). De même, si vous avez des noms avec des `_` dans votre modèle (une classe `Profil_utilisateur` ou une propriété `utilisateur_id`), ils seront également convertis de la même façon (en Java par exemple, ça donnerait `ProfilUtilisateur` et `utilisateurId`). Cette propriété, si renseignée à `true`, permet de désactiver ce comportement s'il est important de garder la casse telle qu'elle a été définie dans le modèle (par exemple pour s'interfacer avec une API externe qui n'a pas les mêmes conventions de nommage).

## Valeurs d'une classe

TopModel permet de définir des valeurs (initiales, ou exhaustives) d'une classe via la propriété `values`. Cela prendra la forme suivante par exemple :

```yaml
values:
  Valeur1: { Code: "1", Libelle: Valeur 1 }
  Valeur2: { Code: "2", Libelle: Valeur 2 }
```

Les différents générateurs utiliseront ces valeurs pour des scripts d'initialisation de base de données, par exemple.

## Classe enum

Si la classe définit une clé primaire simple (une seule propriété marquée comme `primaryKey`) et au moins une `value`, alors cette classe pourra être considérée comme une **enum**.

Une enum est une classe dont l'ensemble des valeurs possibles est défini dans les `values`, ce qui permet en particulier de considérer la clé primaire (et les autres clés uniques simples) comme une "vraie" enum, ce qui pourra être exploité par les divers générateurs.

Par défaut, TopModel considère une telle classe comme étant une enum si le domaine de sa clé primaire définit ses valeurs comme n'étant pas auto-générées (`autoGeneratedValue: false`, ce qui est la valeur par défaut), qui est une façon de dire que sa clé primaire n'est pas un ID auto-incrémenté mais un code explicite que l'on peut utiliser. Il est également possible de forcer une telle classe comme une enum en renseignant `enum: true` sur la classe.

## Classe abstraite

Une classe peut être définie comme **abstraite** (via `abstract: true`), pour indiquer à TopModel qu'il ne devra pas essayer de générer des implémentations pour cette classe. En général, cela se traduit par une génération d'interface au lieu d'une classe. Une classe abstraite à les caractéristiques suivantes :

- Elle ne peut pas hériter d'une autre classe ni être héritée (pas de `extends`).
- Elle ne sera pas générée par les générateurs SQL.
- Puisqu'on ne veut pas présupposer de l'implémentation de la classe, on ne générera pas de setter pour les différentes propriétés. A la place, un setter unique (ou une factory) sera généré(e), qui prendra en paramètres l'ensemble des propriétés non marquées `readonly`, qu'il faudra ensuite implémenter dans le code de l'application.

## Classe persistée

TopModel considère une classe comme étant **persistée** si elle définit **au moins une clé primaire qui n'est pas un alias**. Cette catégorisation est largement utilisée par les divers générateurs pour déterminer si une classe fait partie du modèle de base de données ou s'il d'agit d'un DTO. Cette distinction, bien qu'importante dans l'architecture générale du modèle d'une application, n'a (presque) aucun impact dans la façon de modéliser des classes dans TopModel. De ce fait, **il n'est pas possible de surcharger cette classification**.

_Remarque : en particulier, une classe enum, une classe de référence, une classe abstraite, ou une classe avec des valeurs peuvent tout à fait être persistées comme non persistées. Naturellement, cela impactera ce qui sera généré._

## Décorateurs et mappers

Une classe peut implémenter des **[décorateurs](/model/decorators.md)** et définir des **[mappers](/model/mappers.md)**.

## Tags d'une classe

Une classe peut également définir ses propres tags, qui s'ajouteront aux tags du fichier, via la propriété `tags`, pour plus de flexibilité dans l'organisation des classes en fichiers (par exemple, s'il n'y a qu'une seule classe dans un fichier qui a besoin prise en compte par un autre générateur, alors on peut ajouter un tag directement sur cette classe au lieu de la mettre dans un fichier différent).
