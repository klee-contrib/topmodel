# Documentation Generator

## Présentation

Le module de générateurs `documentation` produit des **documents Markdown** dérivés de votre modèle, utilisables tels quels dans un site statique (Docusaurus, MkDocs, GitLab/GitHub wiki, etc.) ou intégrés dans un export fonctionnel. Il a pour vocation de fournir une **documentation vivante** et systématiquement à jour du modèle, sans qu'il soit nécessaire de la maintenir à la main.

Il est composé de trois générateurs indépendants :

| Nom              | Objets ciblés                     | Fichiers générés                                                                                                                                                                                                                                                                                               |
| ---------------- | --------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `DocClassGen`    | Classes persistées non abstraites | Un ou plusieurs fichiers Markdown contenant le **dictionnaire de données** sous forme de tableau : une ligne par propriété de chaque table persistée. La granularité dépend des variables présentes dans `classesFilePath`.                                                                                    |
| `DocEndpointGen` | Endpoints                         | Un ou plusieurs fichiers Markdown listant **les endpoints** du modèle, avec leur module, leur verbe HTTP, leur route, leur description et éventuellement les rôles déduits des annotations configurées via `authorizationAnnotations`. La granularité dépend des variables présentes dans `endpointsFilePath`. |
| `DocMermaidGen`  | Classes persistées non abstraites | Un ou plusieurs fichiers Markdown contenant un **diagramme de classes Mermaid**. La granularité dépend des variables présentes dans `mermaidFilePath`.                                                                                                                                                         |

Chaque générateur est activé par défaut et peut être désactivé individuellement via la propriété [`disable`](#disable), en utilisant son **nom court** (`DocClassGen`, `DocEndpointGen`, `DocMermaidGen`).

Les fichiers produits sont du Markdown standard, le code Mermaid étant généré dans un bloc ` ```mermaid ` compatible Docusaurus, GitHub ou GitLab.

## Découpage des fichiers générés

Les trois générateurs partagent la même logique de découpage : la granularité est **déduite automatiquement** des variables présentes dans le chemin configuré (`classesFilePath`, `endpointsFilePath`, `mermaidFilePath`).

| Variables présentes                  | Mode     | Comportement                                                                          |
| ------------------------------------ | -------- | ------------------------------------------------------------------------------------- |
| `...{fileName}...`                   | `File`   | Un fichier **par fichier de modèle**. Le nom du fichier est transformé en kebab-case. |
| `...{module}...` (sans `{fileName}`) | `Module` | Un fichier **par module**. Le nom du module est transformé en kebab-case.             |
| Ni `{module}` ni `{fileName}`        | `All`    | Un **unique fichier global** contenant l'ensemble des objets concernés.               |

Le titre (niveau 1) des fichiers générés reflète automatiquement le mode choisi :

- mode `All` : titre de base (ex. « Dictionnaire de données », « Liste des endpoints », « Diagramme global »)
- mode `Module` : titre de base suivi de « du module _X_ »
- mode `File` : titre de base suivi de « du fichier _Y_ »

## Dictionnaire de données

Le générateur `DocumentationClassDocGenerator` (`DocClassGen`) produit un ou plusieurs fichiers (par défaut un unique `classes.md`) contenant un tableau listant, pour chaque classe persistée non abstraite :

- le schéma de base de données (déduit de la configuration `schemas`, voir plus bas) ;
- le nom SQL de la table ;
- pour chaque propriété : son nom logique, son nom SQL, son libellé, son type SQL, sa longueur, sa description, ses contraintes (clé primaire, clé étrangère), son caractère obligatoire et les éventuelles valeurs possibles (issues des `values`).

Les colonnes générées sont : `Schéma`, `Table`, `Désignation`, `Nom SQL`, `Libellé`, `Type SQL`, `Longueur`, `Description`, `Contraintes`, `Obligatoire`, `Valeurs possibles`.

Le nom du schéma et le nom de la table ne sont répétés que sur la première ligne d'une table (les cellules suivantes sont vides), ce qui donne un rendu visuel proche d'une table fusionnée dans les viewers Markdown qui le supportent.

Le découpage en plusieurs fichiers (par module ou par fichier de modèle) est piloté par les variables `{module}` / `{fileName}` de `classesFilePath` (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

> Le type est récupéré via l'implémentation du domain pour le langage du générateur (`sql` par défaut).

## Liste des endpoints

Le générateur `DocumentationEndpointDocGenerator` (`DocEndpointGen`) produit un ou plusieurs fichiers (par défaut un unique `endpoints.md`) contenant un tableau listant les endpoints du modèle, triés par module.

Les colonnes générées sont : `Module`, `Nom`, `Méthode`, `Route`, `Description`, et — de manière optionnelle — `Autorisation`.

Les colonnes `Méthode` et `Route` correspondent respectivement au verbe HTTP de l'endpoint et à sa route complète (préfixe d'endpoints du fichier de modèle inclus, le cas échéant).

La colonne `Module` n'est générée qu'en mode `All` (fichier unique global). En modes `Module` et `File`, tous les endpoints du tableau partagent le même module (rappelé dans le titre du fichier), la colonne devient donc redondante et n'est plus produite.

La colonne `Autorisation` n'est ajoutée au tableau que si la propriété `authorizationAnnotations` est renseignée. Elle est alors alimentée automatiquement à partir des paramètres des annotations listées (par exemple `RoleRequired`, `HasRole`…) portées par les endpoints. Si `authorizationAnnotations` est vide ou absent, la colonne n'est pas générée.

Le découpage en plusieurs fichiers (par module ou par fichier de modèle) est piloté par les variables `{module}` / `{fileName}` de `endpointsFilePath` (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

## Diagrammes Mermaid

Le générateur `DocumentationMermaidGenerator` (`DocMermaidGen`) produit un ou plusieurs fichiers Markdown contenant un diagramme de classes Mermaid, généré pour l'ensemble des classes persistées non abstraites.

Le découpage en plusieurs fichiers (par module — mode par défaut — ou par fichier de modèle) est piloté par les variables `{module}` / `{fileName}` de `mermaidFilePath` (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

## Configuration

### Fichier de configuration

- `outputDirectory`

  Racine du répertoire de génération (comme pour tous les autres générateurs). **Obligatoire**.

- `tags`

  Tags de fichier que le générateur doit lire. **Obligatoire**.

- `endpointsFilePath`

  Chemin de génération du fichier listant les endpoints, relatif au répertoire de génération. La finesse de génération est déduite des variables présentes (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

  _Templating_ : `{module}`, `{fileName}`

  _Valeur par défaut_ : `endpoints.md`

  _Variables par tag_ : **oui**

- `classesFilePath`

  Chemin de génération du fichier de dictionnaire des classes, relatif au répertoire de génération. La finesse de génération est déduite des variables présentes (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

  _Templating_ : `{module}`, `{fileName}`

  _Valeur par défaut_ : `classes.md`

  _Variables par tag_ : **oui**

- `mermaidFilePath`

  Chemin de génération des fichiers de diagrammes Mermaid, relatif au répertoire de génération. La finesse de génération est déduite des variables présentes (voir [Découpage des fichiers générés](#découpage-des-fichiers-générés)).

  _Templating_ : `{module}`, `{fileName}`

  _Valeur par défaut_ : `{module}.md`

  _Variables par tag_ : **oui**

- `authorizationAnnotations`

  Liste des **noms d'annotations** (par exemple `RoleRequired`, `HasRole`…) dont les paramètres doivent être agrégés pour alimenter la colonne `Autorisation` du tableau des endpoints.

  Si la liste est vide ou non renseignée, la colonne `Autorisation` **n'est pas générée** dans le tableau.

  Pour chaque endpoint, toutes les annotations dont le nom figure dans cette liste sont collectées, et leurs paramètres sont concaténés (séparés par `,`) dans la cellule `Autorisation`.

  Exemple :

  ```yaml
  authorizationAnnotations:
    - RoleRequired
    - HasRole
  ```

- `schemas`

  Correspondance entre les **schémas/pods de base de données** et les tags qui y sont persistés. Permet de renseigner la colonne `Schéma` du dictionnaire des classes en fonction du tag du fichier courant.

  Par exemple, la configuration suivante indique que les classes dont les fichiers sont taggués `back` ou `batch` sont persistées dans le schéma `public`, tandis que les classes taggées `archive` sont persistées dans le schéma `archive` :

  ```yaml
  schemas:
    public:
      - back
      - batch
    archive:
      - archive
  ```

- `disable`

  Permet de désactiver spécifiquement certains générateurs du module. Les valeurs attendues sont les **noms courts** des générateurs :
  - `DocClassGen` — désactive `DocumentationClassDocGenerator` (dictionnaire de données)
  - `DocEndpointGen` — désactive `DocumentationEndpointDocGenerator` (liste des endpoints)
  - `DocMermaidGen` — désactive `DocumentationMermaidGenerator` (diagrammes Mermaid)

### Exemple

Configuration typique pour générer, dans un site Docusaurus, un dictionnaire de données global, une liste d'endpoints globale et un diagramme de classes par module :

```yaml
documentation:
  - tags:
      - back
    outputDirectory: ./docs/generated
    classesFilePath: dictionnaire/{tag}.md
    endpointsFilePath: endpoints/{tag}.md
    mermaidFilePath: diagrammes/{module}.md
    schemas:
      public:
        - back
        - batch
```

Pour ne générer qu'un diagramme global (sans découpage par module) :

```yaml
documentation:
  - tags:
      - back
    outputDirectory: ./docs/generated
    mermaidFilePath: diagramme-global.md
    disable:
      - DocClassGen
      - DocEndpointGen
```

Pour un découpage fin par fichier de modèle, sur les trois générateurs :

```yaml
documentation:
  - tags:
      - back
    outputDirectory: ./docs/generated
    classesFilePath: dictionnaire/{module}/{fileName}.md
    endpointsFilePath: endpoints/{module}/{fileName}.md
    mermaidFilePath: diagrammes/{module}/{fileName}.md
```
