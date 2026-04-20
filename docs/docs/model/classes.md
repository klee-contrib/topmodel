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
- `reference` : Indique que la classe peut être considérée comme une classe de référence, pour laquelle on suppose que l'ensemble des valeurs ne change pas souvent (voir jamais), et donc qu'il est souhaitable de mettre en cache. Cet attribut est généralement associé à l'ajout d'informations pour la mise en cache et la récupération des éléments depuis ce cache dans les divers générateurs. Pour être définie comme une classe de référence, une classe doit avoir une clé primaire simple (ou à défaut, au moins une propriété avec une clé d'unicité simple).

## Propriétés d'une classe

- `properties` : Liste des [**propriétés**](/model/properties) de la classe.
- `defaultProperty` : Propriété "par défaut" de la classe, parfois utilisée comme libellé ou pour le tri. Doit référencer une propriété existante de la classe. Si non renseignée et qu'il existe une propriété nommée `Label` ou `Libelle`, elle sera automatiquement ajoutée comme `defaultProperty`.
- `orderProperty` : Propriété de tri de la classe, remplace `defaultProperty` pour cet usage. Si non renseignée et qu'il existe une propriété nommée `Order` ou `Ordre`, elle sera automatiquement ajoutée comme `orderProperty`.
- `flagProperty` : Propriété de la classe à utiliser comme flag binaire (ayant des valeurs comme 1, 10, 100, 1000...). Si non renseignée et qu'il existe une propriété nommée `Flag`, elle sera automatiquement ajoutée comme `flagProperty`.
- `localeProperty` : Propriété de la classe à utiliser pour indiquer la locale. Si non renseignée et qu'il existe une propriété nommée `Locale`, elle sera automatiquement ajoutée comme `localeProperty`.
- `preservePropertyCasing` : Par défaut, TopModel converti les noms de propriétés dans la casse du langage cible. Cela veut dire par exemple qu'en C#, tous les noms de propriétés vont être convertis en `PascalCase`, même si la propriété a été déclarée en `camelCase` dans TopModel, et inversement en Java (ce qui était déjà le cas en revanche). De même, si vous avez des noms avec des `_` dans votre modèle (une classe `Profil_utilisateur` ou une propriété `utilisateur_id`), ils seront également convertis de la même façon (en Java par exemple, ça donnerait `ProfilUtilisateur` et `utilisateurId`). Cette propriété, si renseignée à `true`, permet de désactiver ce comportement s'il est important de garder la casse telle qu'elle a été définie dans le modèle (par exemple pour s'interfacer avec une API externe qui n'a pas les mêmes conventions de nommage).

## Indexes et clés d'unicité

TopModel permet de définir des indexes sur une classe via les propriétés `indexes` et/ou `unique` :

```yaml
indexes:
  - properties: [MyProperty] # Définit un index sur la propriété `MyProperty`.

  - [MyProperty2, MyProperty3] # Définit un index sur le couple `MyProperty2` / `MyProperty3`, force raccourcie équivalente à la définition précédente.

  - properties: [MyProperty4]
    unique: true # Définit un index unique sur la propriété `MyProperty4`.

unique:
  - [MyProperty5, MyProperty6] # Définit un index unique sur le couple `MyProperty5` / `MyProperty6`, force raccourcie équivalente à la définition précédente.
```

Toutes ces façons différentes de créer des indexes et des contraintes d'unicité sont équivalentes, les mêmes objets seront crées dans dans le modèle et le même code sera généré derrière.

Si les indexes simples ne sont en général qu'un détail d'implémentation, les indexes uniques quand à eux seront utilisés dans le modèle pour déterminer certains comportements et certaines vérifications.

## Valeurs d'une classe

TopModel permet de définir des valeurs (initiales, ou exhaustives) d'une classe via la propriété `values`. Cela prendra la forme suivante par exemple :

```yaml
values:
  Valeur1: { Code: "1", Libelle: Valeur 1 }
  Valeur2: { Code: "2", Libelle: Valeur 2 }
```

Les différents générateurs utiliseront ces valeurs pour des scripts d'initialisation de base de données, par exemple. Ils utiliseront aussi les [templates de valeurs](./domains#templates-de-valeurs) associés à leur implémentation pour la génération.

## Classe enum

Si la classe définit au moins une `value`, alors cette classe pourra être considérée comme une **enum**, qui est une classe dont **l'ensemble des valeurs possibles est défini dans les `values`**

Une classe enum peut être représentée de **2 façons** :

- Soit comme une **classe** (en renseigant `enum: class` sur la classe). Par rapport à une classe classique, ses **[propriétés uniques](/model/properties#propriété-avec-des-valeurs-et-une-clé-dunicité) peuvent êtres typées comme des enums** (en particulier la clé primaire). De plus, en indiquant **`readonly: true`** sur la classe (ce qui dans le cas général permet de rendre toutes les propriétés `readonly`), **l'ensemble des valeurs pourra être généré de manière statique** sous forme d'instances de la classe dans le code généré.

- Soit comme une **enum** (en renseignant `enum: true` sur la classe). Dans ce cas, la classe deviendra une **"vraie" enum** et non une classe, et sera générée comme telle, selon les capacités du langage cible. La plupart des langages ne supportant que des enums simples, cela pourrait impliquer que toutes les propriétés autre que la clé primaire ne soient pas disponibles. Quelques remarques :
  - Pour définir une classe `enum: true`, une clé primaire simple (une seule propriété marquée comme `primaryKey`, ou à défaut au moins une propriété avec une clé d'unicité simple) est nécessaire sur la classe, et il faut que tous ses identifiants soient valides dans tous les langages cibles (même si vous ne générez du code que dans des langages qui n'ont pas cette restriction)
  - Une [association](/model/properties#association) vers une classe `enum: true` aura toujours `useClass: true` (la clé primaire ciblée et la classe étant la même chose).
  - Une classe `enum: true` ne peut avoir que des associations ou [compositions](/model/properties#composition) vers d'autres classes `enum: true` (les deux types de propriétés seront d'ailleurs tout à fait équivalents ici).

Par défaut, **si une classe peut être une enum, possède une clé primaire, et si son domaine définit ses valeurs comme n'étant pas auto-générées** (via `autoGeneratedValue: false`, ce qui est la valeur par défaut, ce qui est une façon de dire que sa clé primaire n'est pas un ID auto-incrémenté), alors la **valeur par défaut de `enum` sera `class`**. Renseigner `enum: true` sera toujours explicite, et vous pouvez toujours forcer `enum: class` si la clé primaire est auto-générée, ou `enum: false` si vous ne voulez pas d'enum.

## Classe abstraite

Une classe peut être définie comme **abstraite** (via `abstract: true`), pour indiquer à TopModel qu'il ne devra pas essayer de générer des implémentations pour cette classe. En général, cela se traduit par une génération d'interface au lieu d'une classe. Une classe abstraite à les caractéristiques suivantes :

- Elle ne peut pas hériter d'une autre classe ni être héritée (pas de `extends`).
- Elle ne sera pas générée par les générateurs SQL.
- Aucun setter ne sera généré pour les propriétés marquées `readonly: true` (si `readonly: true` est indiqué sur la classe, tous les propriétés seront `readonly: true`, et donc aucun setter ne sera généré tout court).

## Classe persistée

TopModel considère une classe comme étant **persistée** si elle définit **au moins une propriété de clé primaire** (`primaryKey: true`). Cette catégorisation est largement utilisée par les divers générateurs pour déterminer si une classe fait partie du modèle de base de données ou s'il d'agit d'un DTO. Cette distinction, bien qu'importante dans l'architecture générale du modèle d'une application, n'a (presque) aucun impact dans la façon de modéliser des classes dans TopModel. De ce fait, **il n'est pas possible de surcharger cette classification**.

_Remarque : en particulier, une classe enum, une classe de référence, une classe abstraite, ou une classe avec des valeurs peuvent tout à fait être persistées comme non persistées. Naturellement, cela impactera ce qui sera généré._

## Classe de traductions

Si vous souhaitez persister vos traductions (de libellés et/ou de listes de référence) en base de données, vous pouvez renseigner sur l'une de vos classes dans le modèle la propriété `translation: true`. Cela indiquera à TopModel, dans les générateurs qui le supportent, de générer des inserts en base de données avec les traductions, un peu sur le même principe que les valeurs de classes.

Le format d'une classe de traductions est assez rigide, elle doit avoir :

- Une clé primaire simple pour la clé de traduction si l'appli est mono-langue, ou une clé primaire multiple avec la clé + une `localeProperty` si l'appli est multi-langue.
- Une `defaultProperty` pour y mettre la traduction.
- Aucune autre propriété obligatoire.

Puisqu'il s'agit d'une classe normale du modèle, vous pouvez la taguer pour qu'elle ne soit prise en compte que par les générateurs que vous voulez (par exemple, si elle ne doit exister qu'en SQL, alors mettez lui un tag qui ne cible que les générateurs SQL).

## Décorateurs et mappers

Une classe peut implémenter des **[annotations](/model/annotations)**, des **[décorateurs](/model/decorators)** et définir des **[mappers](/model/mappers)**.

## Tags d'une classe

Une classe peut également définir ses propres tags, qui s'ajouteront aux tags du fichier, via la propriété `tags`, pour plus de flexibilité dans l'organisation des classes en fichiers (par exemple, s'il n'y a qu'une seule classe dans un fichier qui a besoin prise en compte par un autre générateur, alors on peut ajouter un tag directement sur cette classe au lieu de la mettre dans un fichier différent).

## Héritage

Il est possible de définir une classe héritée avec le mot clé `extends`. La classe enfant héritera des différentes propriétés, qui pourront être utilisées dans les mappers, mais aussi en tant que `defaultProperty`, `orderProperty` ou `flagProperty` (ou même `joinColumn` dans les [dataFlows](/model/dataFlows)). Ces propriétés héritées pourront également être utilisées dans des `values`, et être référencées comme propriété cible d'une `association`.

### Classes persistées héritées

Il existe plusieurs mode de stockage pour les objets contenant de l'héritage. Dans la plupart des cas étudiés, le mode le plus pertinent est le mode `join`, où chaque classe possède sa propre table, ne contenant que les informations minimum.

Ainsi :

- La table correspondant à la classe parente correspond exactement à sa représentation sans héritage
- La table enfant contient tous les champs qui lui sont spécifiques, mais aussi un champ qui a une contrainte de clé étrangère vers la table parente. En l'absence d'autre clé primaire dans la classe, ce champ sera sa clé primaire (attention : votre ORM vous imposera probablement de mettre ou non une clé primaire explicite sur la table enfant)

A la sauvegarde d'un objet enfant, l'ORM effectuera donc des modifications dans deux tables.

Le code écrit par les différents générateurs correspond à ce mode de fonctionnement, selon les spécificités de chacun.

### Values

Les `values` ajoutées dans la classe enfant viendront implicitement compléter la liste des valeurs des champs de la classe parent. Les `enum` de valeurs seront impactées de la manière suivante :

- Pas d'enum générée pour la clé primaire de la classe enfant. En effet le type de la propriété est le même que celui de la classe parent
- Les valeurs des champs ajoutées par la classe enfant sont ajoutées à l'enum de la classe parent
