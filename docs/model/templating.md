# Templating de variables

TopModel supporte du templating via l'utilisation de **variables** aux endroits suivants :

- La [configuration](/configuration.md#variables)
- Les [implémentations de domaines](/model/domains.md#templating)
- Les [implémentations de décorateurs](/model/decorators.md#templating)

Une variable s'écrit toujours entre accolades (`{variable}`), et les variables disponibles dépendent du contexte.

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
