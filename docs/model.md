# Modélisation

Un modèle TopModel est divisé en **fichiers de modèle YAML**. Tous les fichiers sont équivalents, dans le sens où il n'y a pas de convention imposée par l'outil pour leur organisation ou dans ce qu'ils doivent contenir. L'organisation de votre hiérarchie de fichiers est donc **totalement libre**, même si bien entendu il est souhaitable de mettre en place un système clair qui fonctionne pour votre modèle.

## Structure d'un fichier de modèle

Un fichier de modèle contient **une suite de documents YAML** (à l'inverse du JSON, il est possible d'avoir plusieurs documents dans un même fichier en YAML. Le séparateur de document est `---`).

### Descripteur du fichier

Le premier document YAML d'un fichier de modèle doit être le **descripteur du fichier**. Il doit impérativement se composer de :

- Une propriété **`module`**, qui sert à identifier à quel module applicatif vont se rattacher les classes définies dans le fichier. Ce module pourra être utilisé par les différents générateurs pour déterminer à quel endroit chaque classe devra être générée par exemple.
- Une propriété **`tags`**, qui est une liste de noms arbitraires. Les tags seront utilisés dans la configuration des générateurs : ils permettront de définir quel générateur devra traiter quel fichier (chaque générateur aura donc aussi sa liste de tags).

Ce sont donc à priori le module et les tags qui vont dicter l'organisation de vos fichiers de modèles. Vous allez vouloir regrouper vos différentes classes et endpoints dans des groupements logiques qui correspondent à la fois à votre métier et à la distinction technique qu'il pourrait y avoir entre les différents objets (par exemple, ce qui est persisté et ce qui ne l'est pas).

### Dépendances entre fichiers

De plus, si **des classes d'un fichier ont besoin de référencer des classes d'un autre fichier**, alors il faudra également **spécifier les dépendances aux autres fichiers**, via la propriété supplémentaire **`uses`**. Il s'agit d'une liste de noms de fichiers, sans l'extension. Un nom de fichier est déterminé comme étant **le chemin du fichier relatif à la racine du modèle**.

Exemple :

```yaml
---
module: Referentiel
tags:
  - Data
  - Server
uses:
  - Referentiel/Data/File_01
```

### Types de documents

Il n'y a pas de contrainte d'ordre sur le reste des documents YAML suivants. Ils peuvent être de **6 types** différents :

- **[Un domaine](/model/domains.md)** : Types métier réutilisables avec leurs représentations par langage
- **[Une classe](/model/classes.md)** : Entités métier, DTOs, classes persistées ou non
- **[Un endpoint](/model/endpoints.md)** : Définition des API REST
- **[Un décorateur](/model/decorators.md)** : Métadonnées réutilisables
- **[Un convertisseur](/model/mappers.md#converters)** : Transformations de types
- **[Un flux de données](/model/dataFlows.md)** : Définition des flux de traitement

## Organisation recommandée

Bien que l'organisation soit libre, voici quelques recommandations :

- **Regrouper les domaines** : Il est fortement conseillé de définir tous les domaines dans un unique fichier qui ne contient que ces définitions, pour éviter les dépendances circulaires.
- **Organiser par module** : Regrouper les classes et endpoints par module applicatif.
- **Séparer par tags** : Utiliser les tags pour distinguer les classes persistées des DTOs, ou pour séparer les différents contextes applicatifs.

Pour plus de détails sur chaque type de document, consultez les pages dédiées ci-dessus.
