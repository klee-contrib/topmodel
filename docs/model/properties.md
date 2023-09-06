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
      domain: DO_LIST
      comment: C'est une liste
```

La classe C# générée aura une propriété `ClasseCibleList` de type `List<ClasseCible>`.

La classe référencée par la composition doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

## Alias de propriété

Dans le modèle d'une application, il est très courant d'avoir besoin de référencer des propriétés définies dans d'autres objets lorsqu'on construit un nouvel objet. L'exemple le plus courant est le cas des DTOs, qui sont des objets qui vont être utilisés dans l'API publique d'une application et qui sont bien souvent presque identiques à des objets persistés, plutôt réservés à une API "interne".

TopModel permet donc de définir des **alias** de propriétés, qui pourront être utilisés pour **recopier** des définitions déjà écrites par ailleurs dans une une autre classe (ou dans la définition d'un endpoint).

Pour se faire, il est possible de définir un `alias` à la place d'une définition de propriété, par exemple :

```yaml
alias:
  class: MyClass
  property: MyProperty1
prefix: true
```

Il est possible de préfixer ou suffixer le nom de la propriété recopiée (`true` est une valeur spéciale qui correspond au nom de la classe), et même de surcharger son libellé, commentaire, caractère obligatoire, domaine, valeur par défaut, caractère readonly...

Plusieurs propriétés peuvent être recopiées par la même définition d'alias, par exemple :

```yaml
# Recopie toutes les propriétés de MyClass avec "MyClass" comme suffixe.
alias:
  class: MyClass
suffix: true

# Recopie les deux propriétés listées de MyClass en marquant qu'elles ne sont pas obligatoires ici.
alias:
  class: MyClass
  include: # ("property" et "include" sont interchangeables)
    - MyProperty1
    - MyProperty2
required: false

# Recopie toutes les propriétés de MyClass sauf "MyProperty3" avec "Class" comme préfixe.
alias:
  class: MyClass
  exclude:
    - MyProperty3
prefix: Class
```

La classe référencée par l'alias doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Il n'est pas possible de définir un alias sur une propriété de type composition, mais en revanche il est possible de définir un alias d'alias.

Enfin, via la propriété `as`, il est possible de remplacer le domaine de la propriété par le `asDomain` correspondant à la valeur de la propriété `as` définie sur le domaine de la propriété aliasée, au lieu de son domaine.

Exemple :

```yaml
# Génère une propriété MyClassCodeList, de domaine DO_CODE_LIST (en supposant que Code est du domaine DO_CODE qui définit DO_CODE_LIST pour `as: list`). Le type des implémentations de cette propriété sera déterminé à partir du type générique de chaque implémentation, en utilisant le type de la propriété initiale comme paramètre.
alias:
  class: MyClass
  include: Code
as: list
prefix: true
suffix: List
```

De plus, il est possible de surcharger les propriétés suivantes de la propriété initiale :

- `label`
- `comment`
- `domain` (Le remplacement de domaine via `as` se fera sur ce domaine-là au lieu du domaine initial)
- `name` (Les préfixes et suffixes s'ajouteront à ce nom-là au lieu du nom initial)
- `trigram`
- `defaultValue`
- `required`
- `readonly`

Ces surcharges s'appliqueront, comme toutes les autres propriétés de configuration de l'alias, sur toutes les propriétés incluses dans la définition. Par conséquent, si vous voulez changer le libellé ou le nom d'un champ dans un alias, il vous faudra très certainement séparer vos définitions d'alias.

## Valeurs par défaut

La propriété `defaultValue` permet de définir une valeur par défaut sur toutes les propriétés hors composition. Elle sera être utilisée dans les définitions de classes et d'endpoints générés, à condition que la configuration du générateur en question ne spécifie pas `ignoreDefaultValues: true` (ce qui est le cas par défaut du générateur SQL).

## Autres informations de propriétés

- `readonly` : Une propriété readonly ne pourra jamais être la cible d'un [mapper](/model/mappers.md), et ne sera pas ajoutée dans le setter unique d'une [classe abstraite](/model/classes.md#classe-abstraite)
- `trigram` : toutes les propriétés non composées peuvent surcharger le trigramme de la classe (ou de la classe associée dans une association).
