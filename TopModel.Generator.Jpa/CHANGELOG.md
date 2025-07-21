# Changelog JPA

## 1.10.2

- [0ee4d3](https://github.com/klee-contrib/topmodel/commit/0ee4d349c7db207cf7003f117cf9942acc58b7a8) [JPA] Fix default value openApiAnnotations

## 1.10.1

- [f6bd96](https://github.com/klee-contrib/topmodel/commit/f6bd9633f14aac0df62825038b28ed020544a6bc) [JPA] OpenApiAnnotations : le request body nécessite l'annotation RequestBody

## 1.10.0

- [f61389](https://github.com/klee-contrib/topmodel/commit/f61389d0660b6a67051769d3eb22a89f850b59fb) [JPA] Ajout de l'option `openApiAnnotations`
- [ffe32e](https://github.com/klee-contrib/topmodel/commit/ffe32e982c09818a0bc23cc43b598895cfff7ccb) [JPA] Trim end "/" pour les routes

## 1.9.7

- [63834f](https://github.com/klee-contrib/topmodel/commit/63834f3068347784ee9d8db3122318e526113c95) [JPA] Ordre déterministe sur les annotations : de la plus courte à la plus longue. Cette évolution permet d'éviter les modifications d'ordre entre deux versions

- ['486'](https://github.com/klee-contrib/topmodel/pull/486) [JPA] Ajout de l'anotation valid si le parameter est de type ModelAttribute

## 1.9.6

- [ab2775](https://github.com/klee-contrib/topmodel/commit/ab27750932461a677faa08638f08b95d3c30e70d) [JPA]Fix annotation Id Enums as Enums dans le cas d'une PK composite

## 1.9.5

- [8ff538](https://github.com/klee-contrib/topmodel/commit/8ff538c2e8c5cf43b3eaf2a4b5bef9298660da7e) [JPA] Gestion Enums as Enums dans le cas d'une PK composite

## 1.9.4

- [7d8ab0a](https://github.com/klee-contrib/topmodel/commit/7d8ab0a512e2fe6afb2485f8ad9e126e027416f6) [JPA] Les décorateurs ne sont pas pris en compte pour les enums dans le mode EnumsAsEnums

## 1.9.3

- [b0990cf](https://github.com/klee-contrib/topmodel/commit/b0990cf5c38e056728f0454082e4ecf2090153fa) [JPA] Fix annotation Enum pour cas EnumsAsEnum pour un association

## 1.9.2

- [56b67f](https://github.com/klee-contrib/topmodel/commit/56b67f6d2148bcaf5a97357b97f76d2965dac65b) [Générateur JPA] Problème dans la génération d'enums simples en mode enumsAsEnums
  Fix [#485](https://github.com/klee-contrib/topmodel/issues/485)
- [0e4b82](https://github.com/klee-contrib/topmodel/commit/0e4b82d549594a3ac380fb6e1bfd83b8a4a88c01) [JPA] Fix commentaires & ajout annotation @Serial

## 1.9.1

- ['484'](https://github.com/klee-contrib/topmodel/pull/484) [JPA] décalage entre le nom du package dans l'import et l'emplacement pour les enumerations

## 1.9.0

- [`a2eddb`](https://github.com/klee-contrib/topmodel/commit/a2eddb5ad2f70481c9de7c07d3d0c2be20cb8935) - [JPA] Mappers : polymorphisme des méthodes générées
  Fix: [#477](https://github.com/klee-contrib/topmodel/issues/477)
  
## 1.8.3

- [480](https://github.com/klee-contrib/topmodel/pull/480) Ajoute annotation dans le cadre d'un alias sur une enum pour une clé composite
  fix #478

## 1.8.2

Prise en compte correcte des `extends` de décorateurs de décorateurs (modgen 2.8).

## 1.8.1

- [`2fb8b73`](https://github.com/klee-contrib/topmodel/commit/2fb8b73ba4487b94628116d001ef2478406584da) - Fix breaking change involontaire sur les générateurs suite à modgen 2.7

## 1.8.0

Compatibilité avec les fallbacks de langage d'implémentation de TopModel 2.7

## 1.7.2

- [3d36a6](https://github.com/klee-contrib/topmodel/commit/3d36a6360c7514e43ed959e673ba7dae8ac00247) [JPA] Fix Check source null dans les cas de mappings de compositions

## 1.7.1

- [19e63c](https://github.com/klee-contrib/topmodel/commit/19e63c6b7bffaa554f87dacd67c429aede256ba7) [JPA] Check source null dans les cas de mappings de compositions

## 1.7.0

- [e2b8](https://github.com/klee-contrib/topmodel/commit/e2b89422256eb8c58f3718229f2258f6d74b6d09) [JPA] Donner la possibilité de définir le nom du dao généré

- [948ae](https://github.com/klee-contrib/topmodel/commit/948aeaab3170a452d58e594a38fa8bc9df7ff4e6) [JPA] Protection contre la nullité dans le cas de certains mappers

## 1.6.10

- [f750d](https://github.com/klee-contrib/topmodel/commit/f750de99be706336c994f209870feaa3450f777a) [JPA] Fix imports enums

## 1.6.9

- [65e08](https://github.com/klee-contrib/topmodel/commit/65e0872c0190bb93f779405f8254c58962d6ad4f) [JPA] Corrections imports & jdbc mode

## 1.6.8

- [42efb3](https://github.com/klee-contrib/topmodel/commit/42efb3828c66486244ee715194aebb1d7b72abcc) [JPA] Fix import manquant dans le cadre d'une association persistée vers une classe d'un autre module

## 1.6.7

- [c1a3b2](https://github.com/klee-contrib/topmodel/commit/c1a3b24fc33fc741676fdecb1126e8d9adfabd62) [JPA] Fix import alias d'association d'enum

## 1.6.6

- [ccc854](https://github.com/klee-contrib/topmodel/commit/ccc85468600ee0c698ec7fce6b77580b093b7caf) Fix : [JPA] Association non persistée [#459](https://github.com/klee-contrib/topmodel/issues/459)

## 1.6.5

- [d3ed6a](https://github.com/klee-contrib/topmodel/commit/d3ed6ae2aec14dfee9a179661d4ff69ad1de9945) [JPA] Support pour la génération d'id en mode UUID
  Fixes [#457](https://github.com/klee-contrib/topmodel/issues/457)

## 1.6.4

- [3b194](https://github.com/klee-contrib/topmodel/commit/a90ecffa2b93af5244d4ee2d5ca24c0756b0f8a4) [JPA] Fix imports enum dans les controllers

- [a90ec](https://github.com/klee-contrib/topmodel/commit/a90ecffa2b93af5244d4ee2d5ca24c0756b0f8a4) [JPA] Ajout des espaces entre le if et sa parenthèse.
  Fix enumAsEnum dans le cas manyToMany explicites

## 1.6.3

- [a100a](https://github.com/klee-contrib/topmodel/commit/a100aa225259ce2a71564f95f8b97193c0e5f2b5s) [JPA] Ajout de l'annotation 204 si le controller renvoie void ou Void

## 1.6.2

- [cf7bb0](https://github.com/klee-contrib/topmodel/commit/cf7bb01c78ca15ac9a965fd3b78aec6b312d6299) [JPA] EnumsAsEnums fix génération constructeur

## 1.6.1

- [3133a](https://github.com/klee-contrib/topmodel/commit/3133a91296e0ce50ded4e4b5e60cfa64f50175f7) [JPA] Fix génération mappers

## 1.6.0

- [bfce6](https://github.com/klee-contrib/topmodel/commit/bfce65f7f62e47a5c43fc2759ab11b9194fec928) Implémentation JPA utilisation de :path

**Breaking change**: Les configuration de chemin (`EntitiesPath`, `dtosPath`, `apiPath` etc) qui contiennent des `.` doivent maintenant être remplacées par des `/`. La partie du chemin après le `:` est toujours le nom du package cible.

Exemple : `javagen:topmodel.jpa.sample.demo.entities` devient `javagen:topmodel/jpa/sample/demo/entities`

Par ailleurs, il est possible de définir des variables dans la configuration de la génération. Ces variables peuvent être utilisées dans les chemins de sortie. Si ces variables contiennent des `.`, ils peuvent automatiquement être remplacés par des `/` avec l'opérateur `:path`. De même, si un path contenait le mot réservé `module`, il faut désormais le remplacer par `{module:path}`, encore une fois pour remplacer les `.` par des `/`.

Exemple : `javagen:{app:path}/entities/{module:path}`

## 1.5.6

- [32c1e](https://github.com/klee-contrib/topmodel/commit/32c1e21f1a20940a25670fda12ab1c080f7366db) [JPA] : Erreur lors de l'ajout d'un prefix sur une interface FeignClient

Fix [#447](https://github.com/klee-contrib/topmodel/issues/447)

## 1.5.5

- [06a1a](https://github.com/klee-contrib/topmodel/commit/06a1a02dbfb8361e918744f588d82de084805b13) [JPA] Erreur lorsqu'on génère une entité persistée alors que le type sql n'est pas défini
  Fix [#446](https://github.com/klee-contrib/topmodel/issues/446)

## 1.5.4

Release technique

## 1.5.3

- [4add3](https://github.com/klee-contrib/topmodel/commit/4add3156138fae8612ff1f511a4c460d929c00ea) [JPA] Incompatibilité entre décorator et référence Fix [#441](https://github.com/klee-contrib/topmodel/issues/441)

> **Attention** : Nécessite modgen v2.4.2

## 1.5.2

- [af781](https://github.com/klee-contrib/topmodel/commit/af7812819384051ca347c2c207fdc4f26ea7fb5a) [JPA] annotation @Digits incomplète. Fix [#440](https://github.com/klee-contrib/topmodel/issues/440)

## 1.5.1

- [f54d39](https://github.com/klee-contrib/topmodel/commit/f54d3932d6cf316ed6d49b0f006fd4572bf826a8) [JPA] Fix annotation digits && size

## 1.5.0

- [e48e5a](https://github.com/klee-contrib/topmodel/commit/e48e5a0e1ec3ab5634cb0ac4d5af78c55c8bfae7) [JPA] Ajouter les annotations de validations @Size ou @Digits aux Dtos des propriétés dont les domains définissent Length ou Scale
  Fix #437

> BREAKING CHANGE : Certaines api risquent de renvoyer des erreurs `400`

## 1.4.0

Compatibilité avec `ignoredFiles` de TopModel 2.4

## 1.3.0

- [#434](https://github.com/klee-contrib/topmodel/pull/434) - Enumérations représentant totalement l'entité. Ajout du mode `enumsAsEnums`

## 1.2.5

- [48855](https://github.com/klee-contrib/topmodel/commit/48855ffee1a89e8157770b720c3dbc72aa563241) [JPA] Fix api client génériques

## 1.2.4

- [afbcb](https://github.com/klee-contrib/topmodel/commit/afbcbd1801f334cf660dfceba430ec0090eb8bbd) [JPA] Amélioration typage méthode, annotation et paramètre Fix #429, #430

## 1.2.3

- [deeed](https://github.com/klee-contrib/topmodel/commit/deeed8d1703ee0009d642b681469f01a65abcabb) [JPA] annotation @NotNull non générée Fix #428

## 1.2.2

- [90a2c](https://github.com/klee-contrib/topmodel/commit/90a2c757fd7580dddd0bce938374db702db853a2) [JPA] Fix génération séquence

## 1.2.0

- [4e3b6b](https://github.com/klee-contrib/topmodel/commit/4e3b6b7072937a40aa0717b585c0bf231908d5e7) [JPA] Le spring client doit retourner un ResponseEntity (pour gérer les différents codes Http)

> BREAKING CHANGE: Les api clientes générées avec `SpringClient` ne renvoient plus l'objet `D` directement, mais un objet `ResponseEntity<D>`.

## 1.1.3

- [8e6c7e](https://github.com/klee-contrib/topmodel/commit/8e6c7e91211edf29108254fc0aef630157c69c90) [JPA] Fix annotation Column sur association manyToMany
- [a8cfea](https://github.com/klee-contrib/topmodel/commit/a8cfea155d4bcfe6d196cd9e8de2e529d16dea98) [JPA] Fix import alias de composition dans un module distant

## 1.1.2

- [429ffe](https://github.com/klee-contrib/topmodel/commit/429ffe4fc8c135fa4b13d36300b7875b7568206d) [JPA] Suppression de l'initialisation des newable types pour les dtos

## 1.1.1

- [95ebd](https://github.com/klee-contrib/topmodel/commit/95ebd7e79a6e482aaa6b7268fd6efe3084a26ff1) [JPA] Suite refacto

## 1.1.0

- [5849b8](https://github.com/klee-contrib/topmodel/commit/5849b8a30aa2a69954bc9fd2a6d2957ae1c52a82) [JPA] Créer un mode FeignClient dans le générateur d'API client
  Fix #419

- [db1f14](https://github.com/klee-contrib/topmodel/commit/db1f14fd5aa5e71f9667a448f419dfa5838b42dc) [JAVA] annotation absente sur un champ issu d'une composition #414

Breaking changes :

- Suppression du mode `enumShortcut`
- Les DAO des listes de références ne sont plus générés. La première génération risque de les supprimer
  - Annuler la suppression des DAO utilisés. Normalement, il y en a peu, d'où la suppression de la génération automatique...

## 1.0.11

- [d31beb](https://github.com/klee-contrib/topmodel/commit/d31beb5e0d42178e62f6b19316abcbbccde8884d) Fix Initialisation enum dans le cas d'alias ou d'association : cas null

## 1.0.10

- [acddcfe](https://github.com/klee-contrib/topmodel/commit/acddcfe1ed07577a7188768d674ee805764da6d4) Fix Initialisation enum dans le cas d'alias ou d'association

## 1.0.9

- [`e01da3f`](https://github.com/klee-contrib/topmodel/commit/e01da3f1d3b8c0dc39fe1eb8e206b953efb4b882) Problème import java entre deux classes générées Fix #398

## 1.0.8

- [`ab967cd`](https://github.com/klee-contrib/topmodel/commit/ab967cd621e914618d141d62d5182f113fbc306c) Correction converter dans le cas de composition

## 1.0.7

- [#395](https://github.com/klee-contrib/topmodel/pull/395) - Accolades sur le "if liste null".

## 1.0.6

- [`e0f01b8e`](https://github.com/klee-contrib/topmodel/commit/e0f01b8ea3d404aa196cfacd85f85564462bf581) Correction régression nullable

## 1.0.5

- [`97bc094a`](https://github.com/klee-contrib/topmodel/commit/97bc094a94e52167fd0bb86d1aca5308dbfc0593)
  - Enums :
    - Les setters ne sont plus générés
    - Les valeurs sont placés en premier
    - Ajout de l'annotation `@Transiant`
    - Les DAOS ne sont plus générés
    - Les `;` en fin d'enum ne sont plus générés lorsqu'ils sont inutiles
  - L'attribut `nullable` n'est plus renseigné lorsqu'il s'agit de la valeur par défaut

BREAKING CHANGES : - Les setters ne sont plus générés - les DAOS n'étant plus générés, ceux existant seront supprimés à la première génération

## 1.0.4

- [`aafe5e0c`](https://github.com/klee-contrib/topmodel/commit/aafe5e0c0b286a610e783d41d06da9ff74232c6a) - Fix formattage hashcode

## 1.0.3

Version initiale.
