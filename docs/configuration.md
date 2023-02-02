# Configuration <!-- {docsify-ignore-all} -->

Pour démarrer votre projet TopModel, vous devez d'abord écrire un fichier de configuration. Celui-ci contient notamment :

- Le nom de l'application
- Le répertoire racine des fichiers de modèle
- La configuration des générateurs
- Un système de filtre (tags) pour la sélection des générateurs par langage sur lesquels TopModel sera utilisé.

Exemple :

```yaml
# topmodel.config
---
app: Hello World
```

Le fichier de configuration doit s'appeler `topmodel.config` ou `topmodel.[NOM DE L'APPLICATION].config`.

## Générateurs

Chaque générateur possède sa propre configuration. Néanmoins, la structure globale reste identique pour tous.

1. Commencer par le nom du générateur
2. Ajouter ensuite une liste de configurations
3. Chaque configuration **doit** préciser une liste de `tags`
4. Chaque configuration **doit** préciser le `outputDirectory`
5. Chaque configuration **peut** préciser des variables **globales**
6. Chaque configuration **peut** préciser des variables **par `tag`**.

Exemple :

```yaml
javascript:
  - tags:
      - Interne
      - Externe
      - Common
    variables:      
      root: ../sources/front
    outputDirectory: "{root}/src"
    domainPath: common/domains
```

## Variables

### Variables globales (à une configuration)

Lorsqu'une variable est ajoutée à une configuration, elle peut être utilisée dans les autres paramètres de cette même configuration. A la génération, les `template` dans les différents paramètres de configuration seront résolus à l'aide des variables déclarées. Dans l'exemple précédent, le paramètre  `"{root}/src"` sera résolu en `../sources/front/src`.

Les variables sont nécessairement des strings et ne peuvent donc être utilisées que dans des paramètres de type string. Il n'est pas exclu par la suite de gérer par la suite les nombres et les booléens, mais pour rester simple dans un premier temps ils ont été exclus.

Les variables peuvent faire l'objet de transformations (`:upper`, `:pascal`, `:camel`...) de la même façon que les variables des templates dans les domaines et décorateurs. Par exemple `{root:lower}`.

### Variables par tag

Un générateur peut choisir d'implémenter des variables qui ont des valeurs différentes **selon le tag** du fichier. De ce fait, un fichier ayant plusieurs tags résultant en des valeurs de paramètres différentes sera à priori généré plusieurs fois pour correspondre à chacune des valeurs possibles.

Seul le générateur `javascript` implémente cette fonctionnalité pour l'instant. En voici un exemple :

```yaml
javascript:
  - tags:
      - Interne
      - Externe
      - Common
    variables:      
      root: ../sources/front
    tagVariables:
      Externe:
        tag: externe
      Common:
        tag: common
      Interne:
        tag: interne
    outputDirectory: "{root}/src"
    modelRootPath: "{tag}/model"
    resourceRootPath: "{tag}/locale"
    apiClientRootPath: "{tag}/services"
    fetchPath: "{tag}/server"
    domainPath: common/domains
```

`tagVariables` permet pour un tag de définir une liste de variables qui ne seront appliquées que lors de la génération de fichiers avec ce tag-là. Toutes les propriétés du générateur ne supportent pas les variables par tag. Pour le générateur `javascript`, il s'agit de `modelRootPath`, `apiClientRootPath`, `resourceRootPath`, `fetchPath` et `domainPath`.

Une variable **globale** peut être **surchargée** par une variable par tag, mais dans ce cas cette variable devient une variable par tag (et par conséquent n'est plus supportée dans les champs qui ne peuvent pas avoir de variables par tag). Une variable par tag existe dès lors qu'elle est définie par au moins un tag, et si un tag ne renseigne pas une variable par tag définie dans un autre tag, alors sa valeur pour ce tag sera automatiquement renseignée à `""` (ce qui effacera effectivement la variable lors de la résolution de valeur du paramètre).

`modgen` affichera un warning s'il trouve une variable non définie dans un paramètre (et précisera qu'il cherche une variable globale dans un paramètre qui ne supporte pas les variables par tag). Les variables non définies seront gardées telles quelles dans la génération.

Toutes ces variables s'ajoutent aux variables prédéfinies `{app}`, `{module}` et `{lang}` qui sont utilisées dans certains paramètres. Idéalement, il ne faudrait pas les redéfinir (elles sont renseignées d'une manière qu'il n'est pas possible de reproduire avec la solution présentée), mais en théorie, puisque les variables de générateurs sont résolues en premier, une redéfinition agirait comme une surcharge.
