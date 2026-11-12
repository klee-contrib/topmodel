# Générateur `C#`

## Présentation

Le générateur C# peut générer les fichiers suivants :

- Un fichier de définition de classe C# pour chaque classe dans le modèle.
- Un (ou deux) fichier(s) par module avec les mappers des classes du module.
- Un DbContext pour Entity Framework Core (si demandé).
- Un fichier de contrôleur pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode serveur.
- Un fichier de client d'API pour chaque fichier d'endpoints dans le modèle, si les APIs sont générées en mode client.
- 2 fichiers par module (interface + implémentation) contenant des accesseurs de listes de référence pour Kinetix (si demandé).
- Un fichier par flux de données et un fichier d'enregistrement de flux par module (si demandés).

Le code généré n'a aucune dépendance externe à part EF Core et Kinetix, et uniquement s'ils sont explicitement demandés dans la configuration.

### Types C#

Tous les types C# renseignés dans les domaines doivent être non-nullables (toujours utiliser `int` à la place de `int?` par exemple), et c'est le générateur qui s'occupera de le rendre nullable (en ajoutant un `?`) si besoin, selon le caractère obligatoire du champ et le paramétrage du générateur (en particulier l'usage de `nullableEnable` et de `requiredNonNullable`).

Pour pouvoir renseigner les éventuels `?` ou `.Value` nécessaires, le générateur a besoin de connaître les types valeurs (comme `int`, `bool` ou `DateTime` par exemple) que vous utilisez. Il connaît déjà la plupart des types courants, et vous pouvez compléter cette liste via la propriété `valueTypes`.

### Génération des classes

Le générateur C# fait peu de différences à la génération entre une classe persistée et non persistée. Seule l'annotation `[Table]` est ajoutée en plus sur une classe persistée, et les annotations `[Column]` sont conservées à moins de les désactiver explicitement sur les alias via le paramètre de config `noColumnOnAlias` (qui est `true` par défaut).

Les propriétés d'associations sont toujours gérées comme en SQL, avec la clé primaire de la classe cible. Les associations `oneToMany` et `manyToMany` auront le type du domaine liste de la clé primaire, munies d'une annotation `[NotMapped]` pour indiquer qu'elles ne seront pas implicitement supportées.

Pour les classes `enum`, le générateur peut soit générer des constantes pour chacune des valeurs possibles de la clé primaire (comportement par défaut), soit, si l'option `enumForStaticReferences` est activée, générer une vraie enum C# dans la classe (dont le nom est `{prop}s`) et remplacer le type de la propriété par cette enum. Cette enum sera évidemment utilisée pour toutes les références de cette propriété (association, alias...). Les constantes auront le nom de la `value`, tandis que les valeurs de l'enum auront la valeur de la clé primaire (ce qui conditionne donc également la génération de l'enum au fait que la valeur est un identifiant C# valide).

De plus, si la classe définit des `values` et une clé d'unicité simple sur un des champs de la classe, alors des constantes seront aussi générées pour chaque valeur définie dans `values`. Si la classe est une `enum` et que l'option `enumForStaticReferences` est activée, alors une vraie enum C# sera générée à la place, comme pour la clé primaire. Les constantes auront le nom de la `value`, suffixées par le nom de la propriété.

### Génération des mappers

Les mappers sont générés comme des méthodes statiques dans une classe statique. Les mappers `to` peuvent être utilisés comme méthodes d'extensions sur la classe qui le définit. Il est conseillé d'utiliser un `using static ModuleMappers;` dans les fichiers où on utilise des mappers pour pouvoir référencer un mapper `from` directement avec son nom (par exemple `CreateMyClassDTO(myClass)` au lieu de `ModuleMappers.CreateMyClassDTO(myClass)`)

Chaque mapper `to` sera généré avec 2 surcharges :

- Une surcharge qui permet de créer une nouvelle instance. Cette surcharge prendra en paramètres (optionnels si `requiredNonNullable` est désactivé) l'ensemble des propriétés obligatoires de la classe cible pour lesquelles il n'existe pas de mapping depuis la classe source.
- Une surcharge qui permet de mapper vers une instance existante, passée en unique paramètre. Cette surcharge ne sera pas générée pour une classe abstraite ou une interface (`type` `abstract` ou `interface`).

De plus, pour un module, on sépare les mappers en deux fichiers potentiels :

- Tous les mappers qui référencent au moins une classe persistée seront générés dans un fichier `ModuleMappers`, qui sera généré à côté des classes persistées du module.
- Les autres (ceux qui référencent uniquement des classes non persistées) seront générés dans un fichier `ModuleDTOMappers`, qui sera généré à côté des classes non-persistées du module.

_Remarque : le module utilisé pour un mapper est celui de la classe persistée qui a été trouvée, où à défaut celui de la classe qui définit le mapper. Dans le cas où toutes les classes sont non persistées, il est possible de spécifier un tag dans la liste du paramètre `moduleTagsOverrides` pour choisir les classes définies dans les fichiers avec ce tag._

#### Impacts de `requiredNonNullable`

Afin de gérer tous les cas où des propriétés nullables pourraient être mappées vers des propriétés non-nullables (soit parce que c'est `required` d'un côté et pas de l'autre, soit parce que la valeur du paramètre n'est pas la même pour les deux classes), on générera en plus :

- Un check de non-nullité sur toutes les propriétés et paramètres nullables lorsqu'on veut les mapper vers une propriété non-nullable
- Un `.Value` (ou un cast vers le type non-nullable) derrière les propriétés de type valeur qu'on a vérifiées dans le point précédent (si votre type n'est pas reconnu par le générateur comme un type valeur, vous pouvez l'ajouter via la propriété `valueTypes`)

Dans le cas où le mapper ne renseigne pas toutes les propriétés obligatoires de la classe cible (celles qui ont un `required` dans leur définition C# générée) :

- Si c'est un mapper `from`, **le mapper généré lèvera une erreur C#**, et vous devrez modifier votre modèle pour que le code généré compile correctement (soit en ajoutant les propriétés manquantes dans le mapper, soit en rendant les propriétés non obligatoires, soit en leur mettant des valeurs par défaut).
- Si c'est un mapper `to`, il n'y aura pas de problème parce que les propriétés obligatoires manquantes ont été ajoutées en paramètre de la surcharge qui instancie une nouvelle classe. Ces paramètres seront en revanche bien obligatoires, comme les propriétés cibles.

### Génération du DbContext

Le DbContext peut être généré soit comme un simple "repository" avec juste la liste de tous les `DbSet` des classes persistées, soit être complété avec l'ensemble des informations nécessaires pour générer les migrations de base de données avec EF Core. Dans le premier cas, le modèle de base de données devra être généré et mis à jour autrement (par exemple avec le générateur SQL de TopModel).

Dans le cas où les migrations sont générées, les associations `oneToMany` et `manyToMany` seront ignorées. Le générateur C# s'attend à ce qu'elles soient gérées explicitement comme si elles l'avaient été en SQL : via une `manyToOne` de l'autre côté pour la première, et via une table dédiée avec deux associations marquées comme `primaryKey` pour la deuxième.

### Génération des contrôleurs

Les contrôleurs sont **partiellement générés**. TopModel va initialiser un contrôleur "vide" avec une méthode par endpoint, pour laquelle il définit tout sauf le corps. Il faudra ensuite implémenter chaque endpoint en modifiant le fichier (et en ajoutant les diverses dépendances, annotations sur la classe...).

Les générations successives du contrôleur vont essayer de retrouver chaque endpoint dans le contrôleur préalablement généré :

- Si on le trouve, alors on récupère le corps de la méthode existante, on remplace l'ancienne méthode par la nouvelle, puis on remet l'ancien corps.
- Si on ne le trouve pas, alors on l'ajoute vide.

De plus, on va **supprimer toutes les méthodes publiques du contrôleur qui ne correspondent pas à un endpoint dans le modèle**. Par conséquent, **un renommage d'endpoint ne va pas conserver son implémentation dans le contrôleur**. Il conviendra donc de faire attention à ne pas perdre du code lors d'une telle opération (il faudra donc faire le copier/coller manuellement).

### Génération des clients d'API

Les clients d'API sont générés comme des classes partielles avec 2 méthodes à implémenter : `EnsureAuthentication` et `EnsureSuccess`. La première sera appelée avant chaque appel, tandis que la seconde sera appelée après. Comme leurs noms le laissent suggérer :

- `EnsureAuthentication` a pour but principal de renseigner un en-tête d'authentification sur la requête (ce qui peut nécessiter un appel à un fournisseur d'identité externe).
- `EnsureSuccess` a pour but principal de gérer les erreurs éventuelles retournées par l'API appelée. Il faudrait au minimum vérifier `response.IsSuccessStatusCode` à l'intérieur et renvoyer une exception si l'appel est en erreur. La gestion précise de l'erreur est a priori spécifique à chaque API et dépend du besoin fonctionnel. Si on attend un résultat en JSON de l'API et qu'on sait qu'elle ne va pas le renvoyer (a priori parce qu'il y a une réponse en erreur), il est important de lever une exception, puisque sinon on va essayer de désérialiser la réponse dans la foulée.

### Traductions

Les traductions de libellés de propriétés (via `translateProperties`) et de valeurs de listes de référence (via `translateReferences`) peuvent être générées de 2 manières différentes :

- Soit en base de données, si une [classe de traductions](/model/classes#classe-de-traductions) existe dans la configuration des générateurs C#.
- Soit dans des fichiers Resx, si la propriété `resourcesResxPath` est renseignée.

Via `resourcesInResx`, vous pouvez partager la génération des traductions entre la base de données et des fichiers de ressources, en générant par exemple les références en BDD et les propriétés dans l'application.

Si `translateReferences` est à `true`, alors les inserts de libellés de valeurs de listes de référence se feront avec la clé de traduction, indépendamment de la solution de génération de traductions choisie (s'il y en a une).

### Génération des accesseurs de références

Cette fonctionnalité n'est disponible qu'en utilisant Kinetix.

Le générateur va générer jusqu'à 2 interfaces par module, en séparant les listes de références persistées des non persistées, pour les classes marquées comme `reference`. Les noms d'interfaces et d'implémentations seront préfixés par `"Db"` pour les premières.

Seule l'implémentation pour les listes de référence persistées sera générée, en utilisant EF Core si un DbContext est configuré et l'ORM Kinetix (`Kinetix.DataAccess.Sql`) dans le cas contraire.

Il est nécessaire de spécifier le chemin de génération des interfaces (`referenceAccessorsInterfacePath`) pour que l'ensemble soit généré (le chemin des implémentations à une valeur par défaut).

Si les libellés de valeurs de listes de références sont traduits en base de données via EF Core, alors les accesseurs générés résoudront les libellés avec la culture courante de la requête.

### Génération des flux de données

_(en preview, documentation à venir)_

## Configuration

### `persistentModelPath`

Localisation du modèle persisté, relative au répertoire de génération.

_Valeur par défaut_ : `{app}.{module}.Models`

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

### `referencesModelPath`

Localisation des classes de références, relative au répertoire de génération. Si non renseigné, ces classes seront générées comme les autres (selon si elles sont persistantes ou non).

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

_Valeur par défaut_ : `persistentModelPath` ou `nonPersistentModelPath`, selon que la classe est persistée ou non

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

### `nonPersistentModelPath`

Localisation du modèle non persisté, relative au répertoire de génération.

_Valeur par défaut_ : `{app}.{module}.Models/Dto`

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs définitions de classes pourraient être générées si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

### `apiRootPath`

Localisation de l'API générée (client ou serveur), relative au répertoire de génération.

_Valeur par défaut_ : `{app}.Web`

_Templating_ : `{module}` (module du fichier d'endpoints)

_Variables par tag_ : **oui** (plusieurs clients/serveurs pourraient être générés si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

### `apiFilePath`

Chemin vers lequel sont créés les fichiers d'endpoints générés, relatif à la racine de l'API.

_Valeur par défaut_ : `{module:path}`

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs clients/serveurs pourraient être générés si un fichier a plusieurs tags)

### `noAsyncControllers`

Génère des contrôleurs d'API synchrones.

### `dbContextPath`

Localisation du DbContext, relative au répertoire de génération.

_Templating_ : `{module}` (un contexte est généré par chemin distinct, avec les classes des modules correspondants)

_Variables par tag_ : **oui** (plusieurs contextes pourraient être générés si un fichier a plusieurs tags)

C'est ce paramètre qui décide si le DbContext est généré ou non.

### `dbContextName`

Nom du DbContext.

_Valeur par défaut_ : `{app}DbContext`

_Templating_ : `{module}` (les points sont supprimés du nom résolu)

_Variables par tag_ : **oui** (plusieurs contextes pourraient être générés si un fichier a plusieurs tags)

### `noBaseDbContext`

Ne renseigne pas la classe de base `DbContext` sur le DbContext généré, pour que vous puissiez renseigner la vôtre (avec son constructeur) dans un partial.

### `dataFlowsPath`

Localisation des flux de données générés.

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs flux de données pourraient être générés si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

### `referenceAccessorsInterfacePath`

Chemin vers lequel générer les interfaces d'accesseurs de référence.

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs accesseurs pourraient être générés si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

Les accesseurs de référence ne seront générés que si `kinetix` est renseigné.

### `referenceAccessorsImplementationPath`

Chemin vers lequel générer les implémentations d'accesseurs de référence.

_Valeur par défaut_ : `{dbContextPath}/Reference`

_Templating_ : `{module}`

_Variables par tag_ : **oui** (plusieurs accesseurs pourraient être générés si un fichier a plusieurs tags)

Le chemin des fichiers cibles sera calculé en remplaçant les `:` par des `/` dans cette valeur, tandis que le nom du namespace des classes générées sera calculé en prenant ce qui est à droite du dernier `:` et en remplaçant tous les `/` par des `.`.

Les accesseurs de référence ne seront générés que si `kinetix` est renseigné.

### `referenceAccessorsName`

Nom des accesseurs de référence (préfixé par 'I' pour l'interface, puis 'Db' pour les accesseurs persistés). (La variable `{module}` aura toujours la transformation `flat` ajoutée).

_Valeur par défaut_ : `{module}ReferenceAccessors`

_Templating_ : `{module}` (la transformation `:flat` sera ajoutée à la fin)

_Variables par tag_ : **oui** (plusieurs accesseurs pourraient être générés si un fichier a plusieurs tags)

Les accesseurs de référence ne seront générés que si `kinetix` est renseigné.

### `mappersName`

Nom des mappers. (La variable `{module}` aura toujours la transformation `flat` ajoutée).

_Valeur par défaut_ : `{module}Mappers`

_Templating_ : `{module}` (la transformation `:flat` sera ajoutée à la fin)

### `useEFMigrations`

Utilise les migrations EF pour créer/mettre à jour la base de données.

_Valeur par défaut_ : `true`

### `dbSchema`

Nom du schéma de base de données à cibler. Si non renseigné, EF utilise le schéma par défaut du fournisseur (`dbo` pour SQL Server, `public` pour PostgreSQL).

_Templating_ : `{module}`

_Variables par tag_ : **oui** (à faire correspondre avec les valeurs de différents `modelPath`)

### `kinetix`

Ce qu'il faut générer pour Kinetix : `references` pour les annotations et accesseurs de listes de référence, `legacy` pour générer en plus des annotations de domaines, et `none` pour ne rien générer.

_Valeurs possibles_ : `none`, `references`, `legacy`

_Valeur par défaut_ : `references`

### `domainNamespace`

Namespace de l'enum de domaine pour Kinetix (mode legacy).

_Valeur par défaut_ : `{app}.Common`

### `valueTypes`

Types C# que le générateur doit considérer comme étant types valeurs (en plus des plus standard comme 'int', 'bool' ou 'DateTime'), qu'il faudra wrapper dans un `Nullable` (avec un `?`) pour les rendre nullables.

### `nullableEnable`

Prend en compte l'activation du paramètre `nullable: enable` dans le code généré (pour ajouter des `?` derrière les types références nullables).

### `requiredNonNullable`

Génère des types non-nullables pour les propriétés obligatoires.

_Variables par tag_ : **oui**

Idéalement, cette propriété est activée pour vos objets persistés et vos DTOs de lecture. Vous ne pouvez pas l'utiliser pour des DTOs d'écriture, car l'usage de types non-nullables (en particulier pour les types valeur) empêchera leur validation (il ne sera pas possible de distinguer un entier valant `0` d'un entier non renseigné, par exemple).

### `noColumnOnAlias`

Ne génère pas les attributs de colonnes sur les alias dans les classes non persistées.

_Valeur par défaut_ : `true`

EF Core n'en a pas besoin mais ça peut être utile pour d'autres ORMs afin de pouvoir mapper directement les colonnes depuis une requête SQL manuelle.

### `noPersistence`

Considère toutes les classes comme étant non persistées (= pas d'attribut SQL).

_Variables par tag_ : **oui**

Si `kinetix` est renseigné, les implémentations d'accesseurs de listes de références ne seront pas générées.

### `mapperTagsOverrides`

Si un mapper contient au moins une classe de ces tags, alors il sera généré avec les tags de cette classe (au lieu du comportement par défaut qui priorise les tags de la classe persistée puis de celle qui définit le mapper).

_Valeur par défaut_ : `[]`

### `mapperLocationPriority`

Détermine le type de classe prioritaire pour déterminer la localisation des mappers générés (`persisted` ou `non-persisted`).

_Valeurs possibles_ : `persisted`, `non-persisted`

_Valeur par défaut_ : `persisted`

### `useEFComments`

Annote les tables et les colonnes générées par EF avec les commentaires du modèle (nécessite `useEFMigrations`).

### `usePostgresEnums`

Utilise des enums Postgres pour les classes `enum: true` en base de données.

Ce paramètre génère une méthode d'extension `MapEnums()` à côté du DbContext, qu'il faudra appeler dans la configuration de `UseNpgsql()` de l'application pour que les enums soient prises en compte par Entity Framework (et Npgsql).

### `dotnetVersion`

Permet de spécifier la version de .NET cible, afin de savoir si certaines fonctionnalités sont disponibles pour la génération.

_Valeur par défaut_ : `10`

Si >= 8 :

- Utilise les constructeurs principaux pour la génération des classes avec dépendances (clients d'API, DbContext, accesseurs de références).
- Utilise les initialiseurs de collection quand c'est possible.

### `useRecords`

Utilise des records (mutables) au lieu de classes pour la génération de classes.

_Valeur par défaut_ : `true`

### `useCancellationTokens`

Ajoute un CancellationToken en paramètre des endpoints générés (client et serveur).

_Valeur par défaut_ : `false`

### `useAsyncReferenceAccessors`

Génère des accesseurs de référence asynchrones (pour Kinetix 10+).

_Valeur par défaut_ : `false`

### `resourcesResxPath`

Chemin vers les fichiers de ressources (`*.resx`) à générer pour les traductions de libellés de propriétés et de listes de références.

_Templating_ : `{module}`, `{lang}`

_Variables par tag_ : **oui**

### `resourcesInResx`

Précise les ressources à générer dans les fichiers resx (si `resourcesResxPath` est renseigné), au lieu de les générer en base de données.

_Valeurs possibles_ : `all`, `properties`, `references`

_Valeur par défaut_ : `all`

### `enumCols`

Génère les enums de nom de colonnes dans les classes persistées. Activé automatiquement si `dbContextPath` est `null` et si `kinetix` est renseigné.
