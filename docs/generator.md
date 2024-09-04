# Génération

**TopModel.Generator** est le générateur de code basé sur `TopModel.Core` et l'application principale à travers laquelle vous pourrez valider et utiliser votre modélisation. Il s'utilise via la CLI **`modgen`**.

Depuis la version 2.0, **`modgen` n'inclus plus aucun générateurs par défaut**. TopModel maintient et publie les 5 modules de générateurs suivants, qui seront automatiquement installés si vous les renseignez dans votre [fichier de configuration `topmodel.config`](/configuration.md) :

- **Le module de générateurs [C# (`csharp`)](/generator/csharp.md)**
- **Le module de générateurs [JPA (`jpa`)](/generator/jpa.md)**
- **Le module de générateurs [Javascript (`javascript`)](/generator/js.md)**
- **Le module de générateurs [SQL (`sql`)](/generator/sql.md)**
- **Le module de générateurs [traductions manquantes (`translation`)](/generator/tranlsation.md)**

Ces modules sont publiés sur NuGet (comme toute librairie .NET) sous le nom `TopModel.Generator.{module}`. En théorie, `modgen` n'est pas limité à ces 5 modules là, et si quelqu'un d'autre publiait un module `TopModel.Generator.Brainfuck` par exemple, il serait automatiquement installé si une config `brainfuck` était renseignée dans la configuration.

Au premier lancement, `modgen` installera la dernière version de chaque module, dans le répertoire `.modgen` (à ajouter dans votre `.gitignore`). Les versions de modules installées seront ensuite renseignées dans le fichier `topmodel.lock`, à côté de la version de TopModel utilisée pour la dernière génération ainsi que la liste des fichiers générés. Pour les installations suivantes, les versions installées seront celles listées dans le ficher `topmodel.lock`. La commande `modgen --update csharp` ou `modgen --update all` permettra de forcer la mise à jour d'un ou tous les modules vers leurs dernières versions (vous pouvez aussi modifier le fichier manuellement si vous voulez une version précise).

## Validation du fichier de configuration

Puisque `modgen` est un outil distinct de l'extension VS Code, cette dernière ne contient pas les définitions de configuration des générateurs. La validation par défaut qui sera faite par l'extension ne pourra donc pas vérifier (et auto-compléter) la configuration des différents générateurs (sections `csharp`, `jpa`, `javascript`...).

La commande **`modgen --schema`** (ou `modgen -s`) permet de générer le fichier de schéma JSON complet, à côté du fichier de configuration, et mettra à jour ce dernier pour y inclure une référence vers le schéma, pour que VS Code puisse proposer la complétion et la validation.

Cette commande sera également automatiquement lancée après une installation de module.

Vous pouvez choisir d'ajouter le fichier `topmodel.config.schema.json` à votre `.gitignore` ou non. Ce fichier pourrait être commité afin de pouvoir visualiser les évolutions de configuration de `modgen`, mais vous pouvez également choisir de l'ignorer.

## Générateurs personnalisés

Via le paramètre `generators` du fichier de config, **il est possible de renseigner vos propres modules de générateurs**. Il s'agit d'une liste de chemins vers **des projets C# de même nature que les modules existants**. En particulier, un module de générateurs doit :

- Prendre une dépendance à `TopModel.Generator.Core` (le package est publié sur NuGet et versionné comme `TopModel.Generator`)
- Implémenter une config en dérivant de `GeneratorConfigBase`. La section de configuration associée aura le nom de la classe de configuration en minuscules sans le suffixe `Config` (exemple : `BrainfuckConfig` > `brainfuck`). Le schéma JSON pour valider cette configuration doit également être fourni et s'appeler `{section}.config.json`.
- Implémenter `IGeneratorRegistration<TConfig>` pour enregistrer vos générateurs dans TopModel.

Vous pouvez vous inspirer des générateurs existants pour écrire le votre (une documentation plus détaillée sera réalisée dans le futur...). En particulier, `TopModel.Generator.Core` inclus quelques classes abstraites pour vous aider à implémenter les types de générateurs suivants :

- `ClassGeneratorBase` (un fichier par classe)
- `ClassGroupGeneratorBase` (un fichier par groupe de classe, selon le nom du fichier généré)
- `EndpointsGeneratorBase` (un fichier par contrôleur/client d'API)
- `MapperGeneratorBase` (un fichier par module persisté/non persisté de mappers)
- `TranslationGeneratorBase` (un fichier par module pour les traductions)
- Sinon, vous pouvez utiliser `GeneratorBase` si votre générateur n'entre dans aucune de ces catégories.

Vous pouvez également prendre une dépendance à un module existant (par exemple `TopModel.Generator.Csharp`), dans le cas ou vous souhaitez réutiliser, surcharger, ou compléter un module existant. Dans ce cas :

- La version du module référencée dans votre module sera automatiquement celle utilisée par `modgen` (ce sera la version renseignée dans le `topmodel.lock` quoi qu'il arrive)
- Si vous y définissez une `IGeneratorRegistration` pour la même config que le module que vous référencez, alors cette enregistrement **remplacera** celui du module existant, qui ne sera pas enregistré. De fait, vous remplacerez donc l'entièreté du module par le votre. Rien ne vous empêche d'y réenregistrer certains générateurs existants, en plus de ceux que vous remplacerez ou retirerez.

Si vous incluez des dépendances autres que `TopModel.Generator.Core` dans votre générateur personnalisé (y compris une dépendance à un module TopModel existant), il vous faudra utiliser l'option `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` pour que TopModel les charge bien toutes avec votre module.

Enfin, il est indispensable de **build votre projet au préalable** avant de lancer la commande `modgen`.

Bon courage !
