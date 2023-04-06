# Générateur `C#`

## Présentation

Le générateur C# peut générer les fichiers suivants :

- Un fichier de définition de classe C# pour chaque classe dans le modèle.
- Un (ou deux) fichier(s) par module avec les mappers des classes du module.
- Un DbContext pour Entity Framework Core (si demandé).
- Un fichier de contrôleur pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode serveur.
- Un fichier de client d'API pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode client.
- 2 fichiers par module (interface + implémentation) contenant des accesseurs de listes de référence pour Kinetix (si demandé).

Le code généré n'a aucune dépendance externe à part EF Core et Kinetix, et uniquement s'ils sont explicitement demandés dans la configuration.

### Génération des classes

Le générateur C# fait peu de différences à la génération entre une classe persistée et non persistée. Seule l'annotation `[Table]` est ajoutée en plus sur une classe persistée, et les annotations `[Column]` sont conservées à moins de les désactiver explicitement sur les alias via le paramètre de config `noColumnOnAlias`.

Les propriétés d'associations sont toujours gérées comme en SQL, avec la clé primaire de la classe cible. Les associations `oneToMany` et `manyToMany` auront le type du domaine liste de la clé primaire, munie d'une annotation `[NotMapped]` pour indiquer qu'elle ne seront pas implicitement supportées.

Pour les classes `enum`, le générateur peut soit générer des constantes pour chacune des valeurs possibles de la clé primaire (comportement par défaut), soit, si l'option `enumForStaticReferences` est activée, générer une vraie enum C# dans la classe (dont le nom est `{prop}s`) et remplacer le type de la propriété par cette enum. Cette enum sera évidemment utilisé pour toutes les références de cette propriété (association, alias...). Les constantes auront le nom de la `value`, tandis que les valeurs de l'enum auront la valeur de la clé primaire (ce qui conditionne donc également la génération de l'enum au fait que la valeur est un identifiant C# valide).

De plus, si la classe définit des `values` et une clé d'unicité simple sur un des champs de la classe, alors des constantes seront aussi générées pour chaque valeur définie dans `values`. Si la classe est une `enum` et que l'option `enumForStaticReferences` est activée, alors une vraie enum C# sera générée à la place, comme pour la clé primaire. Les constantes auront le nom de la `value`, suffixée par le nom de la propriété.

### Génération des mappers

Les mappers sont générés comme des méthodes statiques dans une classe statique. Les mappers `to` peuvent être utilisés comme méthodes d'extensions sur la classe qui le définit. Il est conseillé d'utiliser un `using static ModuleMappers;` dans les fichiers où on utilise des mappers pour pouvoir référencer un mapper `from` directement avec son nom (par exemple `CreateMyClassDTO(myClass)` au lieu de `ModuleMappers.CreateMyClassDTO(myClass)`)

De plus, pour un module, on sépare les mappers en deux fichiers potentiels :

- Tous les mappers qui référencent au moins une classe persistée seront générés dans un fichier `ModuleMappers`, qui sera généré à côté des classes persistées du module.
- Les autres (ceux qui référencent uniquement des classes non persistées) seront générés dans un fichier `ModuleDTOMappers`, qui sera généré à côté des classes non-persistées du module.

### Génération du DbContext

Le DbContext peut être généré soit comme un simple "repository" avec juste la liste de tous les `DbSet` des classes persistées, ou alors il peut être complété avec l'ensemble des informations nécessaires pour générer les migrations de base de données avec EF Core. Dans le premier cas, le modèle de base de données devra être généré et mis à jour autrement (par exemple avec le générateur SQL de TopModel).

Dans le cas ou les migrations sont générées, les associations `oneToMany` et `manyToMany` seront ignorées. Le générateur C# s'attend à ce qu'elles soient gérées explicitement comme si elles l'avaient été en SQL : via une `manyToOne` de l'autre côté pour la première, et via une table dédiée avec deux associations marquées comme `primaryKey` pour la deuxième.

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

Les implémentations d'accesseurs de listes de références pour Kinetix utilisent EF Core si un DbContext est configuré et l'ORM Kinetix (`Kinetix.DataAccess.Sql`) dans le cas contraire. Ils sont générés pour les classes marquées comme `reference`.

## Configuration

- `persistantModelPath`

  Localisation du modèle persisté, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{app}.{module}.Models"`

  _Variables par tag_: **oui** (plusieurs définition de classes pourraient être générées si un fichier à plusieurs tags)

- `persistantReferencesModelPath`

  Localisation des classes de références persistées, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{module}`

  _Valeur par défaut_: Valeur de `persistantModelPath`

  _Variables par tag_: **oui** (plusieurs définition de classes pourraient être générées si un fichier à plusieurs tags)

- `nonPersistantModelPath`

  Localisation du modèle non persisté, relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{app}.{module}.Models/Dto"`

  _Variables par tag_: **oui** (plusieurs définition de classes pourraient être générées si un fichier à plusieurs tags)

- `apiRootPath`

  Localisation du l'API générée (client ou serveur), relative au répertoire de génération.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  _Valeur par défaut_: `"{app}.Web"`

  _Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier à plusieurs tags)

- `apiFilePath`

  Chemin vers lequel sont créés les fichiers d'endpoints générés, relative à la racine de l'API.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{module}"`

  _Variables par tag_: **oui** (plusieurs clients/serveurs pourraient être générés si un fichier à plusieurs tags)

- `apiGeneration`

  Mode de génération de l'API (`"client"` ou `"server"`).

  _Variables par tag_: **oui** (la valeur de la variable doit être `"client"` ou `"server"`. le client et le serveur pourraient être générés si un fichier à plusieurs tags)

- `noAsyncControllers`

  Génère des contrôleurs d'API synchrones (pour des applications "legacy" qui ne gèrent pas des services asynchrones)

- `dbContextPath`

  Localisation du DbContext, relative au répertoire de génération.

  C'est ce paramètre qui décide si le DbContext est généré ou non.

  _Variables par tag_: **oui** (plusieurs contextes pourraient être générés si un fichier à plusieurs tags)

- `dbContextName`

  Nom du DbContext.

  _Valeur par défaut_: `"{app}DbContext"`

  _Variables par tag_: **oui** (plusieurs contextes pourraient être générés si un fichier à plusieurs tags)

- `referenceAccessorsInterfacePath`

  Chemin vers lequel générer les interfaces d'accesseurs de référence.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{DbContextPath}/Reference"`

  _Variables par tag_: **oui** (plusieurs accesseurs pourraient être générés si un fichier à plusieurs tags)

- `referenceAccessorsImplementationPath`

  Chemin vers lequel générer les implémentation d'accesseurs de référence.

  Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{DbContextPath}/Reference"`

  _Variables par tag_: **oui** (plusieurs accesseurs pourraient être générés si un fichier à plusieurs tags)

- `referenceAccessorsName`

  Nom des accesseurs de référence (préfixé par 'I' pour l'interface).

  Les accesseurs de référence ne seront générés que si `kinetix: true`.

  _Templating_: `{module}`

  _Valeur par défaut_: `"{module}ReferenceAccessors"`

  _Variables par tag_: **oui** (plusieurs accesseurs pourraient être générés si un fichier à plusieurs tags)

- `useEFMigrations`

  Utilise les migrations EF pour créer/mettre à jour la base de données.

- `useLowerCaseSqlNames`

  Utilise des noms de tables et de colonnes en lowercase.

  EF Core met des guillemets autour de tous les noms de table dans les requêtes qu'il génère (ainsi que dans les migrations), ce qui pose problème avec PostgreSQL qui est case-sensitive mais utilise les minuscules par défaut.

- `dbSchema`

  Le nom du schéma de base de données à cibler (si non renseigné, EF utilise 'dbo'/'public').

  _Templating_: `{module}`

  _Variables par tag_: **oui** (à faire correspondre avec les valeurs de différents `modelPath`)

- `useLatestCSharp`

  Utilise les features C# 10 dans la génération. (namespaces de fichiers, usings implicites...)

  _Valeur par défaut_: `true`

- `kinetix`

  Active les fonctionnalités Kinetix dans la génération.

  - Génération des accesseurs de liste de références
  - Annotations de domaines et de classe de références dans les classes

  _Valeur par défaut_: `true`

- `domainNamespace`

  Namespace de l'enum de domaine pour Kinetix.

  _Valeur par défaut_: `{app}.Common`

- `noColumnOnAlias`

  Retire les attributs de colonnes sur les alias (ça ne plaît pas à EF Core mais ça peut être utile pour d'autres ORMs pour mapper directement les colonnes)

- `noPersistance`

  Considère tous les classes comme étant non-persistantes (pas d'attributs SQL, génération vers le chemin non-persistant...).

  _Variables par tag_: **oui**

- `enumsForStaticReferences`

  Utilise des enums C# à la place du type original pour les listes de références statiques (= clé primaire non-autogénérée).

- `useEFComments`

  Génère les commentaires en SQL pour les migrations EF Core (à partir des commentaires du modèle).
