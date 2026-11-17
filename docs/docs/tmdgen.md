# Migration d'un modèle externe

**TopModel.ModelGenerator** (`tmdgen`) est un outil de migration de modèle, permettant de générer des fichiers `.tmd` à partir d'une source externe (`openApi` ou `database`) dans un modèle `TopModel` existant.

## Installation

L'installation de l'outil `tmdgen` se fait avec la commande :

```bash
dotnet tool install --global TopModel.ModelGenerator
```

Pour mettre à jour l'outil :

```bash
dotnet tool update --global TopModel.ModelGenerator
```

## Configuration globale

La configuration de l'outil se fait dans un fichier au format `tmdgen*.config`. La configuration globale nécessite une seule propriété `modelRoot`, qui doit contenir le chemin relatif vers la racine du modèle `TopModel` des fichiers générés (pour écrire correctement les imports).

Puis, vous pouvez définir sous la propriété `database` une liste de sources de base de données, et sous la propriété `openapi`, une liste de sources `openapi`.

### Exemple de configuration

```yaml
# tmdgen.config
---
modelRoot: ./model

database:
  - name: ma_base
    connectionString: "Server=localhost;Database=MaBase;..."
    # ... autres options

openapi:
  - name: mon_api
    url: "https://api.example.com/openapi.json"
    # ... autres options
```

Pour plus de détails sur la configuration de chaque source, consultez les pages dédiées :

- [Migration depuis une base de données](./tmdgen/database)
- [Migration depuis OpenAPI](./tmdgen/openapi)

## Correspondance des domaines

Pour chaque source, la propriété `domains` associe les propriétés (ou colonnes) de la source à un domaine du modèle. Chaque entrée contient un `domain` et un critère `name` (nom de la propriété) ou `type` (type de la source).

### Format des expressions régulières

Par défaut, `name` et `type` sont comparés en valeur exacte (insensible à la casse). Pour utiliser une expression régulière, entourez la valeur de `/` : `/motif/`. Le `/` de début et le `/` de fin sont retirés, le reste est passé tel quel à l'implémentation [.NET `Regex`](https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-language-quick-reference).

- La recherche n'est **pas ancrée** : `/id/` correspond à `userid`, `idx`, etc. Utilisez `^` et `$` pour ancrer le motif.
- La recherche est **sensible à la casse**, sauf avec l'option `(?i)` en début de motif. Pensez-y pour les colonnes de base de données, souvent en majuscules (`USER_ID`) ou en snake_case (`user_id`).
- Le fichier de configuration étant du YAML, entourez toujours les motifs de **quotes simples** (`'/\d+$/'`) :
  - en double quotes, `\d` est une séquence d'échappement invalide ;
  - sans quotes, un `#` précédé d'un espace est interprété comme le début d'un commentaire, et un `:` suivi d'un espace casse la valeur, sans erreur explicite.

### Exemples

```yaml
domains:
  # Nom exact (insensible à la casse)
  - name: email
    domain: DO_EMAIL
  # Finit par "Id" (camelCase, OpenAPI) : userId, petId
  - name: '/Id$/'
    domain: DO_ID
  # Finit par "_id", quelle que soit la casse (base de données) : user_id, USER_ID
  - name: '/(?i)_id$/'
    domain: DO_ID
  # Commence par "date", quelle que soit la casse : dateNaissance, DATE_FIN
  - name: '/(?i)^date/'
    domain: DO_DATE
  # Se termine par un ou plusieurs chiffres : adresse1, adresse2
  - name: '/\d+$/'
    domain: DO_LIBELLE
  # Sur le type : varchar, nvarchar, character varying...
  - type: '/char/'
    domain: DO_LIBELLE
```

### Choix du domaine

La recherche se fait en deux temps :

1. **Sur le nom** : seules les entrées avec un `name` sont considérées. Un `name` exact l'emporte sur un `name` regex.
2. **Sur le type**, uniquement si aucune entrée `name` ne correspond. Un `type` exact l'emporte sur un `type` regex.

Un `name` qui correspond prend donc toujours le pas sur un `type`. À score égal, le premier domaine déclaré l'emporte. Si rien ne correspond, le type de la source est utilisé tel quel comme domaine.

Les spécificités de chaque source (`scale` et `precision` pour les bases de données, types OpenAPI) sont détaillées dans les pages [OpenApi](./tmdgen/openapi) et [Database](./tmdgen/database).

## Lancement de la génération

L'outil `tmdgen` se lance avec la commande :

```bash
tmdgen
```

### Options disponibles

- **`--file`**/**`-f`** : Précise le fichier de configuration à utiliser. Par défaut, l'outil cherchera tous les fichiers de configuration au format `tmdgen*.config`.
- **`--watch`**/**`-w`** : Permet de surveiller les modifications du fichier de configuration `tmdgen.config` et de relancer la génération automatiquement. À la différence de `modgen`, cette option ne permet de suivre les modifications que du fichier de configuration.

### Exemples d'utilisation

```bash
# Génération simple (tous les fichiers de configuration trouvés)
tmdgen

# Génération avec surveillance des modifications
tmdgen --watch

# Génération d'un fichier de configuration spécifique
tmdgen --file tmdgen.config
```
