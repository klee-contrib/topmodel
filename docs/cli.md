# Ligne de commandes

## Générateur TopModel

### Commande principale

La commande **`modgen`** lance le générateur TopModel. Elle récupère par défaut tous les fichiers de configuration qu'elle trouve dans le répertoire courant, et génère le modèle correspondant à chaque configuration.


### Options

| Option | Description |
|--------|-------------|
| `-f, --file <file>` | Chemin vers un fichier de config en particulier à générer (au lieu de la récupération automatique de tous les fichiers). Cette option peut être spécifiée plusieurs fois pour embarquer plusieurs configurations spécifiques. |
| `-e, --exclude <exclude>` | Tag à ignorer lors de la génération. Cette option peut être spécifiée plusieurs fois pour exclure plusieurs tags. |
| `-w, --watch` | Permet de "surveiller" toute modification de fichier, et TopModel essaiera de "recompiler" le(s) modèle(s) à chaque fois. En cas d'erreur, cette dernière sera affichée dans la console avec sa localisation dans les fichiers sources. Si TopModel est ouvert dans la console intégrée de VSCode, alors les liens seront cliquables. |
| `-u, --update <update>` | Met à jour le module de générateurs spécifié (ou tous les modules si 'all'). |
| `-c, --check` | Vérifie que le code généré est conforme au modèle. |
| `-s, --schema` | Génère le fichier de schéma JSON complet, à côté du fichier de configuration, et met à jour ce dernier pour y inclure une référence vers le schéma, pour que VS Code puisse proposer la complétion et la validation. |
| `--version` | Affiche les informations de version. |
| `-?, -h, --help` | Affiche l'aide et les informations d'utilisation. |


## Changer la version du générateur

Pour mettre à jour ou changer la version de TopModel.Generator, utilisez la commande suivante :

```bash
dotnet tool update -g TopModel.Generator --version X.X.X
```

> **Note** : `X.X.X` correspond au numéro de version souhaitée.

Pour effectuer un downgrade (rétrograder vers une version antérieure), utilisez l'option `--allow-downgrade` :

```bash
dotnet tool update -g TopModel.Generator --version X.X.X --allow-downgrade
```






