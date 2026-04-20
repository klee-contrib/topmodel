# Domaines

Un domaine se définit comme un document YAML, dans un fichier de modèle.

Un domaine correspond à un type métier. Chaque champ doit avoir un domaine. Les règles de gestion liées à chaque domaine devront être implémentées dans chaque couche technique. En revanche, il faut décrire ici, dans le modèle, comment chaque domaine va être représenté dans chaque langage, puisque la génération va en avoir besoin.

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
  annotations: # Liste d'annotations à devoir ajouter à toute propriété de ce domaine.
  csharp:
    type: int?
    imports: # Liste d'imports à devoir ajouter à la définition d'une classe qui utilise ce domaine.
  ts:
    type: number
    imports: # Liste d'imports à devoir ajouter à la définition d'une classe qui utilise ce domaine.
  java:
    type: Integer
    imports: # Liste d'imports à devoir ajouter à la définition d'une classe qui utilise ce domaine.
  sql:
    type: int
```

Il n'y a pas besoin de préciser les dépendances aux fichiers contenant des domaines dans `uses` : tous les domaines sont automatiquement accessibles dans tous les fichiers. En revanche, cela implique que tous les fichiers ont une dépendance implicite à tous les fichiers contenant des domaines, ce qui pourrait entraîner des dépendances circulaires involontaires entre fichiers. Par conséquent, et également par soucis de clarté, **il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions**.

Toutes les définitions de langage suivent un format standard, quel que soit le langage utilisé. Chaque configuration de générateur sélectionne une implémentation de domaine adaptée à son langage, comme `csharp`, `java`, `ts`, `sql`, etc. Bien que chaque générateur ait un langage par défaut, il est possible de le modifier en utilisant l'attribut `language` dans la configuration du générateur. Cet attribut accepte une liste de valeurs, permettant ainsi de définir un ordre de priorité pour les langages d'implémentation. Par exemple, si l'attribut `language` est défini comme `[java21, java8]`, le générateur utilisera `java21` s'il est disponible pour le domaine ; sinon, il utilisera `java8`.

Naturellement, il n'est pas nécessaire de spécifier les langages pour lesquels le domaine n'est pas utilisé (et c'est évidemment obligatoire sinon).

Un domaine peut recevoir des [annotations](/model/annotations), qui seront posées sur toutes les propriétés de ce domaine.

Il est possible de définir le `mediaType` du domaine. Cette information pourra être prise en compte par certains générateurs (notamment les générateurs d'API).

Un domaine peut également définir des `asDomains`, qui sont des domaines à utiliser lorsque l'on a besoin de transformer le domaine d'une propriété que l'on référence. Il peut être utilisé dans un alias via `as`, et les [associations multiples](/model/properties#associations-multiples-et-réciproques) ont besoin d'avoir un `asDomain` `list` défini sur le domaine de la clé primaire pour être utilisées (puisqu'il y a une transformation de son type a réaliser).

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
- Lorsque le domaine est utilisé dans une **transformation de domaine** (via `as`, donc les cas décrits dans le premier point de cette PR). Si `genericType` n'est pas renseigné dans ce cas, elle vaudra par défaut la valeur de `type`, ce qui veut dire qu'il faut nécessairement spécifier la transformation pour qu'elle soit réalisée. Auparavant, cela n'existait que pour `asList: true` (et les associations multiples), et chaque générateur implémentait en dur la transformation à réaliser sur le type original (impossible à débrancher, et souvent en ajoutant `"[]"` à la fin ou en mettant `List<>` autour). **`{T}` référencera ici le type original** (qui peut être une enum ou une association par exemple).

Par exemple, le domaine `DO_ID_LIST` référencé dans l'exemple précédent comme `asDomain` de `DO_ID` devrait être défini ainsi :

```yaml
---
domain:
  name: DO_ID_LIST
  label: Code
  ts:
    type: number[]
    genericType: "{T}[]"
  csharp:
    type: int[]
    genericType: "{T}[]"
  java:
    type: List<int>
    genericType: List<{T}>
    imports:
      - java.util.List
```

## Templating et paramètres

Comme pour les décorateurs et les annotations, il est possible d'utiliser du [templating](/model/templating) dans les annotations, et de définir des paramètres :

```yaml
domain:
  name: DO_CODE
  parameters:
    - name: param1
      required: true
      comment: Premier paramètre.
    - name: param2
      defaultValue: Test
      comment: Deuxième paramètre.
  csharp:
    type: GenericType<{param1}, {param2}>
```

Ces paramètres pourront être passés lorsqu'on associe un domaine à une propriété, en passant un objet `{name, parameters}` au lieu du nom du domaine :

```yaml
properties:
  - name: MyProperty
    domain:
      name: DO_CODE
      parameters:
        param1: Type1
        param2: Type2
```

Tous les paramètres passés doivent être définis au préalable sur le domaine. Les paramètres obligatoires doivent être renseignés avec le domaine, et les paramètres non renseignés le seront avec leur `defaultValue` (qui vaut `""` si non renseignée).

Les variables (en) et paramètres sont utilisables :

- Dans les paramètres d'annotations
- Dans les propriétés d'implémentations suivantes :
  - `type`
  - `genericType` (qui autorise aussi `{T}` comme vu précédemment)
  - `imports`

## Templates de valeurs

Puisqu'il est possible de renseigner dans la modélisation des valeurs pour certaines propriétés (via des [valeurs par défaut](./properties#valeurs-par-défaut) ou des [valeurs de classe](./classes#valeurs-dune-classe)), il faudra parfois spécifier pour certaines implémentations comment cette valeur doit être générée pour le language cible. Cela peut se définir via l'objet `values` dans une implémentation de domaine, de la façon suivante :

```yaml
domain:
  name: DO_DATE
  label: Date
  csharp:
    type: DateTime?
    values:
      template: DateTime.Parse("{value}")
  ts:
    type: string
```

La variable `{value}` référence ici la valeur renseignée pour la propriété dans le modèle, par exemple pour la propriété suivante :

```yaml
name: DateCreation
domain: DO_DATE
defaultValue: "2023-01-01"
comment: Date de création.
```

Toutes les autres variables de templating décrites précédemment sont également disponibles dans ces templates.

De plus, il est également possible de définir des surcharges de ces templates pour des valeurs précises. Pour l'exemple précédent, on préférerait certainement que la valeur par défaut soit égale à la date du jour plutôt qu'à une date précise. Dans ce cas, on pourrait y affecter la valeur `now`, et définir une surcharge pour `now` de la façon suivante :

```yaml
csharp:
  type: DateTime?
  values:
    template: DateTime.Parse("{value}")
    overrides:
      now: DateTime.UtcNow
sql:
  type: timestamptz
  values: # Pas besoin de template par défaut en SQL puisque la conversion string > date est automatique.
    overrides:
      now: now()
```

Enfin, il est possible que certaines valeurs ait besoin d'imports supplémentaires pour être utilisées. Pour répondre à ce besoin, il est possible de renseigner un objet `{value, imports}` à la place du template, par exemple pour le même domaine date :

```yaml
js:
  type: string
  values:
    overrides:
      now:
        value: today()
        imports:
          - ./common/utils/today # Cet import ne sera donc ajouté que dans les fichiers qui utilisent cette valeur.
```
