# Générer du code

La modélisation TopModel est le point d'entrée de son générateur `modgen`. L'outil va lire le modèle ainsi que le fichier de configuration associé, puis lancer les différents générateurs paramétrés.

## Installer `modgen`

`TopModel.Generator` est une application .NET 8 ou 9, packagée comme un [outil .NET](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

Pour l'utiliser, il faut avoir le [SDK .NET](https://dotnet.microsoft.com/download) installé sur votre machine, puis lancer la commande

```bash
dotnet tool install --global TopModel.Generator
```

Par la suite, pour mettre à jour TopModel, utiliser la commande :

```bash
dotnet tool update --global TopModel.Generator
```

## Système de filtre

Pour affiner le paramétrage des générateurs, `modgen` utilise un système de filtre par fichier.

Dans les données d'en-tête de chaque fichier, où sont précisés les imports (`uses`) et le module, le paramètre `tags` permet d'ajouter au fichier une liste de mots clés relatifs à ce fichier.

Exemple (à ne pas reproduire) dans le fichier `"Utilisateur.tmd"` :

```yaml title="Utilisateur.tmd"
---
module: Users
uses:
  - References
tags:
  - entity
---
```

Lorsque nous ajoutons un générateur, nous lui attribuons également une liste de tags.

Exemple (à ne pas reproduire) dans le fichier `"topmodel.config"` :

```yaml title="topmodel.config"
---
app: Hello World # Nom de l'application
sql:
  - tags:
      - entity
```

Lors de la génération, `modgen` enverra, à chacun des générateurs paramétrés, la liste des fichiers **dont au moins un tag correspond**.

Classiquement, on utilise les tags `back` et `front` pour différencier entre les fichiers back end et front end. Les fichiers contenant les `endpoints` contiennent les deux tags pour que soient générées les API clientes et server.

## Génération du projet

Nous allons enfin pouvoir générer le projet que nous avons bâti jusque là. La dernière étape consiste à ajouter la liste des `tags` à nos différents fichiers puis de mettre à jour le fichier `"topmodel.config"` en ajoutant les générateurs correspondants. (Les diverses champs des propriétés ne seront pas détaillés ici. Voir la documentation pour en savoir plus).

### Récapitulatif

Afin d'être sûr que vous ayez les mêmes éléments que ce tutoriel et éviter tout oubli, on récapitule d'abord le contenu de l'ensemble des fichiers auquel on ajoute la liste des tags. Copiez les si jamais vous avez un doute.

`"topmodel.config"` :

```yaml title="topmodel.config"
app: Hello World
modelRoot: ./
jpa:
  - tags: [back]
    outputDirectory: ./output/java
    apiGeneration: Server
csharp:
  - tags: [back]
    outputDirectory: ./output/csharp
    apiGeneration: Server
sql:
  - tags: [back]
    outputDirectory: ./output/sql
    procedural:
      crebasFile: 01_tables.sql
      indexFKFile: 02_fk_indexes.sql
      uniqueKeysFile: 03_unique_keys.sql
      initListFile: 04_references.sql
javascript:
  - tags: [front]
    outputDirectory: ./output/javascript
    apiMode: angular
    apiClientRootPath: api
    modelRootPath: model
```

`"Utilisateur.tmd"` :

```yaml title="Utilisateur.tmd"
---
uses:
  - References
module: Users
tags: [back]
---
class:
  name: Utilisateur
  comment: Utilisateur de l'application
  properties:
    - name: Id
      comment: Identifiant unique de l'utilisateur
      primaryKey: true
      domain: DO_ID

    - name: Email
      comment: Adresse mail de l'utilisateur
      domain: DO_EMAIL
      required: true
      label: Adresse mail

    - name: Nom
      comment: Nom de l'utilisateur
      domain: DO_LIBELLE
      label: Nom

    - name: DateInscription
      comment: Date d'inscription
      domain: DO_DATE
      label: Inscrit depuis le

    - association: TypeUtilisateur
      comment: Type de l'utilisateur
      label: Type
```

`"References.tmd"` :

```yaml title="References.tmd"
---
module: Refs
tags: [back, front]
---
class:
  name: TypeUtilisateur
  comment: Type d'utilisateur
  reference: true
  properties:
    - name: Code
      comment: Code du type d'utilisateur
      primaryKey: true
      required: true
      domain: DO_CODE

    - name: Libelle
      comment: Libellé du type d'utilisateur
      primaryKey: false
      required: true
      domain: DO_LIBELLE

  values:
    ADM: { Code: ADM, Libelle: Administrateur }
    GES: { Code: GES, Libelle: Gestionnaire }
    CLI: { Code: CLI, Libelle: Client }
```

`"Dto.tmd"` :

```yaml title="Dto.tmd"
---
uses:
  - Utilisateur
  - References
module: Users
tags: [back, front]
---
class:
  name: UtilisateurDto
  comment: Objet de transfert pour la classe Utilisateur
  properties:
    - alias:
        class: Utilisateur
        property: Nom
      suffix: true
    - composition: AdresseDto
      name: Adresse
      comment: Adresse de l'utilisateur
      domain: DO_PAGE
---
class:
  name: AdresseDto
  comment: Adresse
  properties: []
---
class:
  name: UtilisateurSearchResultDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas d'une recherche
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
    - alias:
        class: TypeUtilisateur
        include:
          - Libelle
      suffix: true
  mappers:
    from:
      - params:
          - class: Utilisateur
          - class: TypeUtilisateur
---
class:
  name: UtilisateurCreateDto
  comment: Objet de transfert pour la classe Utilisateur dans le cas d'une création
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
      prefix: true
  mappers:
    to:
      - class: Utilisateur
---
class:
  name: UtilisateurDetailDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas de la consultation de la page de détail
  properties:
    - alias:
        class: Utilisateur
        exclude:
          - Id
    - alias:
        class: TypeUtilisateur
        include:
          - Libelle
      suffix: true
  mappers:
    from:
      - params:
          - class: Utilisateur
          - class: TypeUtilisateur
---
class:
  name: UtilisateurUpdateDto
  comment: Objet de transfert pour la classe Utilisateur, dans le cas de la modification de celui-ci
  properties:
    - alias:
        class: Utilisateur
        include:
          - Nom
  mappers:
    to:
      - class: Utilisateur
```

`"domains.tmd"` :

```yaml title="domains.tmd"
---
module: Users # Module obligatoire, bien qu'inutile dans le cas des domaines
tags: # tags obligatoires, bien qu'inutiles dans le cas des domaines
  - ""
---
domain:
  name: DO_ID # Nom du domaine utilisé dans la définition des propriétés
  label: ID technique # Description du domaine
  ts:
    type: number # Type TS à utiliser pour ce domaine
  java:
    type: long # Type Java à utiliser pour ce domaine
  sql:
    type: int8
  csharp:
    type: long
---
domain:
  name: DO_DATE
  label: Date
  ts:
    type: string
  java:
    type: LocalDate
    imports:
      - java.time.LocalDate # Imports nécessaires au bon fonctionnement de la classe Java
  sql:
    type: timestamp
  csharp:
    type: DateTime
---
domain:
  name: DO_EMAIL
  label: Email
  length: 50 # Taille maximum de la chaine de caractères représentée par ce domaine
  ts:
    type: string
  java:
    type: String
  sql:
    type: varchar
  csharp:
    type: string
---
domain:
  name: DO_CODE
  label: Code
  length: 3
  ts:
    type: string
  java:
    type: String
  sql:
    type: varchar
  csharp:
    type: string
---
domain:
  name: DO_LIBELLE
  label: Libellé
  length: 15
  ts:
    type: string
  java:
    type: String
  sql:
    type: varchar
  csharp:
    type: string
---
domain:
  name: DO_PAGE
  label: Date
  ts:
    genericType: Page<{T}>
    imports:
      - "@/services/api-types" # Imports nécessaires au bon fonctionnement
  java:
    genericType: Page<{T}>
    imports:
      - "org.springframework.data.domain.Page" # Imports nécessaires au bon fonctionnement de la classe Java
  csharp:
    genericType: Page<{T}>
    imports:
      - "MyProject.Common.Page" # Imports nécessaires au bon fonctionnement de la classe C#
```

`"Endpoints.tmd"` :

```yaml title="Endpoints.tmd"
---
module: Users
uses:
  - Utilisateur
  - Dto
tags: [back, front]
---
endpoint:
  name: DeleteUtilisateur
  method: DELETE
  route: Utilisateur/{utilisateurId}
  description: Supprime un Utilisateur
  params:
    - alias:
        class: Utilisateur
        property: Id
---
endpoint:
  name: GetUtilisateur
  method: GET
  route: Utilisateur/{utilisateurId}
  description: Charge le détail d'un Utilisateur
  params:
    - alias:
        class: Utilisateur
        property: Id
  returns:
    composition: UtilisateurDetailDto
    name: detail
    comment: Le détail d'un Utilisateur
---
endpoint:
  name: CreateUtilisateur
  method: POST
  route: Utilisateur
  description: Créé un nouvel Utilisateur
  params:
    - composition: UtilisateurCreateDto
      name: detail
      comment: Le détail de l'utilisateur à créer
  returns:
    composition: UtilisateurDetailDto
    name: detail
    comment: Le détail de l'utilisateur créé
---
endpoint:
  name: UpdateUtilisateur
  method: PATCH
  route: Utilisateur/{utilisateurId}
  description: Modifie un Utilisateur
  params:
    - composition: UtilisateurUpdateDto
      name: detail
      comment: Le détail de l'utilisateur à modifier
    - alias:
        class: Utilisateur
        property: Id
  returns:
    composition: UtilisateurDetailDto
    name: detail
    comment: Le détail de l'utilisateur modifié
```

### Résultat de la génération

Il ne nous reste plus qu'à lancer la commande `modgen` depuis le terminal.  
Dans les logs, vous pouvez observer :

- Le numéro de la version TopModel utilisé
- La liste des Watchers enregistrés
- L'ensemble des fichiers créés, modifiés ou supprimés
  Des logs d'informations s'affichent sur le terminal, notamment les fichiers créés suite à la génération.

Dans votre répertoire projet, vous devriez voir la structure suivante apparaître :

`output/javascript`/  
├─ api/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ endpoints.service.ts  
└─ model/  
&nbsp;&nbsp;&nbsp;&nbsp;├─ refs/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ references.ts  
&nbsp;&nbsp;&nbsp;&nbsp;└─ users/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ adresse-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-create-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-detail-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-search-result-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ utilisateur-update-dto.ts  

`output/java/javagen/hello world`/  
├─ api/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ EndpointsController.java  
├─ dtos/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ AdresseDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurCreateDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDetailDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurSearchResultDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ UtilisateurUpdateDto.java  
├─ entities/  
│ ├─ refs/  
│ │ └─ TypeUtilisateur.java  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UsersMappers.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ Utilisateur.java  
└─ enums/  
&nbsp;&nbsp;&nbsp;&nbsp;└─ refs/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ TypeUtilisateurCode.java  

`output/csharp`/  
├─ Hello World.Refs.Models/  
│ └─ generated/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ TypeUtilisateur.cs  
├─ Hello World.Users.Models/  
│ ├─ Dto/  
│ │ └─ generated/  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ AdresseDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurCreateDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDetailDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurSearchResultDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;└─ UtilisateurUpdateDto.cs  
│ └─ generated/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UsersMappers.cs  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ Utilisateur.cs  
└─ Hello World.Web/  
&nbsp;&nbsp;&nbsp;&nbsp;└─ Controllers/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ Users/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ EndpointsController.cs  

`output/sql`/  
├─ 01_tables.sql  
├─ 02_fk_indexes.sql  
├─ 03_unique_keys.sql  
└─ 04_references.sql

## Générer du 'SQL' (postgresql)

Ajoutons tout d'abord un générateur SQL à notre application.

Dans le fichier de configuration `"topmodel.config"`, nous pouvons ajouter le générateur `sql`, en indiquant les fichiers de destination de la génération :

```yaml title="topmodel.config"
---
app: Hello World # Nom de l'application
sql: # Nom du générateur
  - tags: # Liste des tags des fichiers à filtrer pour ce paramétrage
      - back # Tag des fichiers contenant des classes persistées
    outputDirectory: ./sqlOutput/model/
```

### Scripts de créations des tables

Ajoutons au générateur postgres la configuration permettant de générer les fichiers de création de table, des indexes et des clés d'unicités

```yaml title="topmodel.config"
---
app: Hello World
sql:
  - tags:
      - back
    outputDirectory: ./sqlOutput/model/
    targetDBMS: postgre
    procedural:
      crebasFile: 01_crebas.sql
      indexFKFile: 02_index-fk.sql
      uniqueKeysFile: 03_uniq.sql
```

Dans le répertoire du projet, contenant le fichier `"topmodel.config"`, lancer la commande `modgen`.

Les fichiers attendus, `01_crebas.sql`, `02_index-fk.sql`, `03_uniq.sql` ont été créés dans les dossiers configurés.

### Scripts d'insertion des listes de référence

Bien qu'il soit possible de se contenter de compléter la configuration précédente, nous allons, pour l'exemple , ajouter un nouveau générateur postgresql pour générer les scripts d'initialisation des listes de référence.

Ajoutons donc un élément à la liste des générateurs posgresql :

```yaml title="topmodel.config"
---
app: Hello World
sql:
  - tags:
      - back
    outputDirectory: ./sqlOutput/model/
    targetDBMS: postgre
    procedural:
      crebasFile: 01_crebas.sql
      indexFKFile: 02_index-fk.sql
      uniqueKeysFile: 03_uniq.sql
  - tags:
      - back
    outputDirectory: ./sqlOutput/model/
    targetDBMS: postgre
    procedural:
      initListFile: 04_references.sql
```

Le fichier d'initialisation des listes de référence est créé.

Dans les logs, vous pouvez observer :

- Les autres fichiers, inchangés, n'apparaissent pas
- Deux watchers ont été enregistrés, `ProceduralSqlGen@1` et `ProceduralSqlGen@2`

Nous avons donc ajouté un générateur à notre modèle, puis généré le code correspondant.

### Résultat de la génération

Après le rajout du générateur `sql`, voici à quoi devrait ressembler votre répertoire final :

`output/javascript`/  
├─ api/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ endpoints.service.ts  
└─ model/  
&nbsp;&nbsp;&nbsp;&nbsp;├─ refs/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ references.ts  
&nbsp;&nbsp;&nbsp;&nbsp;└─ users/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ adresse-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-create-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-detail-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├─ utilisateur-search-result-dto.ts  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ utilisateur-update-dto.ts  

`output/java/javagen/hello world`/  
├─ api/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ EndpointsController.java  
├─ dtos/  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ AdresseDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurCreateDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDetailDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurSearchResultDto.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ UtilisateurUpdateDto.java  
├─ entities/  
│ ├─ refs/  
│ │ └─ TypeUtilisateur.java  
│ └─ users/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UsersMappers.java  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ Utilisateur.java  
└─ enums/  
&nbsp;&nbsp;&nbsp;&nbsp;└─ refs/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ TypeUtilisateurCode.java  

`output/csharp`/  
├─ Hello World.Refs.Models/  
│ └─ generated/  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ TypeUtilisateur.cs  
├─ Hello World.Users.Models/  
│ ├─ Dto/  
│ │ └─ generated/  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ AdresseDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurCreateDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDetailDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;├─ UtilisateurSearchResultDto.cs  
│ │ &nbsp;&nbsp;&nbsp;&nbsp;└─ UtilisateurUpdateDto.cs  
│ └─ generated/  
│ &nbsp;&nbsp;&nbsp;&nbsp;├─ UsersMappers.cs  
│ &nbsp;&nbsp;&nbsp;&nbsp;└─ Utilisateur.cs  
└─ Hello World.Web/  
&nbsp;&nbsp;&nbsp;&nbsp;└─ Controllers/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ Users/  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└─ EndpointsController.cs  

`output/sql`/  
├─ 01_tables.sql  
├─ 02_fk_indexes.sql  
├─ 03_unique_keys.sql  
└─ 04_references.sql

## Génération en continu

Dans le répertoire du projet, contenant le fichier `"topmodel.config"`, lancer la commande `modgen --watch` pour lancer la génération en continu. Ainsi, à chaque modification effectuée dans l'un des fichiers du modèle, la génération est relancée automatiquement !

## Générateurs disponibles

Voir la page [Génération](/generator) pour la liste des générateurs disponibles, ainsi que la documentation spécifique de chacun des générateurs pour connaître les paramétrages possibles.
