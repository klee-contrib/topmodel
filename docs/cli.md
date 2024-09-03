# Ligne de commandes

## Générateur 

### Commande
```modgen```: Lance le générateur Topmodel.


### Options

```bash
-f, --file <file>
Chemin vers un fichier de config.

-e, --exclude <exclude>
Tag à ignorer lors de la génération.

-w, --watch
Lance le générateur en mode 'watch'.

-u, --update <update>    
Met à jour le module de générateurs spécifié (ou tous les modules si 'all').

-c, --check
Vérifie que le code généré est conforme au modèle.

-s, --schema
Génère le fichier de schéma JSON du fichier de config.

--version
Affiche les informations de version.

-?, -h, --help
Affiche l'aide et les informations d'utilisation.
```


## Changer la version du générateur

```bash
dotnet tool update -g TopModel.Generator --version X.X.X
```

Note: X.X.X correspond au numéro de version souhaitée.

 Pour un downgrade, utiliser l'option ```--allow-downgrade``` :

```bash
dotnet tool update -g TopModel.Generator --version X.X.X --allow-downgrade
```






