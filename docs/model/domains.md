# Domaines

Un domaine se définit comme un document YAML, dans un fichier de modèle.

Un domaine correspond à un type métier. Chaque champ doit avoir un domaine. Les règles de gestion liées à chaque domaine devront être implémentées dans chaque couche technique. En revanche, il faut par contre décrire ici, dans le modèle, comment chaque domaine va être représenté dans chaque langage, puisque la génération va en avoir besoin.

Un domaine se décrit donc de la façon suivante :

```yaml
---
domain:
  name: DO_ID
  label: Identifiant
  length: # Longueur du champ, si applicable.
  scale: # Nombre de décimales du champ, si applicable.
  asDomains:
    list: DO_ID_LIST # Domaine a utiliser s'il faut transformer le domaine du champ en 'list'.
  csharp:
    type: int?
    annotations: # Liste d'annotations à devoir ajouter à toute propriété de ce domaine, si applicable.
    imports: # Liste d'imports à devoir ajouter à la définition d'une classe qui utilise ce domaine, si applicable.
  ts:
    type: number
    imports:
      -  # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  java:
    type: Integer
    imports: # Chemin des imports à ajouter pour utiliser le type, si applicable.
  sql:
    type: int
```

Les définitions de langages ont toutes le même format, indépendamment du langage choisi. Chaque configuration de générateur choisira une implémentation de domaine qui correspondra à son langage (à priori `csharp`, `java`, `ts`, `sql`...).

Naturellement, il n'est pas nécessaire de spécifier les langages pour lesquels le domaine n'est pas utilisé (et c'est évidemment obligatoire sinon).

Il n'y a pas besoin de préciser les dépendances aux fichiers contenant des domaines dans `uses` : tous les domaines sont automatiquement accessibles dans tous les fichiers. En revanche, cela implique que tous les fichiers ont une dépendance implicite à tous les fichiers contenant des domaines, ce qui pourrait entraîner des dépendances circulaires entre fichiers (qui ne sont **pas** supportées) involontaires. Par conséquent, et également par soucis de clarté, **il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions**.

Il est possible de définir le `mediaType` du domaine. Cette information pourra être prise en compte par certains générateurs (notamment les générateurs d'API).

Un domaine peut également définir des `asDomains`, qui sont des domaines à utiliser lorsque l'on a besoin de transformer le domaine d'une propriété que l'on référence. Il peut être utilisé dans un alias via `as`, et les associations `oneToMany` et `manyToMany` ont besoin d'avoir un `asDomain` `list` défini sur le domaine de la clé primaire pour être utilisées (puisqu'il y a une transformation de son type a réaliser).

Exemple :

```yaml
---
domain:
  name: DO_PDF
  mediaType: application/pdf
  label: File Response Entity
  ts:
    type: File
  java:
    type: ResponseEntity<Resource>
    imports:
      - org.springframework.http.ResponseEntity
      - org.springframework.core.io.Resource
```

## Types d'implémentation génériques

Une implémentation de langage peut définir, en plus du `type`, un **`genericType`**. Ce champ peut (et devrait) référencer la variable spéciale `{T}`, qui correspond au type original de la propriété. Le type générique est utilisé pour les 3 cas suivants :

- Lorsque le domaine est utilisé pour une **composition**, et la variable **`{T}` est égal au nom de la classe**. Si `genericType` n'est pas renseigné pour l'implémentation du domaine, il vaudra `"{T}"`. Un domaine utilisé pour une composition peut toujours être utilisé pour un autre type de propriété, il ne faudra pas oublier de renseigner la valeur de `type` (non générique).
- Lorsque la propriété utilisant la propriété est considérée comme une **enum** par la configuration du générateur. Elle vaut également `"{T}"` par défaut, **`{T}` correspondant à la représentation de l'enum dans le langage cible**.
- Lorsque le domaine est utilisé dans une **transformation de domaine** (via `as`, donc les cas décrits dans le premier point de cette PR). Si `genericType` n'est pas renseigné dans ce cas, elle vaudra par défaut la valeur de `type`, ce qui veut dire qu'il faut nécessairement spécifier la transformation pour qu'elle soit réalisée. Auparavant, cela n'existait que pour `asList: true` (et les associations `toMany`), et chaque générateur implémentait en dur la transformation à réaliser sur le type original (impossible à débrancher, et souvent en ajoutant `"[]"` à la fin ou en mettant `List<>` autour). **`{T}` référencera ici le type original** (qui peut être une enum ou une association par exemple).

Par exemple, le domaine `DO_ID_LIST` référencé dans l'exemple précédent comme `asDomain` de `DO_ID` devrait être défini ainsi :

```yaml
---
domain:
  name: DO_ID_LIST
  label: Code
  ts:
    type: number[]
    genericType: "{type}[]"
  csharp:
    type: int[]
    genericType: "{type}[]"
  java:
    type: List<int>
    genericType: List<{type}>
    imports:
      - java.util.List
```

## Templating

Il est possible que certaines propriétés des domaines dépendent de la propriété sur laquelle vous l'ajouter. Vous pourriez par exemple ajouter une annotation `@Label` dans le code `java` qui aurait besoin du libelle renseigné dans TopModel.

Pour utiliser un attribut de la propriété dans le domaine, il suffit de référencer cette propriété entre accolades :

```yaml
---
domain:
  name: DO_ID
  label: Identifiant
  java:
    type: Integer
    annotations:
      - text: @Label(\"{label}\")
        imports:
          - topmodel.sample.custom.annotation.Label
```

Le code généré sera ainsi différent selon la propriété sur laquelle vous allez effectivement ajouter ce domaine/

Actuellement il est possible d'utiliser ces variables

- `class.name`
- `class.sqlName`
- `name`
- `trigram`
- `label`
- `comment`
- `required`
- `resourceKey`
- `defaultValue`

Dans le cadre d'une composition, il est possible d'utiliser ces variables :

- `class.name`
- `composition.name`
- `name`
- `label`
- `comment`

Le tout dans les propriétés d'implémentation :

- `type`
- `annotations`
- `imports`

Les templates des domaines des propriétés sont également valorisés. Ces variables s'ajoutent à la variable `{T}` utilisée dans les types génériques.

### Paramètres

Il est également possible de passer des paramètres lorsqu'on associe un domaineà une propriété, en passant un objet `{name, parameters}` au lieu du nom du domaine :

```yaml
properties:
  - name: MyProperty
    domain:
      name: DO_CODE
      parameters: ["Param1", "Param2"]
```

Les paramètres seront utilisés dans la résolution des variables `$0`, `$1`... Par exemple :

```yaml
domain:
  name: DO_CODE
  csharp:
    annotations:
      - text: MyAnnotation("{$0}", "{$1}"))
```

Génèrera l'annotation `[MyAnnotation("Param1", "Param2")]` sur la propriété `MyProperty`. Si les paramètres ne sont pas renseignés, les variables `$0`, `$1` ne seront simplement pas remplacées. Et bien entendu, rien ne se passera si on passe des paramètres alors que le domaine ne les utilise pas.

### Transformations

Il est possible que la variable que vous utilisez dans votre template ne corresponde pas tout à fait à votre besoin. TopModel gère l'ajout de `transformateurs` sur les templates. Vous pouvez ajouter un `transformateur` après le nom de la variable que vous référencez, précédé de `:`. Le code généré tiendra compte de cette transformation.

Exemple :

```yaml
domain:
  name: DO_ID
  label: Identifiant
  java:
    type: Integer
    annotations:
      - text: '@Label("{label:lower}")'
        imports:
          - topmodel.sample.custom.annotation.Label
```

Actuellement, voici les transformations gérées par `TopModel` :

| nom          | résultat      |
| ------------ | ------------- |
| `kebab`      | kebab-case    |
| `snake_case` | snake_case    |
| `constant`   | CONSTANT_CASE |
| `camel`      | camelCase     |
| `pascal`     | PascalCase    |
| `lower`      | lowercase     |
| `upper`      | UPPERCASE     |

## Spécialisation des annotations

Les annotations peuvent être spécialisées selon la cible de la génération. Il y a actuellement trois cibles possibles, qui sont composables. La propriété `target` de l'annotation peut donc prendre les valeurs suivantes :

- `Persisted`
- `Dto`
- `Persisted_Dto`
- `Api`
- `Api_Persisted`
- `Api_Dto`
- `Api_Dto_Persisted`

Ainsi, les annotations `Dto` ne seront ajoutées que pour les classes non persistées, les annotations `Persisted` ne seront ajoutées que pour les classes persistées etc. Par défaut, la valeur est `Persisted-Dto`.

Ex :

```yaml
domain:
  name: DO_ID
  label: Identifiant
  java:
    type: Integer
    annotations:
      - text: '@Label("{label:lower}")'
        imports:
          - topmodel.sample.custom.annotation.Label
        target: Dto
```
