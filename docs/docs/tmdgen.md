# Migrer un modèle externe

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
