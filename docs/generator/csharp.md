# Générateur C#

## Présentation

Le générateur C# peut générer les fichiers suivants :

- Un fichier de définition de classe C# pour chaque classe dans le modèle.
- Un (ou deux) fichier(s) par module avec les mappers des classes du module.
- Un DbContext pour Entity Framework Core (si demandé).
- Un fichier de contrôleur pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode serveur.
- Un fichier de client d'API pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode client.
- 2 fichiers par module (interface + implémentation) contenant des accesseurs de listes de référence pour Kinetix (si demandé).

Le code généré n'a aucune dépendance externe à part EF Core et Kinetix, et uniquement s'ils sont explicitement demandés dans la configuration.

### Génération des mappers

Les mappers sont générés comme des méthodes statiques dans une classe statique. Les mappers `to` peuvent être utilisés comme méthodes d'extensions sur la classe qui le définit. Il est conseillé d'utiliser un `using static ModuleMappers;` dans les fichiers où on utilise des mappers pour pouvoir référencer un mapper `from` directement avec son nom (par exemple `CreateMyClassDTO(myClass)` au lieu de `ModuleMappers.CreateMyClassDTO(myClass)`)

De plus, pour un module, on sépare les mappers en deux fichiers potentiels :

- Tous les mappers qui référencent au moins une classe persistée seront générés dans un fichier `ModuleMappers`, qui sera généré à côté des classes persistées du module.
- Les autres (ceux qui référencent uniquement des classes non persistées) seront générés dans un fichier `ModuleDTOMappers`, qui sera généré à côté des classes non-persistées du module.

### Génération du DbContext

Le DbContext peut être généré soit comme un simple "repository" avec juste la liste de tous les `DbSet` des classes persistées, ou alors il peut être complété avec l'ensemble des informations nécessaires pour générer les migrations de base de données avec EF Core. Dans le premier cas, le modèle de base de données devra être généré et mis à jour autrement (par exemple avec le générateur SQL de TopModel).

### Génération des contrôleurs

Les contrôleurs sont **partiellement générés**. TopModel va initialiser un contrôleur "vide" avec une méthode par endpoint, pour laquelle il définit tout sauf son corps. Il faudra ensuite implémenter chaque endpoint en modifiant le fichier (et en ajoutant les diverses dépendances, annotations sur la classe...).

Les générations successives du contrôleur vont essayer de retrouver chaque endpoint dans le contrôleur préalablement généré :

- Si on le trouve, alors on récupère le corps de la méthode existante, on remplace l'ancienne méthode par l'ancienne, puis on remet l'ancien corps.
- Si on ne le trouve pas, alors on l'ajoute vide.

De plus, on va **supprimer toutes les méthodes publiques du contrôleur qui ne correspondent pas à un endpoint dans le modèle**. Par conséquent, **un renommage d'endpoint ne va pas conserver son implémentation dans le contrôleur**. Il conviendra donc de faire attention à ne pas perdre du code lors d'une telle opération (il faudra donc faire le copier/coller manuellement).

### Génération des clients d'API

Les clients d'API sont générés comme des classes partielles avec 2 méthodes à implémenter : `EnsureAuthentication` et `EnsureSuccess`. La première sera appelée avant chaque appel, tandis que la seconde sera appelée après. Comme leurs noms le laisse suggérer :

- `EnsureAuthentication` a pour but principal de renseigner un header d'authentification sur la requête (qui peut supposer d'avoir à faire un appel à un fournisseur d'identité externe)
- `EnsureSuccess` a pour but principal de gérer les erreurs éventuelles retournées par l'API appelée. Il faudrait au minimum vérifier `response.IsSuccessStatusCode` à l'intérieur et renvoyer une exception si l'appel est en erreur. La gestion précise de l'erreur est à priori spécifique à chaque API et dépend du besoin fonctionnel. Si on attend un résultat en JSON de l'API et qu'on sait qu'elle ne va pas le renvoyer (à priori parce qu'il y a une réponse en erreur), il est important de lever une exception puisque sinon on va essayer de désérialiser la réponse dans la foulée.

### Génération des accesseurs de références

Les implémentations d'accesseurs de listes de références pour Kinetix utilisent EF Core si un DbContext est configuré et l'ORM Kinetix (`Kinetix.DataAccess.Sql`) dans le cas contraire.

## Configuration

- `persistantModelPath`

  Localisation du modèle persisté, relative au répertoire de génération.

  Le namespace des classes générées sera déterminé à partir de cette localisation, en retirant tout ce qui précède `{app}` dans le chemin.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"{app}.{module}.Models"`

- `persistantReferencesModelPath`

  Localisation des classes de références persistées, relative au répertoire de génération.

  Le namespace des classes générées sera déterminé à partir de cette localisation, en retirant tout ce qui précède `{app}` dans le chemin.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: Valeur de `persistantModelPath`

- `nonPersistantModelPath`

  Localisation du modèle non persisté, relative au répertoire de génération.

  Le namespace des classes générées sera déterminé à partir de cette localisation, en retirant tout ce qui précède `{app}` dans le chemin.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"{app}.{module}.Models/Dto"`

- `apiRootPath`

  Localisation du l'API générée (client ou serveur), relatif au répertoire de génération.

  _Templating_: `{app}`

  _Valeur par défaut_: `"{app}.Web"`

- `apiFilePath`

  Chemin vers lequel sont créés les fichiers d'endpoints générés, relatif à la racine de l'API.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{module}"`

- `apiGeneration`

  Mode de génération de l'API (`"client"` ou `"server"`).

- `noAsyncControllers`

  Génère des contrôleurs d'API synchrones (pour des applications "legacy" qui ne gèrent pas des services asynchrones)

- `dbContextPath`

  Localisation du DbContext, relatif au répertoire de génération.

  C'est ce paramètre qui décide si le DbContext est généré ou non.

- `dbContextName`

  Nom du DbContext.

  _Templating_: `{app}`

  _Valeur par défaut_: `"{app}DbContext"`

- `referenceAccessorsInterfacePath`

  Chemin vers lequel générer les interfaces d'accesseurs de référence.

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"{DbContextPath}/Reference"`

- `referenceAccessorsImplementationPath`

  Chemin vers lequel générer les implémentation d'accesseurs de référence.

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"{DbContextPath}/Reference"`

- `referenceAccessorsName`

  Nom des accesseurs de référence (préfixé par 'I' pour l'interface).

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{app}`, `{module}`

  _Valeur par défaut_: `"{module}ReferenceAccessors"`

- `useEFMigrations`

  Utilise les migrations EF pour créer/mettre à jour la base de données.

- `useLowerCaseSqlNames`

  Utilise des noms de tables et de colonnes en lowercase.

  EF Core met des guillemets autour de tous les noms de table dans les requêtes qu'il génère (ainsi que dans les migrations), ce qui pose problème avec PostgreSQL qui est case-sensitive mais utilise les minuscules par défaut.

- `dbSchema`

  Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo'/'public').

- `useLatestCSharp`

  Utilise les features C# 10 dans la génération. (namespaces de fichiers, usings implicites...)

  _Valeur par défaut_: `true`

- `kinetix`

  Active les fonctionnalités Kinetix dans la génération.

  - Génération des accesseurs de liste de références
  - Annotations de domaines et de classe de références dans les classes

  _Valeur par défaut_: `true`

- `noColumnOnAlias`

  Retire les attributs de colonnes sur les alias (ça ne plaît pas à EF Core mais ça peut être utile pour d'autres ORMs pour mapper directement les colonnes)

- `noPersistance`

  Considère tous les classes comme étant non-persistantes (pas d'attributs SQL, génération vers le chemin non-persistant...).

- `enumsForStaticReferences`

  Utilise des enums C# à la place du type original pour les listes de références statiques (= clé primaire non-autogénérée).

- `useEFComments`

  Génère les commentaires en SQL pour les migrations EF Core (à partir des commentaires du modèle).
