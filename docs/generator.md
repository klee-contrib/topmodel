# Génération

**TopModel.Generator** (TMG) est le générateur de code basé sur `TopModel.Core` et l'application principale à travers laquelle vous pourrez valider et utiliser votre modélisation. Il s'utilise via la CLI **`modgen`**.

TMG dispose de 5 générateurs, qui seront instanciés si la section de configuration associée est renseignée dans le fichier de config (`topmodel.config`) du générateur. Chaque générateur se configure avec une liste de **tags**, pour indiquer à chaque générateur les fichiers de modèles qu'il doit lire. Par exemple, si mon modèle est divisé en "modèle persisté" et "modèle non persisté", je peux définir deux tags correspondants ("Data" et "DTO") que je vais associer aux fichiers concernés. Dans ma configuration de générateurs, je peux dire que le tag "Data" sera passé au générateur de SQL et de C#, tandis que le tag "DTO" sera passé au C# et au JS, ce qui permet de ne pas mélanger les choses.

Chaque générateur peut être spécifié **plusieurs fois**, ce qui peut permettre par exemple de générer du modèle dans plusieurs applications, en filtrant les classes/endpoints générés avec des tags.

Les générateurs disponibles sont :

- **Le générateur [C# (`csharp`)](/generator/csharp.md)**
- **Le générateur [JPA (`jpa`)](/generator/jpa.md)**
- **Le générateur [Javascript (`javascript`)](/generator/js.md)**
- **Le générateur [SQL (`sql`)](/generator/sql.md)**
- **Le générateur de [traductions manquantes (`translation`)](/generator/tranlsation.md)**

## Validation du fichier de configuration

Puisque `modgen` est un outil distinct de l'extension VS Code, cette dernière ne contient pas les définitions de configuration des générateurs. La validation par défaut qui sera faite par l'extension ne pourra donc pas vérifier (et auto-compléter) la configuration des différents générateurs (sections `csharp`, `jpa`, `javascript`...).

La commande **`modgen --schema`** (ou `modgen -s`) permet de générer le fichier de schéma JSON complet, à côté du fichier de configuration. Ce fichier pourra être ensuite référencé dans les paramètres de workspace de VSCode (`.vscode/settings.json`) dans la section `yaml.schemas` pour remplacer le schéma par défaut de l'extension.

La génération du schéma ainsi que l'ajout aux paramètres du workspace peuvent être réalisés via les commandes associées dans l'extension VS Code. La génération du schéma est par défaut réalisée après chaque mise à jour de `modgen` par l'extension (dans le workspace VS Code ouvert).

Vous pouvez choisir d'ajouter le fichier `topmodel.config.schema.json` à votre `.gitignore` ou non. Ce fichier pourrait être commité afin de pouvoir visualiser les évolutions de configuration de `modgen`, mais vous pouvez également choisir de l'ignorer.

## Générateurs personnalisés

Via le paramètre `generators` du fichier de config, **il est possible d'ajouter aux générateurs existants vos propres générateurs**. Il s'agit d'une liste de chemins vers **des projets C# de la même forme que les générateurs existants**. Vous pouvez vous inspirer des générateurs existants pour écrire le votre (une documentation plus détaillée sera réalisée dans le futur une fois qu'on aura travaillé sur la modularisation de `modgen`). En particulier, il faut :

- Prendre une dépendance à `TopModel.Generator.Core` (le package est publié sur NuGet et versionné comme `TopModel.Generator`)
- Implémenter une config en dérivant de `GeneratorConfigBase`. La section de configuration associée aura le nom de la classe de configuration en camelcase sans le suffixe `Config` (exemple : `MonLangageConfig` > `monLangage`). Le schéma JSON pour valider cette configuration doit également être fourni et s'appeler `{section}.config.json`.
- Implémenter `IGeneratorRegistration<TConfig>` pour enregistrer vos générateurs dans TopModel.

`TopModel.Generator.Core` inclus quelques classes abstraites pour vous aider à implémenter les types de générateurs suivants :

- `ClassGeneratorBase` (un fichier par classe)
- `ClassGroupGeneratorBase` (un fichier par groupe de classe, selon le nom du fichier généré)
- `EndpointsGeneratorBase` (un fichier par contrôleur/client d'API)
- `MapperGeneratorBase` (un fichier par module persisté/non persisté de mappers)
- `TranslationGeneratorBase` (un fichier par module pour les traductions)

Sinon, vous pouvez utiliser `GeneratorBase` si votre générateur n'entre dans aucune de ces catégories.

Bon courage !
