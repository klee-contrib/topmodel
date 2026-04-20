# Templating

TopModel supporte du templating via l'utilisation de **variables** aux endroits suivants :

- La [configuration](/configuration.md#variables)
- Les [implémentations d'annotations](/model/annotations)
- Les [implémentations de domaines](/model/domains)
- Les [implémentations de décorateurs](/model/decorators)

Une variable s'écrit toujours entre accolades (`{variable}`), et les variables disponibles dépendent du contexte.

## Variables

Il est possible que certaines propriétés d'une annotation, d'un domaine ou d'un décorateur dépendent de l'objet sur lequel vous l'ajoutez. Vous pourriez par exemple, sur un décorateur, ajouter une interface `java` générique de la classe sur laquelle est ajouté le décorateur.

```java
/**
 * Utilisateur de l'application.
 */
@Generated("TopModel : https://github.com/klee-contrib/topmodel")
@Entity
@Table(name = "UTILISATEUR")
@EntityListeners(AuditingEntityListener.class)
public class Utilisateur implements MonInterface<Utilisateur> {
}
```

Ici nous pourrions écrire un décorateur `MonInterfaceUtilisateur` :

```yaml
decorator:
  name: MonInterfaceUtilisateur
  description: Implémente MonInterface pour la classe utilisateur
  java:
    annotations:
      - MonInterface<Utilisateur>
```

Cela pose un problème évident : ce décorateur n'est utilisable que pour cette classe là.
Mais TopModel permet de gérer ce type de cas, et de créer des décorateurs plus génériques avec des `templates`.

TopModel peut remplacer la chaîne de caractère `{name}` par le nom de la classe sur lequel est ajouté le générateur. Ainsi :

```yaml
decorator:
  name: MonInterface
  description: Implémente MonInterface pour la classe sur laquelle ce décorateur est ajouté
  java:
    implements:
      - MonInterface<{name}>
```

Permet de généraliser le comportement du décorateur `MonInterface` à toutes les classes qui voudraient l'utiliser.

### Variables disponibles dans tous les cas

- Les paramètres de l'annotation, du décorateur ou du domaine.
- N'importe quelle variable définie dans la configuration (dans `variables` ou `tagVariables`).

### Variables disponibles pour une classe

Actuellement, il est possible d'utiliser ces variables pour une classe (dans les annotations et les décorateurs de classe) :

- `trigram`
- `name`
- `sqlName`
- `comment`
- `label`
- `pluralName`
- `module`
- `customProperties.*`
- `primaryKey.*` permet d'accéder à toutes les variables accessibles dans les templates de propriété, pour la clé primaire de la classe qui fait l'objet du décorateur
- `properties[i]` permet d'accéder à toutes les variables accessibles dans les templates de propriété, pour la ième propriété de la classe qui fait l'objet du décorateur
- `extends` permet d'accéder à toutes les variables accessibles dans les templates de calsse, pour la classe parente de la classe qui fait l'objet du décorateur

### Variables disponibles pour un endpoint

Actuellement, il est possible d'utiliser ces variables pour un endpoint (dans les annotations et les décorateurs d'endpoint) :

- `name`
- `description`
- `route`
- `method`
- `module`
- `customProperties.*`
- `returns.*` permet d'accéder à toutes les variables accessibles dans les templates de propriété, pour la propriété définie dans le `returns` du endpoint
- `params[i].*` permet d'accéder à toutes les variables accessibles dans les templates de propriété, pour le ième param du endpoint qui fait l'objet du décorateur

### Variables disponibles pour une propriété

Actuellement, il est possible d'utiliser ces variables pour une propriété (dans les annotations de propriétés et les domaines) :

- `name`
- `trigram`
- `label`
- `comment`
- `required`
- `resourceKey`
- `defaultValue`
- `customProperties.*`
- `class.*` ou `parent.*` ou `endpoint.*` : permet d'accéder à toutes les variables accessibles dans les templates de classe, pour l'objet parent de la propriété (la classe ou le endpoint)
- `domain.*` permet d'accéder à toutes les variables accessibles dans les templates de domaine, pour le domain de la propriété (plus utile pour les décorateurs...)

Dans le cadre d'une composition, il est possible d'utiliser ces variables :

- `name`
- `label`
- `comment`
- `composition.*` : permet d'accéder à toutes les variables accessibles dans les templates de classe, pour la classe qui fait l'objet de la composition

## Transformations

Il est possible que la variable que vous utilisez dans votre template ne corresponde pas tout à fait à votre besoin. TopModel gère l'ajout de `transformateurs` sur les templates. Vous pouvez ajouter un `transformateur` après le nom de la variable que vous référencez, précédé de `:`. Le code généré tiendra compte de cette transformation.

Exemple :

```yaml
decorator:
  name: MonInterface
  description: Implémente MonInterface pour la classe sur laquelle ce décorateur est ajouté
  java:
    annotations:
      - @Label(\"{name:lower}\")
```

Actuellement, voici les transformations gérées par `TopModel` :

| nom        | résultat                        |
| ---------- | ------------------------------- |
| `kebab`    | kebab-case                      |
| `snake`    | snake_case                      |
| `constant` | CONSTANT_CASE                   |
| `camel`    | camelCase                       |
| `pascal`   | PascalCase                      |
| `lower`    | lowercase                       |
| `upper`    | UPPERCASE                       |
| `path`     | `My.Module` => `My/Module`      |
| `flat`     | `My.Module` => `MyModule`       |
| `head`     | `My.Module` => `My`             |
| `last`     | `My.Top.Module` => `Module`     |
| `tail`     | `My.Top.Module` => `Top.Module` |

Ces transformations peuvent être **chainées**, en accolant plusieurs transformations à la suite des autres :

Exemples :

- `{variable:tail:path:upper}` pour `My.Top.Module` donnera `TOP/MODULE`
- `{variable:path:snake}` pour `My.TopModule` donnera `my/top-module`

_Remarque : Les transformations de casse (`kebab`, `camel`, `constant`...) sont appliquées sur chaque section de la valeur délimitée par un `.`, un `/` ou un `\`, ce qui permet notamment de gérer correctement le deuxième exemple._
