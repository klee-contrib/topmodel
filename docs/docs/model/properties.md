# Propriétés

Les propriétés sont définies aussi bien dans des classes que pour des endpoints.

Il y a au total 4 types de propriétés :

## Propriété "standard"

C'est une propriété tout ce qu'il y a de plus standard (comme son nom l'indique). Elle est identifiée par la présence de la propriété `name` en premier.

Elle correspond à un champ "primitif" (<=> ne référence pas une autre classe), et doit donc être obligatoirement associée à un [domaine](/model/domains). Elle peut être marquée comme étant (ou faisant partie de) la clé primaire de la classe.

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

Une association est par défaut représentée par la **propriété** cible, mais dans une [classe persistée](/model/classes#classe-persistée), elle peut être aussi représentée par la **classe** ciblée. Ce comportement est déterminé par la valeur de `useClass` (`true` ou `false`), et sa valeur par défaut est configurée par la valeur de la propriété `defaultAssociationUseClass` (qui vaut `false` par défaut) dans la configuration globale de TopModel. Ce choix de représentation impactera :

- Le type de la propriété d'association : soit c'est celui de la clé primaire de la classe, soit c'est la classe elle-même. Dans le code généré, vous pourriez avoir besoin de représenter votre association comme une classe pour que votre ORM se comporte comme attendu (en JPA, notamment).
- Les [mappings](./mappers) possibles vers depuis et vers cette propriété.
- Le nom de la propriété dans le modèle et dans le code généré. Dans le cas propriété, elle s'appellera `{ClasseCible.Name}{ClasseCible.PrimaryKey}{Rôle}`, tandis que dans le cas classe, ce sera simplement `{ClasseCible.Name}{Rôle}`.

La classe référencée par l'association doit être connue du fichier de modèle courant, soit parce qu'elle est définie dedans, soit parce que son fichier est référencé dans la section `uses`.

Exemple :

```yaml
association: ClasseCible
required: true
comment: C'est une FK obligatoire
```

Et avec un rôle :

```yaml
association: ClasseCible
required: true
comment: C'est une FK obligatoire
role: Exemple
```

Dans ce cas, la propriété qui en découlera sera `ClasseCibleIdExemple` (si la `primaryKey` de `ClasseCible` est `Id`), ou simplement `ClasseCibleExemple` si `useClass` est `true`. Il est possible de surcharger le nom de la classe cible dans la propriété (donc ici `ClasseCible`) via la propriété `className`.

Une association peut référencer une classe non persistée, dans ce cas il faut identifier la propriété de la classe cible à utiliser via `property` (puisqu'une telle classe ne peut pas avoir de clé primaire par définition).

### Associations multiples et réciproques

Via `withReverse`, il est possible de déclarer **l'association réciproque sur la classe cible de l'association**. Pour une association sans contrainte d'unicité (parce qu'il s'agit d'une clé primaire simple, où bien si une clé d'unicité simple est définie dessus), classiquement appelée "Many to One", l'association réciproque sera une "One to Many", représentée par une collection de la classe source (si `useClass: true` est renseigné sur l'association, sinon vous aurez une liste de la clé primaire de la classe source, ce qui est rarement exploitable...) de l'association. Pour une association avec contrainte d'unicité (une "One to One"), la réciproque sera du même type que la propriété originale (une "One to One" également).

Une association peut également être directement marquée comme **multiple**, ce qui permet de définir cette "One to Many" explicitement dans le modèle, et d'avoir son association "classique" comme réciproque. Dans ce cas-là, l'**association réciproque est automatiquement ajoutée au modèle**, puisque la propriété "réelle" d'association (typiquement, la clé étrangère en base de données) est celle de la réciproque.

Puisqu'une **association multiple est une collection, il est nécessaire de changer son domaine**. Il sera égal au `asDomain` `list` (par défaut, surchargeable via `as` sur l'association) de la propriété d'association source. Il faudra que les implémentations du [domaine](/model/domains) utilisé définissent un `genericType` pour préciser le type de collection à utiliser.

La propriété correspondant à l'association réciproque sera **ajoutée effectivement comme une propriété d'association sur la classe cible**. En particulier, cela imposera une **référence circulaire** entre les deux classes, et donc les fichiers qui les contiennent. Cette dépendance devra être déclarée explicitement (si elle ne l'est pas déjà par ailleurs, ou si les deux classes ne sont pas déjà dans le même fichier) sur le fichier de la classe cible. Les cycles de dépendances sont traités comme un seul gros fichier par TopModel, donc pour simplifier la résolution et éviter des effets de bord indésirables, il est conseillé de les réduire au minimum possible.

Une association réciproque peut être déclarée via `withReverse: true`, ou par un objet qui peut paramétrer la propriété d'association réciproque :

```yaml
withReverse:
  className: # Equivalent de `className` sur l'association.
  label: # Equivalent de `label` sur l'association.
  comment: # Equivalent de `comment` sur l'association. Un commentaire est généré par défaut s'il n'y en a pas.
  annotations: # Annotations a ajouter sur l'association réciproque.
```

Les associations réciproques étant de vraies propriétés de classe dans le modèle, elles sont disponibles dans les alias et les mappers.

## Composition

Une composition est une propriété spéciale qui permet de **référencer une autre classe**, à l'inverse de l'association qui ne concerne que la clé primaire. Par conséquent, la composition est le seul type de **propriété non primitif** (ce qui ne prescrit pas à priori son usage dans un objet persisté puisqu'elle pourrait y être stockée en JSON, mais on préfèrera bien souvent utiliser une association à la place). Elle est identifiée par la présence de la propriété `composition` en premier.

Exemple :

```yaml
composition: ClasseCible
name: ClasseCible
comment: Une instance de classe cible.
```

Une composition peut également spécifier un [domaine](/model/domains) via la propriété **`domain`**, afin de renseigner une composition avec un **type personnalisé** (au lieu d'une composition simple qui est une simple instance de la classe). L'implémentation du domaine devra définir `genericType`, afin de spécifier le type "conteneur" de la composition.

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

TopModel permet donc de définir des **alias** de propriétés, qui pourront être utilisés pour **recopier** des définitions déjà écrites par ailleurs dans une autre classe, un autre endpoint, ou un autre décorateur.

Pour se faire, il est possible de définir un `alias` à la place d'une définition de propriété, par exemple :

```yaml
alias:
  class: MyClass
  property: MyProperty1
prefix: true

alias:
  endpoint: MyEndpoint
  property: MyParameter # Vous pouvez référencer à la fois les paramètres comme la propriété de retour.

alias:
  decorator: MyDecorator
  property: MyProperty
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

Il est tout à fait possible de définir un alias d'alias.

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
- `primaryKey` (Voir les règles en dessous)
- `trigram` (Voir les règles en dessous)
- `composition` (N'est possible que pour une composition ou une association, qui sera donc transformée en composition)
- `defaultValue`
- `required`
- `readonly`

Ces surcharges s'appliqueront, comme toutes les autres propriétés de configuration de l'alias, sur toutes les propriétés incluses dans la définition. Par conséquent, si vous voulez changer le libellé ou le nom d'un champ dans un alias, il vous faudra très certainement séparer vos définitions d'alias.

### Alias de clé primaire

**La valeur de `primaryKey` n'est par défaut pas recopiée** depuis la définition de la propriété initiale : la notion de clé primaire étant liée à la classe (et permettant de déterminer si la classe est [persistée](./classes#classe-persistée)), il est logique qu'elle ne soit pas recopiée sur l'alias. Un alias peut néanmoins faire partie de la clé primaire de la classe qui le défini, soit avec `primaryKey: true` comme pour les autres propriétés, soit en renseignant `preservePrimaryKey: true` sur l'alias pour surcharger le comportement par défaut qui ne le reprend pas.

_Remarque : `preversePrimaryKey: true` ne conserve pas le caractère auto-génénéré de la valeur (portée par le domaine via `autoGeneratedValue: true` ou une configuration de séquence) pour les clés primaires définies ainsi. Il n'y aura donc pas de séquence ou d'identité posée sur la colonne en base de données._

### Trigramme sur un alias

Par défaut, **le trigramme d'une propriété d'alias est celui de la propriété de classe persistante à laquelle il fait référence**, qui peut être **lui-même** s'il est dans une classe persistante. En particulier :

- Si la classe de l'alias n'est pas persistante, alors la valeur de `trigram` de la classe de l'alias ou de la propriété d'alias sera ignorée (puisque ce sera celui de la propriété persistante à laquelle il fait référence).
- Si la classe de l'alias est persistante, alors la valeur de `trigram` de la propriété référencée (directement dessus ou via sa classe) sera ignorée (sauf s'il s'agit d'une association, puisque dans ce cas le trigramme ne vient pas d'ici mais de la classe associée). Si vous voulez un trigramme, il faudra le définir sur la classe ou sur la propriété d'alias.

Si vous voulez simplement récupérer le trigramme de la propriété référencée par l'alias sur l'alias, indépendement de la persistance de l'une ou de l'autre, vous pouvez renseigner `preserveTrigram: true` sur sa définition. À noter que vous ne pourrez donc plus surcharger ce trigramme via `trigram`, comme dans le premier cas.

## Valeurs par défaut

La propriété `defaultValue` permet de définir une valeur par défaut sur toutes les propriétés hors composition. Elle sera être utilisée dans les définitions de classes et d'endpoints générés, à condition que la configuration du générateur en question ne spécifie pas `ignoreDefaultValues: true` (ce qui est le cas par défaut du générateur SQL). Les générateurs utiliseront les [templates de valeurs](./domains#templates-de-valeurs) associés à leur implémentation pour la génération.

## Propriété avec des valeurs et une clé d'unicité

Une telle propriété, comme son nom l'indique, est définie par l'existence de **[valeurs sur la classe](/model/classes#valeurs-dune-classe)**, ainsi qu'une **clé d'unicité qui porte sur cette propriété** (comme une clé primaire simple par exemple).

Ces valeurs étant uniques, elle pourront être **représentées dans le code généré** :

- Soit par une **enum**, si la classe qui la contient est une [classe enum](/model/classes#classe-enum) (sans `enum: true`, puisque sinon la classe entière est une enum et les propriétés n'existent pas indépendamment). Dans ce cas, cela impactera le **type** de cette propriété qui **écrasera celui défini dans le domaine** par cette enum.
- Soit par une liste de constantes.

Le choix de réprésentation dépendra de si les valeurs peuvent bien constituer une enum dans le langage cible (les restrictions sur les noms de variables peuvent s'appliquer sur certains langages, comme C# ou Java, et il faut aussi que le langage puisse supporter des enums de manière générale). De plus, une option de génération commune à tous les générateurs (si implémentée), `uniqueValueGeneration`, peut adapter ce qui est effectivement généré :

- `enum-or-const` (la valeur par défaut) génèrera des enums si possible, et des constantes pour le reste
- `const-only` génèrera des constantes pour tout
- `enum-only` ne génèrera que les enums possibles
- `none` ne génèrera rien

## Annotations

Une propriété peut recevoir des [annotations](/model/annotations), qui seront ajoutées au code généré s'il y a bien une implémentation correspondante au language générée, et que le type d'objet ciblé correspond. Toute annotation ciblant `property` ou `XXX-property` peut être posée sur une propriété (ou son domaine), mais elle ne sera effectivement générée que si le type de propriété correspond (ou qu'elle cible `property`).

```yaml
properties:
  - name: DateCreation
    label: Date de création
    required: true
    defaultValue: now
    domain: DO_DATE_HEURE
    comment: Date de création de l'utilisateur.
    annotations:
      - LastDate:
          change: Created
```

Un alias hérite des annotations de la propriété source, ainsi que de son type pour le ciblage. Un alias d'association pourra donc être ciblé par des annotations qui ciblent les associations. Les annotations renseignées sur l'alias lui même seront ajoutées aux annotations existantes.

## Tags de propriétés

Chaque classe ou endpoint sera transmis aux configurations qui référencent les tags de ces objets (qu'ils soient sur le fichier ou la classe/l'endpoint lui-même). Par défaut, l'ensemble des propriétés sera généré par les générateurs de ces configurations. Parfois, cela n'est pas souhaitable, par exemple si :

- La propriété est déjà implémentée par une classe externe spécifiée dans un `extends` d'un [décorateur](/model/decorators)
- On ne veut pas que la propriété soit générée dans un client d'API alors qu'elle existe sur le serveur (pour du versionning, pour masquer des propriétés qui ne devraient pas être visibles en front..)

Pour répondre à ce besoin, vous pouvez spécifier des **tags sur les propriétés**. Si la propriété `tags` n'est pas renseignée, alors la propriété sera générée normalement, mais si renseignée, alors la propriété ne sera générée **que pour les tags listés** :

```yaml
- name: MyProperty
  domain: DO_CODE
  comment: Propriété toujours générée.
- name: MyProperty2
  domain: DO_CODE
  tags: [back]
  comment: Propriété générée uniquement en back.
- name: MyProperty3
  domain: DO_CODE
  tags: []
  comment: Propriété jamais générée.
```

Quelques remarques :

- Les tags sur les paramètres de routes des endpoints sont ignorés : on ne peut pas ne pas générer un paramètre de route vu qu'il fait partie de la définition de l'endpoint.
- Les tags ne sont **pas hérités par les alias**. La non-génération d'une propriété est liée à la propriété elle-même, et non à son héritage via les alias. Vous pouvez toujours recopier les tags si besoin.
- Les tags sont en revanche bien **recopiés dans les implémentations** (décorateurs et interfaces). Il s'agit bien ici de la même propriété.
- Les mappings de mappers ne prennent pas en compte la non-génération éventuelle de la propriété, puisque la propriété pourrait quand même exister dans le code final. Si un mapping fait référence à une propriété inexistante car non générée, supprimez le mapping.

## Autres informations de propriétés

- `readonly` : Une propriété readonly ne devrait être renseignable qu'à la création d'une classe (une notion qui n'est représentable qu'en C# malheureusement..), mais en revanche, dans une [interface](/model/classes#interfaces-et-classes-abstraites), elle n'aura pas de setter.
- `trigram` : toutes les propriétés non composées peuvent surcharger le trigramme de la classe (ou de la classe associée dans une association).
