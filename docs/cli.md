# Ligne de commandes

## Générateur TopModel

### Commande principale

La commande **`modgen`** lance le générateur TopModel. Elle récupère par défaut tous les fichiers de configuration qu'elle trouve dans le répertoire courant, et génère le modèle correspondant à chaque configuration.

### Utilisation de base

```bash
# Génération simple (tous les fichiers de configuration trouvés)
modgen

# Génération avec surveillance des modifications (mode watch)
modgen --watch

# Génération d'un fichier de configuration spécifique
modgen --file topmodel.config

# Génération de plusieurs fichiers de configuration
modgen --file topmodel.config --file topmodel.autre.config
```

### Options

| Option | Description | Exemple |
|--------|-------------|---------|
| `-f, --file <file>` | Chemin vers un fichier de config en particulier à générer (au lieu de la récupération automatique de tous les fichiers). Cette option peut être spécifiée plusieurs fois pour embarquer plusieurs configurations spécifiques. | `modgen --file topmodel.config` |
| `-e, --exclude <exclude>` | Tag à ignorer lors de la génération. Cette option peut être spécifiée plusieurs fois pour exclure plusieurs tags. | `modgen --exclude TagA --exclude TagB` |
| `-w, --watch` | Permet de "surveiller" toute modification de fichier, et TopModel essaiera de "recompiler" le(s) modèle(s) à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables. | `modgen --watch` |
| `-u, --update <update>` | Met à jour le module de générateurs spécifié (ou tous les modules si 'all'). | `modgen --update csharp` ou `modgen --update all` |
| `-c, --check` | Vérifie que le code généré est conforme au modèle. | `modgen --check` |
| `-s, --schema` | Génère le fichier de schéma JSON complet, à côté du fichier de configuration, et met à jour ce dernier pour y inclure une référence vers le schéma, pour que VS Code puisse proposer la complétion et la validation. | `modgen --schema` |
| `--version` | Affiche les informations de version. | `modgen --version` |
| `-?, -h, --help` | Affiche l'aide et les informations d'utilisation. | `modgen --help` |

## Changer la version du générateur

### Mise à jour vers une version spécifique

Pour mettre à jour ou changer la version de TopModel.Generator, utilisez la commande suivante :

```bash
dotnet tool update -g TopModel.Generator --version X.X.X
```

> **Note** : `X.X.X` correspond au numéro de version souhaitée (par exemple `3.6.1`).

### Downgrade vers une version antérieure

Pour effectuer un downgrade (rétrograder vers une version antérieure), utilisez l'option `--allow-downgrade` :

```bash
dotnet tool update -g TopModel.Generator --version X.X.X --allow-downgrade
```

### Mise à jour vers la dernière version

Pour mettre à jour vers la dernière version disponible :

```bash
dotnet tool update -g TopModel.Generator
```

### Vérifier la version installée

Pour vérifier la version actuellement installée :

```bash
modgen --version
```

ou

```bash
dotnet tool list -g | grep TopModel.Generator
```
