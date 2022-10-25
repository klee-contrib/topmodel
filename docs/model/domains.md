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
  csharp:
    type: int?
    annotations: # Liste d'annotations à devoir ajouter à toute propriété de ce domaine, si applicable.
    usings: # Liste de usings à devoir ajouter à la définition d'une classe qui utilise ce domaine, si applicable.
  ts:
    type: number
    import: # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  java:
    type: Integer
    import: # Chemin de l'import à ajouter pour utiliser le type, si applicable.
  sqlType: int
```

Naturellement, il n'est pas nécessaire de spécifier les langages pour lesquels le domaine n'est pas utilisé (et c'est évidemment obligatoire sinon).

Il n'y a pas besoin de préciser les dépendances aux fichiers contenant des domaines dans `uses` : tous les domaines sont automatiquement accessibles dans tous les fichiers. En revanche, cela implique que tous les fichiers ont une dépendance implicite à tous les fichiers contenant des domaines, ce qui pourrait entraîner des dépendances circulaires entre fichiers (qui ne sont **pas** supportées) involontaires. Par conséquent, et également par soucis de clarté, **il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions**.

Il est possible de définir le `mediaType` du domaine. Cette information pourra être prise en compte par certains générateurs (notamment les générateurs d'API).

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

Le tout dans ces propriétés :

- `java.type`
- `java.annotations`
- `java.imports`
- `csharp.type`
- `csharp.annotations`
- `csharp.usings`
- `ts.type`
- `ts.import`

Les templates des domaines des propriétés sont également valorisés.

## Transformation

Il est possible que la variable que vous utilisez dans votre template ne corresponde pas tout à fait à votre besoin. TopModel gère l'ajout de `transformateurs` sur les templates. Vous pouvez ajouter un `transformateur` après le nom de la variable que vous référencez, précédé de `:`. Le code généré tiendra compte de cette transformation.

Exemple :

```yaml
domain:
  name: DO_ID
  label: Identifiant
  java:
    type: Integer
    annotations:
      - text: "@Label(\"{label:lower}\")"
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

## Spécialisation

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
      - text: "@Label(\"{label:lower}\")"
        imports:
          - topmodel.sample.custom.annotation.Label
        target: Dto
```
