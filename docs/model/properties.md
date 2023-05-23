# Propriétés

Les propriétés sont définies aussi bien dans des classes que pour des endpoints.

Il y a au total 3 types de propriétés :

## Propriété "standard"

C'est une propriété tout ce qu'il y a de plus standard (comme son nom l'indique). Elle est identifiée par la présence de la propriété `name` en premier.

Elle correspond à un champ "primitif" (<=> ne référence pas une autre classe), et doit donc être obligatoirement associée à un [domaine](/model/domains.md). Elle peut être marquée comme étant (ou faisant partie de) la clé primaire de la classe.

Exemple :

```yaml
name: MontantInitialPret
label: Montant initial du prêt
required: false
domain: DO_MONTANT
comment: Montant initial du prêt.
```

## Association

Une association est une propriété spéciale qui permet de **référencer la clé primaire (simple) d'une autre classe**. L'usage principal est de pouvoir définir des clés étrangères dans un modèle persisté. Elle est identifiée par la présence de la propriété `association` en premier.

Une association peut être obligatoire (ou non) (`required`) et optionnellement définir un rôle (`role`), utilisé pour suffixer le nom de la propriété représentant l'association (et permettre donc d'avoir plusieurs associations vers la même classe).

Une association peut être (ou faire partie de) la clé primaire de la classe (via `primaryKey`).

Une association peut également définir sa multiplicité : `manyToOne` (par défaut), `oneToOne`, `oneToMany` et `manyToMany`.

Une `manyToOne` correspond à une clé étrangère simple vers la classe référencée, tandis que `oneToOne` y ajoute une contrainte d'unicité. Dans ces deux cas, le nom de la propriété sera déterminé automatiquement comme étant `{ClasseCible.Name}{ClasseCible.PrimaryKey}{Rôle}`.

Les `oneToMany` et `manyToMany` sont des associations qui seront implémentées comme des collections de la classe cible. Dans ces deux cas, le nom de la propriété sera déterminé automatiquement comme étant `{ClasseCible.PluralName}{Rôle}`. Le domaine de la clé primaire doit définir un `asDomain` `list` pour pouvoir définir une telle association (le `asDomain` correspondant peut être surchargé en renseignant la propriété `as` sur l'association, qui ne sera utilisée que pour ces types d'associations-là). Il faudra que les implémentations du [domaine](/model/domains.md) utilisé définissent un `genericType` pour préciser le type de collection à utiliser.

La classe référencée par l'association doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Exemple :

```yaml
association: ClasseCible
required: true
comment: C'est une FK obligatoire
```

Il est par ailleurs possible de définir un `role` pour l'association. Le rôle est une chaîne de caractères permettant de spécifier l'usage de l'association. Il viendra se placer en suffix du nom de la propriété.

```yaml
association: ClasseCible
required: true
comment: C'est une FK obligatoire
role: Exemple
```

La propriété qui en découlera sera `ClasseCibleExempleId` (si la `primaryKey` de `ClasseCible` est `Id`).

Une association peut référencer une classe non persistée, dans ce cas il faut identifier la propriété de la classe cible à utiliser via `property` (puisqu'une telle classe ne peut pas avoir de clé primaire par définition).

## Composition

Une composition est une propriété spéciale qui permet de **référencer une autre classe**, à l'inverse de l'association qui ne concerne que la clé primaire. Par conséquent, la composition est le seul type de **propriété non primitif**, ce qui à priori proscrit son usage dans un objet persisté (dans lequel on utilisera plutôt une association). Elle est identifiée par la présence de la propriété `composition` en premier.

Exemple :

```yaml
composition: ClasseCible
name: ClasseCible
comment: Une instance de classe cible.
```

Une composition peut également spécifier un [domaine](/model/domains.md) via la propriété **`domain`**, afin de renseigner une composition avec un **type personnalisé** (au lieu d'une composition simple qui est une simple instance de la classe). L'implémentation du domaine devra définir `genericType`, afin de spécifier le type "conteneur" de la composition.

Par exemple, pour une liste :

```yaml
---
domain:
  name: DO_LIST
  label: List
  csharp:
    genericType: List<{T}>
---
class:
  name: MyClass

  properties:
    - composition: ClasseCible
      name: ClasseCibleList
      kind: DO_LIST
      comment: C'est une liste
```

La classe C# générée aura une propriété `ClasseCibleList` de type `List<ClasseCible>`.

La classe référencée par la composition doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

## Alias

[Voir section correspondante](/model/aliases.md?id=alias-de-propriétés)

## Autres informations de propriétés

- `defaultValue` : toutes les propriétés hors composition peuvent définir une valeur par défaut, qui pourra être utilisée dans les définitions de classes et d'endpoints générées. Cette valeur par défaut ne sera en revanche pas définie en base de données.
- `readonly` : Une propriété readonly ne pourra jamais être la cible d'un [mapper](/model/mappers.md), et ne sera pas ajoutée dans le setter unique d'une [classe abstraite](/model/classes.md#classe-abstraite)
- `trigram` : toutes les propriétés non composées peuvent surcharger le trigramme de la classe (ou de la classe associée dans une association).
