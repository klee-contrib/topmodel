# Database

## Présentation

Le générateur `database` sert à générer du code `TopModel` à partir d'une source **base de données**. Il se connecte à une base de données pour récupérer les informations suivantes:

- Noms des tables
- Noms et types de colonnes
- Contraintes de clés étrangères
- Contraintes de clés primaires
- Contraintes d'unicité

Ces informations servent donc à initialiser des classes TopModel. Le générateur construit ensuite un arbre de **dépendance** des différentes classes les unes avec les autres. Puis, elles sont **regroupées** par **modules**, et enfin par **fichiers**. Les différentes étapes de cet algorithme sont expliquées plus bas.

## Configuration

La configuration du générateur `database` permet de définir :

- `outputDirectory` : répertoire de génération des fichiers, relatif au `modelRoot`
- `source` :
  - `dbType`: type de base de base de données source (`postgresql` ou `oracle`)
  - `host`: hôte de la base de données
  - `port`: port d'écoute de la base de données
  - `dbName`: nom de la base de données
  - `user`: nom de l'utilisateur de connexion à la base de données
  - `schema`: schéma dont on souhaite extraire la structure, `db owner` pour les bases Oracle
  - `password`: (factultatif) mot de passe de connexion à la base de données
- `domains` : Correspondance entre les types dans la spécification `openApi` et les domaines du modèle cible. Quelques spécificités
  - Définition d'un `name` ou d'un `type`, pour matcher soit sur le nom de la propriété soit sur son type
  - Les regexp sont acceptées
  - Possibilité de définir `scale` et `precision`, pour faire correspondre les types avec plus de finesse
  - Le meilleur domaine à correspondre au type/nom de la colonne est utilisé pour la propriété
- `tags` : liste des tags à ajouter aux fichier de modèle
- `exclude` : liste des noms des tables à exclure de la génération (au format `snake_case`)
- `extractValues`: liste des noms des classes dont on souhaite extraire les valeurs à placer dans des enums TopModel (au format `PascalCase`)
- `modules`: description des modules à générer avec
  - `name`: Nom du modules
  - `classes`: Liste des classes (au format `PascalCase`) qu'on souhaite voir regroupées dans ce module

### Exemple

```yaml
modelRoot: ./model
database:
  - outputDirectory: ./
    tags:
      - Back
    extractValues:
      - TypeUtilisateur
    modules:
      - name: Common
        classes:
          - Adresse
          - Departement
          - Fichier
      - name: Utilisateur
        classes:
          - Utilisateur
    domains:
      - name: /(?i)id/
        domain: DO_ID
      - type: integer
        domain: DO_ENTIER
      - type: numeric
        domain: DO_ENTIER
      - type: bigint
        domain: DO_ENTIER
      - type: name
        domain: DO_LIBELLE
      - type: character varying
        domain: DO_LIBELLE
      - type: character varying
        domain: DO_CODE
        scale: 3
      - type: text
        domain: DO_LIBELLE
      - type: double precision
        domain: DO_ENTIER
      - type: boolean
        domain: DO_BOOLEAN
      - type: date
        domain: DO_DATE
      - type: smallint
        domain: DO_ENTIER
      - type: timestamp without time zone
        domain: DO_DATE_TIME
    exclude:
      - spatial_ref_sys
      - us_lex
      - us_gaz
      - us_rules
      - flyway_schema_history
      - raster_columns
      - raster_overviews
      - geometry_columns
      - geography_columns
    source:
      host: localhost
      port: 5432
      dbName: demo
      schema: demo
      user: demo
```

## Connexion à la base de données

Actuellement, seules les bases de données `postgresql` et `oracle` sont supportées par le générateur `database`. Pour s'y connecter, remplir les informations de la propriété `source` de la configuration. Vous pouvez remarquer que le `password` n'est pas obligatoire. En effet, vous pouvez utiliser la variable d'environnement `PGPASSWORD` pour `postgresql` par exemple.

Si la connexion à la base de données échoue, le mot de passe vous sera demandé dans la console.

## Algorithme de regroupement

### Fichiers sans dépendance circulaire

Tout d'abord, après avoir construit les classes, le générateur va essayer de leur affecter chacune un fichier. Dans un premier temps, il va créer un fichier pour chaque classe sans dépendance. Puis, et de manière itérative, chaque classe dont toutes les dépendances possèdent déjà un fichier se voit affecter également un nouveau fichier.

### Fichiers avec dépendance circulaire

Suite à cette première étape, seules les classes avec des dépendances circulaires restent sans fichier. L'algorithme prend alors parmi elles celle qui a le plus de dépendances. Il lui affecte un nouveau fichier, puis l'attribue également à toutes ses dépendances (qui n'ont pas déjà un fichier). Cette opération est répétée tant que toutes les classes n'ont pas toutes été placées dans un fichier.

### Regroupement en modules

Afin d'optenir un modèle prêt à l'emploi, lisible et bien ordonné, le générateur `database` va essayer de son mieux de regrouper les classes par modules fonctionnels. Pour cela, il utilise deux sources d'information:

- Le fichier de configuration
- Le poid de chaque classe, et ses dépendances

#### Regroupements forcés par la configuration

Tout d'abord, les classes paramétrées dans les modules définis dans le fichier de configuration se voient affecter le module correspondant.

> Veillez à respecter la règle suivante : **ne pas affecter dans deux modules différents deux classes qui ont des dépendances circulaires entre elles**. Bien que cela ne bloque pas la génération, l'attribution de module ne pourra jamais correspondre à votre configuration

Lorsque les modules _forcés_ par la configuration ont été attribués, le générateur va ajouter à ces modules, dans l'ordre dans lequels ces derniers ont été écrits dans le fichier de configuration, toutes les classes dont dépendent celles déjà affectées. Par exemple si la classe `Utilisateur` possède dans le modèle cible une association vers la classe `TypeUtilisateur`, et que la classe `Utilisateur` a été ajoutée au module `Securite` dans le fichier de configuration, alors `TypeUtilisateur` sera également ajouté au module `Securite`.

Si ce regroupement automatique de `TypeUtilisateur` ne convient pas, il est toujours possible de _forcer_ `TypeUtilisateur` dans un autre module, via le fichier de configuration.

Ensuite, pour chaque classe qui a des dépendances, si celles-ci sont déjà toutes dans le même module, alors cette classe sera elle-même également ajoutée à ce module.

#### Regroupements automatiques

Une fois que tous ces regroupement ont été effectués, le générateur va tenter de faire des regroupements automatiques pour les classes restantes.

Il les trie d'abord par poids (nombre de dépendances + nombre de références), prend la première et lui affecte un module portant son nom. Puis, de la même manière que précédemment, regroupe toutes ses dépendances (non affectées) dans ce module. Et enfin, pour chaque classe qui a des dépendances, si elles sont toutes dans le même module, alors cette classe y est ajoutée.

Le générateur procède ainsi tant qu'il reste des classes avec plus de deux dépendances, sans module. En toute logique, il ne restera que des classes avec deux dépendances ou moins.

Les classes possédant une ou deux dépendances sont regroupées dans le module `Join`, tandis que celles sans dépendance ni référence sont regroupées dans `Autres`. Le module `Join` est principalement constitué des classes de jointures entre deux classes de modules différents. L'on peut imaginer qu'il s'agisse d'associations `ManyToMany`, mais le générateur ne les écrira pas ainsi par soucis de simplicité.

> Les modules sont numérotés, dans l'ordre de leur création. Ceci permet de mieux comprendre les regroupement effectués, et éventuellement d'adapter la configuration

### Regroupements des fichiers

Jusque là, les fichiers créés ne contiennent qu'une seule classe, sauf évidemment ceux qui contiennent des dépendances circulaires. Cela peut représenter beaucoup de fichiers. Le générateur va donc essayer de les regrouper par proximité logique. La règle est la suivante : **regroupement des fichiers dont le module est identiques, et qui possèdent les mêmes dépendances aux autres fichiers**. Appliquée de manière itérative, cette règle permet de réduire drastiquement le nombre de fichier générés, tout en gardant une certaine logique.

Les fichiers ainsi regroupés sont finalement renommés avec le nom de la classe la plus **importante** du fichier, c'est à dire celle avec le plus de dépendances.

> Les fichiers sont également numérotés à la fin de la génération. L'ordre des fichiers respecte l'ordre des profondeurs des dépendances. Le premier fichier n'a souvent aucune dépendance, le second n'en a qu'une seule, etc. Plus le fichier est _profond_, moins il contient de classes, mais plus il contient de dépendances. On garde finalement une complexité de fichier _à peu près_ constante, les fichiers sont _à peu près_ lisibles. L'objectif étant d'obtenir un modèle **prêt à l'emploi**, maintenable facilement, organisé de manière efficiente, etc.
