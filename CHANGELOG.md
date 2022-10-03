# TopModel.Generator (`modgen`)

## 1.14.3

- [`1040e99`](https://github.com/klee-contrib/topmodel/commit/1040e99940f981a1aa6b05ab6cc093424a67f6bd) - [JPA] Correction sqlName dans le cas surcharge du trigram dans la pk

## 1.14.2

- [`ca79975`](https://github.com/klee-contrib/topmodel/commit/ca79975663cd95f403419e72965259c30bfa014a) - [C#Gen] Fix génération enums sur les FK
- [`bcd76d4`](https://github.com/klee-contrib/topmodel/commit/bcd76d4485460cfbe0e61ae1a7349c2d3b26d80b) - Core : Correction récupération Label dans cas alias d'alias
- [`7ed8ef6`](https://github.com/klee-contrib/topmodel/commit/7ed8ef6c5f7c8409c8b0ced8ccab9a41122b2217) - [JPA] Correction utilisation trigram dans le cas des manyToMany
- [`d9dec2d`](https://github.com/klee-contrib/topmodel/commit/7ed8ef6c5f7c8409c8b0ced8ccab9a41122b2217) - [VSCode] Ne fonctionne pas sur des fichiers en LF si Environment.NewLine = 'CRLF'
- [`d9dec2d`](https://github.com/klee-contrib/topmodel/commit/ca1c1d4c4e05baffeb0055df81c1700c7f01d3c3) - [JPA] Ajout du générateur de resources

## 1.14.1

- [`74c99e99`](https://github.com/klee-contrib/topmodel/commit/74c99e99a7da1e5a923761d98ad5b047b1405414) - Retrait "GO" en trop si SSDT Postgres
- [`047be582`](https://github.com/klee-contrib/topmodel/commit/047be582120ce3196fa58e08725dc3a3ed2795c4) - [C#Gen] Récupération "AppName" propre depuis la config
- [`30fb0349`](https://github.com/klee-contrib/topmodel/commit/30fb0349aa06ee82b291e6f9507242fcc693d50b) - [modgen] Retrait message d'erreur en trop si on lance --version ou --help

## 1.14.0

- [#153](https://github.com/klee-contrib/topmodel/pull/153) - `modgen` avec configs multiples
- [`097cd3af`](https://github.com/klee-contrib/topmodel/commit/cc857e8c06d4ee3a3bb353b1dba5140fc3340899)...[`cb91c9bf`](https://github.com/klee-contrib/topmodel/commit/cb91c9bf7556f23c24467257f5751d6e8baf8254) - Surcharge locale de trigramme (fix [#128](https://github.com/klee-contrib/topmodel/issues/128), revert [#151](https://github.com/klee-contrib/topmodel/pull/151))
- [#154](https://github.com/klee-contrib/topmodel/pull/154) - [JPA] - Enum dans les values d'enum (fix [#152](https://github.com/klee-contrib/topmodel/issues/152))
- [`716191f4`](https://github.com/klee-contrib/topmodel/commit/716191f4e6a5c38eab50e4778aa8f62d9d76490a) - Fix watch avec des alias de classes et endpoints

## 1.13.5

- [#151](https://github.com/klee-contrib/topmodel/pull/151) - JPA/PG : Préfixer les PK dans le cas où les classes associées n'ont pas de trigram #150
- [`4a605e16`](https://github.com/klee-contrib/topmodel/commit/4a605e16c778e035fe529658841a0035c344ffd2) - [C#ApiGen] Gestion des mots clés réservés dans les noms de paramètres
- [`d1bdb2b3`](https://github.com/klee-contrib/topmodel/commit/d1bdb2b3e9f7bbc65d6a52bfc02f630c3ba04602) - homogénéisation snake_case/dash-case

## 1.13.4

- [`96c32208`](https://github.com/klee-contrib/topmodel/commit/96c3220825b392b36abd7cdea19dce163e8fd1e3) - [C#ServerApiGen] Fix génération contrôleur vide s'il n'y a que des alias d'endpoints

## 1.13.3

- [`831711dd`](https://github.com/klee-contrib/topmodel/commit/831711dd83c40f094e92cc51b58d0d703f42077d) - [C#] Fix Distinct() manquant sur DBContext

## 1.13.2

- [`8427cee2`](https://github.com/klee-contrib/topmodel/commit/8427cee2f53f39f6d4d8b24b2020608cf1bf41c8) - [SSDTGen] InitListMainScriptName facultatif
- [`1072f99b`](https://github.com/klee-contrib/topmodel/commit/1072f99b9b5aea2493a75df14e7f007ca5ff98ec) - [SSDTGen] Mode "postgres"

_Remarque : Générer du SSDT pour PostgreSQL n'a évidemment pas de sens, mais le générateur peut servir pour créer des scripts de définitions de tables et d'inserts complets sans tout regrouper et réordonner pour avoir un script général qui fonctionne en une seule fois._

## 1.13.1

- [`e3a8a3f1`]((https://github.com/klee-contrib/topmodel/commit/e3a8a3f1d45a3ee8a3d862fcaa4e0360406f30bd) JPA : Correction mapping associations - compositions

## 1.13.0

- [#148](https://github.com/klee-contrib/topmodel/pull/148) - Générateur de client d'API pour Angular (fix [#112](https://github.com/klee-contrib/topmodel/issues/112))
- [#149](https://github.com/klee-contrib/topmodel/pull/149) - Mappings sur les compositions (fix [#146](https://github.com/klee-contrib/topmodel/issues/146))

## 1.12.3

- [`bc2207fa`](https://github.com/klee-contrib/topmodel/commit/bc2207faf0a3b7e73c0e2f93e00fbf93d96ec53b) - JPA : Génération de toutes les associations réciproques des oneToMany.

## 1.12.2

- [`0b48d2ee`](https://github.com/klee-contrib/topmodel/commit/0b48d2eee444d849fb5ec1cf1aaa44df021e21f9) - Gestion des dates dans "values"

## 1.12.1

- [`1f017a3f`](https://github.com/klee-contrib/topmodel/commit/1f017a3fec6a1ad32596d9b29d797d1cf679a72f) - Hotfix schema.config.json

## 1.12.0

- [#18](https://github.com/klee-contrib/topmodel/pull/143) PG : Génération tenant compte des types d'associations (ManyToMany en particulier)
  - Adaptation de la génération JPA pour coller avec la génération PG
    - Ajout du mode de génération de clé primaire: `sequence`, `none` ou `identity`
    - Ajout du paramétrage du debut et de l'incrément de la séquence
- [#145](https://github.com/klee-contrib/topmodel/commit/9d588ebdc84a0c74cd3bf37c4982032504138cf6) JPA: compatibilité avec Java 11 (`instance of Class classe` n'est disponible qu'à partir de Java 16)
- [#142](https://github.com/klee-contrib/topmodel/pull/142) Prise en compte de length et scale dans le générateur pg
- [#144](https://github.com/klee-contrib/topmodel/commit/c2411a15022237142dd98c2bf1a7bdf2c081b091) JPA : Générer correctement l'entity Java si le name de la clé primaire ne commence pas par une majuscule

## 1.11.3

- [#140](https://github.com/klee-contrib/topmodel/pull/140) - [JPA] Différenciation des configuration de package (fix [#139](https://github.com/klee-contrib/topmodel/issues/139))
- [#137](https://github.com/klee-contrib/topmodel/pull/137) - Ajout du champ mediaType sur le domaine (fix [#133](https://github.com/klee-contrib/topmodel/issues/133))
- [`6639924b`](https://github.com/klee-contrib/topmodel/commit/6639924bebdb3d67ede1bf3796630773bba1bd92) - [C#Gen] Autorise {module} dans le nom de schéma DB + surcharge nom DbContext

## 1.11.2

- [`3dbbe392`](https://github.com/klee-contrib/topmodel/commit/3dbbe3922d15da6dd263fe63a25bd0725f6dc2a4) - [C# ClientGen] Fix vraiment les paramètres Guid cette fois-ci

## 1.11.1

- [`9e3b2b5e`](https://github.com/klee-contrib/topmodel/commit/9e3b2b5e1d63a7746cf355eccad18d2f5e0fe528) - Fix références "asListWithDomain" manquantes + mini fix C# client gen

## 1.11.0

- [#125](https://github.com/klee-contrib/topmodel/pull/125) - Endpoint Prefix et FileName (fix [#123](https://github.com/klee-contrib/topmodel/issues/123))
- [`25bdb766`](https://github.com/klee-contrib/topmodel/commit/25bdb7663d2588ba18b1c6e6b597ed499a82cb2b) - [C#] Fix génération client API avec un guid dans un query param
- [`29ec1c42`](https://github.com/klee-contrib/topmodel/commit/29ec1c42d1cde038cd663062eb2667ac3f70bd46) - [C#/JS] Gestion API avec plusieurs fichiers et des sous objets (avec FormData)
- [#135](https://github.com/klee-contrib/topmodel/pull/135) - JPA : Implémenter générateur Spring Api client (fix [#134](https://github.com/klee-contrib/topmodel/issues/134))

## 1.10.0

- [`0c0a0a2d`](https://github.com/klee-contrib/topmodel/commit/0c0a0a2d807cf243f839eda357f3550946c4086c) - `property` sur `association` pour spécifier la propriété cible (fix [#129](https://github.com/klee-contrib/topmodel/issues/129))
- [`7ca733cb`](https://github.com/klee-contrib/topmodel/commit/7ca733cbee0e524195e4485d0137dfe6b9cffff9) - Retrait de "asAlias" sur les associations (fix [#130](https://github.com/klee-contrib/topmodel/issues/130))

## 1.9.12

- [`60316754`](https://github.com/klee-contrib/topmodel/commit/60316754166386114c629059aa5087e407636949) - [JPA] Enum Shortcut gestion nullité
- [`428891ed`](https://github.com/klee-contrib/topmodel/commit/428891edfd5c9ca08ac0e4e15211aa268441e5fc) - [JPA] Simplification code généré api
- [`8d68d8b4`](https://github.com/klee-contrib/topmodel/commit/8d68d8b49042328b586d63dc830ff33e53ee7c6e) - [JPA] Suppression annotation RestController inutile

## 1.9.11

- [`e9f70af7`](https://github.com/klee-contrib/topmodel/commit/e9f70af7a069d3e265db09046e120a0e59ba6660) - [JS] Homogénéisation imports pour domaines/liste de ref

## 1.9.10

- [`7a04334`](https://github.com/klee-contrib/topmodel/commit/7a043346ad81e3e85e28d3a24d053e9b33785386) - [TSGen] Fix génération types enum dans les cas non standards

## 1.9.9

- [`766095e`](https://github.com/klee-contrib/topmodel/commit/766095e74ec6e87c12a872fee50215eb1feacd89) - [SSDT] Fix PK uuid manquante dans le table type

## 1.9.8

- [`54c3f84`](https://github.com/klee-contrib/topmodel/commit/54c3f844f6104357b5aec2d361ee21e5a1296650) - Fix divers quand autoGeneratedValue est un string (ex: uuid)

## 1.9.7

- [`75a535c`](https://github.com/klee-contrib/topmodel/commit/75a535c4a8365aafb11b5e4116761501723aada0) - Fix warning import non utilisé si non existant
- [`e722d41`](https://github.com/klee-contrib/topmodel/commit/e722d4159f3219ba14b69ad19ff581823e17fc0d) - [#116](https://github.com/klee-contrib/topmodel/issues/116) Fix plantage si classe non résolue dans un mapper
- [`f41f1fd`](https://github.com/klee-contrib/topmodel/commit/f41f1fd1a9b3264804d71d2bf1a67598bef7bd7e) - Fix [#120](https://github.com/klee-contrib/topmodel/issues/120) (erreurs mal positionnées sur les propriétés)
- [`5fae0b1`](https://github.com/klee-contrib/topmodel/commit/5fae0b134c9a7f94df4e02bf8316b403b11800cb) - Warning décorateur non utilisé ([#119](https://github.com/klee-contrib/topmodel/issues/119))
- [`fae6e99`](https://github.com/klee-contrib/topmodel/commit/fae6e9904e71438a330e2c28eb723aa892aaf6b0) - Amélioration détection mappings
- [`26a989b`](https://github.com/klee-contrib/topmodel/commit/26a989bb231dbb29b7f27c3dbb2523ee9ad72481) - PG: Suppression des anciens fichiers [#114](https://github.com/klee-contrib/topmodel/issues/114)

## 1.9.6

- [`32601d4`](https://github.com/klee-contrib/topmodel/commit/32601d4a8f27177144a0888223b438cf8f12400d) JPA - Ne plus utiliser le stream().toList() car la liste renvoyée est immutable.

## 1.9.5

[#113](https://github.com/klee-contrib/topmodel/pull/113) + [`43c67ef`](https://github.com/klee-contrib/topmodel/commit/43c67efbde63698b01e7c2a117235d94b324ff9e)

- Ajout de commentaires dans les mappers (définition + paramètres de mappers `from`)
- Implémentation C# de l'héritage de mappers

## 1.9.4

[`2547b6`](https://github.com/klee-contrib/topmodel/commit/2547b691ce42d67b2f66caaab6f1f50d43e56b0a) JPA :

- Ajout de la propriété `required` dans les requests params
- Passage de `emptyList` à `new ArrayList()` en cas de nullité de la liste

## 1.9.3

[`2547b6`](https://github.com/klee-contrib/topmodel/commit/2547b691ce42d67b2f66caaab6f1f50d43e56b0a) Fix génération topmodel.lock quand lancé depuis un autre répertoire

## 1.9.2

- [#110](https://github.com/klee-contrib/topmodel/pull/110) JPA - Gestion des associations réciproques au sein d'un même package racine

## 1.9.1

[#109](https://github.com/klee-contrib/topmodel/pull/109) Gestion de l'héritage des mappers.

Cette release ajoute la gestion de l'héritage aux **mappers**.

[Plus de détails dans la doc](https://klee-contrib.github.io/topmodel/#/model/mappers).

## 1.9.0

[#106](https://github.com/klee-contrib/topmodel/pull/106)

Cette release introduit la notion de **mappers** entre classes dans TopModel.

Pour une classe donnée, il est désormais possible de définir des mappers pour instancier cette classe à partir d'autres classes (`from`), ou bien pour instancier une autre classe à partir de cette classe (`t`o`). Les mappings entre propriétés sont déterminés automatiquement dès lors qu'une propriété est un alias d'une autre, ou à défaut si les propriétés ont le même nom et le même domaine. Il est évidemment possible de personnaliser le mapping au-delà de ce qui est déterminé automatiquement.

[Plus de détails dans la doc](https://klee-contrib.github.io/topmodel/#/model/mappers).

## 1.8.3

- JPA: Suppression du allocationSize pour optimisation récupération séquence en masse

## 1.8.2

- [#108](https://github.com/klee-contrib/topmodel/pull/108) [VSCode] Améliorations status bar TopModel #104

## 1.8.1

- [#105](https://github.com/klee-contrib/topmodel/pull/107) VSCode : Correction détection numéro de version et autoUpdate #105

> L'extension est maintenant capable de mettre à jour automatiquement TopModel, si le paramètre topmodel.autoUpdate est renseigné à true

- JS : Correction anomalie régression de la v1.8.0

## 1.8.0

- [#101](https://github.com/klee-contrib/topmodel/pull/101) et [5a15a7](https://github.com/klee-contrib/topmodel/commit/5a15a76141ec6ee17680f7629735e0364b8109ea)/[`2f59e4`](https://github.com/klee-contrib/topmodel/commit/2f59e413eb9a00f6a75e9bf9997df30a32d07aa6) - TopModel génère désormais un fichier `topmodel.lock` qui contient la version de TopModel utilisé par la dernière génération ainsi que la liste des fichiers générés. Cela permet :
  - D'avertir si la version de TopModel installée n'est pas celle avec laquelle le modèle a été généré la dernière fois (pour indiquer à l'utilisateur qu'il doit soit mettre à jour sa version de TopModel, soit mettre à jour son modèle s'il y a des breaking changes avec une nouvelle version)
  - De supprimer automatiquement les fichiers qui correspondant à des classes ou modules précédemment générés par TopModel qui ont été retirés du modèle.
- Bug fixes : [aa8bb02](https://github.com/klee-contrib/topmodel/commit/aa8bb02e5c7a4267a12b64663593a4bb2075cb44) et [7a1eb56](https://github.com/klee-contrib/topmodel/commit/7a1eb56139c7d73c5177f2360c0e70a25723aa3c)

## 1.7.1

[`0f161c6`](https://github.com/klee-contrib/topmodel/commit/0f161c6f216642e940da0718467b7cec8aec219b) - Inférence de default/order/flagProperty que si liste de ref + [C#] DefaultProperty explicite

## 1.7.0

[#97](https://github.com/klee-contrib/topmodel/pull/97) et [#98](https://github.com/klee-contrib/topmodel/pull/98)

- Les définitions de clés d'unicité et de valeurs de références utilisent désormais des références de propriétés, comme on utilise déjà par ailleurs pour les alias par exemple. Cela permet de remonter les erreurs de résolution de propriétés directement dans l'IDE au bon endroit, et d'avoir de l'autocomplétion sur les noms de propriétés.
- De même, `defaultProperty`, `orderProperty` et `flagProperty` utilisent désormais également des références de propriétés, et sont renseignées par défaut par une propriété `Label`/`Libelle` (resp. `Order`/`Ordre` et `Flag`), si elle existe sur la classe. Auparavant, certains générateurs utilisaient "en dur" Libelle si defaultProperty n'était pas renseignée.
- La propriété `autoGeneratedValue` a été ajoutée sur les domaines, pour pouvoir identifier proprement les clés primaires auto-générées au lieu d'un mélange de `Domain.Name != "DO_ID"` ou de `Domain.{lang}.Type == "string"` pas très heureux.
- Une nouvelle erreur a été ajoutée pour proprement identifier le besoin d'avoir une clé d'unicité sur un seul champ pour utiliser des valeurs de références en l'absence d'une clé primaire autogénérée.
- Les deux issues [#95](https://github.com/klee-contrib/topmodel/issues/92) et [#96](https://github.com/klee-contrib/topmodel/issues/92) ont été corrigées

### Breaking changes

- Il faut désormais utiliser le vrai nom de la propriété dans les définitions de clés d'unicités et de valeurs de références. Cela concerne les associations, pour lesquelles on utilisait le nom de classe (et du rôle s'il existe) à la place.
- Il faut ajouter la propriété `autoGeneratedValue: true` sur votre domaine `DO_ID` (que vous devez très certainement avoir)
- [C#] Les propriétés `Label`/`Libelle`/`Order`/`Ordre`/`Flag` étant désormais considérées par défaut comme propriétés par défaut/de tri/de flag, la génération des accesseurs de référence s'en voit impactée avec un tri par `Ordre`/`Order` ou `Label`/`Libelle` alors que rien n'a été spécifié dans le modèle. Ce comportement est tout à fait voulu, mais peut entraîner des différences avec le fonctionnement attendu (le tri par défaut de la base de données était par hasard le bon par exemple).

## 1.6.1

- [#92](https://github.com/klee-contrib/topmodel/issues/92) JPA Ajouter des constructeurs par recopie

## 1.6.0

- [#76](https://github.com/klee-contrib/topmodel/issues/76) Core : Ajouter une notion de décorateur de classe
- [#89](https://github.com/klee-contrib/topmodel/issues/89) Ajouter un générateur d'interfaces pour gérer les projections sur les dto
- [#67](https://github.com/klee-contrib/topmodel/issues/67) Garde-fou sur les doublons

## 1.5.7

- [`07aa8982`](https://github.com/klee-contrib/topmodel/commit/07aa898215c64dc99dfa1c1b98b0616f7d50b4af) Combo PLS HotFix : JPA : Générer des alias constructor #84

## 1.5.6

- [`07aa8982`](https://github.com/klee-contrib/topmodel/commit/07aa898215c64dc99dfa1c1b98b0616f7d50b4af) PLS HotFix : JPA : Générer des alias constructor #84

## 1.5.5

- [`07aa8982`](https://github.com/klee-contrib/topmodel/commit/07aa898215c64dc99dfa1c1b98b0616f7d50b4af) HotFix : JPA : Générer des alias constructor #84

## 1.5.4

- [`07aa8982`](https://github.com/klee-contrib/topmodel/commit/07aa898215c64dc99dfa1c1b98b0616f7d50b4af) JPA : Générer des alias constructor #84

## 1.5.3

- [`79305ff`](https://github.com/klee-contrib/topmodel/commit/79305ff8f65f50e1025ebb2b3d3ede32ee6efeaa) Gestion de la biderectionnalité avec choix du sens arbitraire (fix #82) Cas des oneToMany

## 1.5.2

- [`5f379b`](https://github.com/klee-contrib/topmodel/commit/5f379b3bf9b2d243a9af51bb05a5ddf0927bd7d8) Gestion de la biderectionnalité avec choix du sens arbitraire (fix #82)

## 1.5.1

- [`5f379b3b`](https://github.com/klee-contrib/topmodel/commit/5f379b3bf9b2d243a9af51bb05a5ddf0927bd7d8) JPA : Retirer la dépendance à Lombok (fix #77)
- [`4fea1d49`](https://github.com/klee-contrib/topmodel/commit/4fea1d497b848da6780a83fc2f6421401b63f450) JPA : Générer les associations manyToMany avec des tables de correspondances identiques (fix #80)
- [`2268df33`](https://github.com/klee-contrib/topmodel/commit/2268df33cef6d7eb374bbb89cdc146915ba540e5) Warning sur les domaines non utilisés (fix #73)
- [`c83420e7`](https://github.com/klee-contrib/topmodel/commit/c83420e7c7451e007a0258d85b0b4d19cb31dfb3) Warning sur l'ordre des paramètres d'un endpoint (fix #74)
- [`35c04911`](https://github.com/klee-contrib/topmodel/commit/35c04911081166715946564aa38419f4d5997155) Ignore les fichiers vides (fix #75)

## 1.5.0

- [`550179a9`](https://github.com/klee-contrib/topmodel/commit/550179a9fa45b821538231f8426d1fc85711eea6) VSCode : Prévisualisation du schéma UML

## 1.4.2

- [JPA] : Correction import enum suite release 1.4.1

## 1.4.1

- [JPA] : Correction import enum suite release 1.4.0

## 1.4.0

- [`944b659e`](https://github.com/klee-contrib/topmodel/pull/70) [JPA] : Pour les listes de référence, mettre la FK plutôt que la composition #69
- [`2f057ab7`](https://github.com/klee-contrib/topmodel/pull/68) [JPA] : Bug sur les alias d'association #22
- [`ec6709ae`](https://github.com/klee-contrib/topmodel/pull/64) [Core] : Pluriels et types d'associations #64

## 1.3.4

- [`b344ca01`](https://github.com/klee-contrib/topmodel/commit/b344ca01dc7f47c504dfd75d05fc102cff0f4ecb) [JPA] : EqualsAndHash dans hashset #61

## 1.3.3

- [`5828b26e`](https://github.com/klee-contrib/topmodel/commit/5828b26e8a35b23a448ec7352097ad4d042304c7) [JPA] : Mauvaise génération oneToMany #59
- [`5828b26e`](https://github.com/klee-contrib/topmodel/commit/5828b26e8a35b23a448ec7352097ad4d042304c7) [JPA] : Le EqualsAndHashCode devrait se base sur l'ID uniquement #60

## 1.3.2

- [`6cdc19f`](https://github.com/klee-contrib/topmodel/commit/6cdc19f414ec2606df51d173438b2193e1217d21) [JS] Fix API CLient options non utilisé dans le cas de téléversement

## 1.3.1

- [`95ac1b0`](https://github.com/klee-contrib/topmodel/commit/95ac1b025eb9655ea5c5fb07a3954e995f807174) [JS] Fix API CLient dans le cas d'un téléversement de fichier
- [`065ce7d`](https://github.com/klee-contrib/topmodel/commit/065ce7d160660efa39fae23dbd7ba556f615b66e) [JPA] Feat. Upload de Multipart File
- [`927a846`](https://github.com/klee-contrib/topmodel/commit/927a846b11bd6757e7b185cbd9eb9deb26f2ddb1) [Core] Erreur si une propriété est référencée plusieurs fois dans le même al

## 1.3.0

Version d'accompagnement de la release 1.3.x de l'extension VSCode. Contient de nombreuses améliorations internes pour aider le suivi des références de domaines et classes. Aucun impact n'est attendu sur la génération.

Un nouveau warning a été ajouté pour détecter les doublons de trigramme. Les warnings sont désormais désactivables unitairement via la propriété `noWarn` du fichier de config (valable pour la génération et l'extension VSCode).

## 1.2.11

- [`2788644`](https://github.com/klee-contrib/topmodel/commit/27886449dc529aa1a7a4e229efb0892978e65fb9) [C#] Fix génération classe persistée/DbContext avec des alias

## 1.2.10

- [`b127ba6`](https://github.com/klee-contrib/topmodel/commit/b127ba6b51abd3e08c847a5e062f57120afa3ab6) [JS] Ajout du as const si non mode focus

- [`2e5a932`](https://github.com/klee-contrib/topmodel/commit/2e5a932e1a4ffa05ea379dcec4d6cec31f0500bd) [JS] Correction import liste de ref depuis un sous-module

## 1.2.9

- [`fb3ccc6`](https://github.com/klee-contrib/topmodel/commit/fb3ccc6fdcb5e57636848d4e77633b46a1f61bfc) [C#Gen] Mise à jour nom package Kinetix.

## 1.2.8

- [`869f980`](https://github.com/klee-contrib/topmodel/commit/869f980323727add90afbee892fab27a9586dde3) [C#Gen] Encore un fix sur les usings générés à la marge.

## 1.2.7

- [`fb70003`](https://github.com/klee-contrib/topmodel/commit/fb70003c060d27afa9d0ca4f4bfeb3b27ac49eb2) Fix imports en trop en C# si pas de field properties dans le fichier.

## 1.2.6

- [`b648f4f`](https://github.com/klee-contrib/topmodel/commit/b648f4fe93190ecfd607b616b46fe6a1a6a3150f) JPA - Gérer l'héritage [#38](https://github.com/klee-contrib/topmodel/issues/38)

## 1.2.5

- [`3c83c59`](https://github.com/klee-contrib/topmodel/commit/3c83c59dc36ed159f7c60a6a4d630d178ed5b1c6) JPA - Améliorations persistence :

  - OneToOne : ajout du paramètre `optional` dépendant de la propriété `required`
  - OneToOne : ajout de la contrainte d'unicité sur la colonne qui fait l'objet de la oneToOne
  - Listes de références : les colonnes ont `updatables = false`
  - ManyToOne : ajout du paramètre `optional` dépendant de la propriété `required`
  - ManyToMany : Ajout FetchType.LAZY

- [`2596d4f`](https://github.com/klee-contrib/topmodel/commit/2596d4ffbdd09bae0c327ad6efc717fc43dd40a2) Core : Typage des erreurs [#34](https://github.com/klee-contrib/topmodel/issues/34)

- [`7eeff2f`](https://github.com/klee-contrib/topmodel/commit/7eeff2f38e9bc47f0a2f272fb145fdc780b660ed) Template de type pour les domaines de composition

## 1.2.4

- [`8139acc`](https://github.com/klee-contrib/topmodel/commit/8139acc971fa6214164f841ac37df9933283293d) JS - Mauvais import lorsqu'on référérence un type dans un sous-module [#26](https://github.com/klee-contrib/topmodel/issues/26)

- [`2804a70`](https://github.com/klee-contrib/topmodel/commit/2804a70b308ffaf0c8e853655400428242d0c686) Core : Doit remonter une erreur si un import est dupliqué [#24](https://github.com/klee-contrib/topmodel/issues/24)

## 1.2.3

- [`1c783b6`](https://github.com/klee-contrib/topmodel/commit/1c783b66d2bbd73d010a11ababcd9844e7d25af4) JPA - Ajout du cascade type dans les oneToOne et généralisation de l'utilisation des Set.

## 1.2.2

- [`4de0ae7`](https://github.com/klee-contrib/topmodel/commit/4de0ae72a00b35729d90949c57f915db35e066c8) JPA - Fix referenced column dans les associations oneToOne qui ont un rôle

## 1.2.1

- [`b5c7d25`](https://github.com/klee-contrib/topmodel/commit/b5c7d2553c0bea9d0e65df43a360cab817305913) JS - Api client : Correction import des liste de ref si elles se trouvent dans un sous-module

## 1.2.0

- [`192dbf6`](https://github.com/klee-contrib/topmodel/commit/192dbf67e68da23871956d3fc05e667103602d4f) JS - Ajout du mode `VALUES` pour la génération des listes de ref

- [`a839236`](https://github.com/klee-contrib/topmodel/commit/a839236ea7012961f5761e56934502e953c19bce) (Issue [#7](https://github.com/klee-contrib/topmodel/issues/7)) "reset" des alias déjà résolus à chaque appel de "ResolveReferences"

- [`2a5bff1`](https://github.com/klee-contrib/topmodel/commit/2a5bff154869787f2e637bcdc62b9af0af70984c) (Issue [#17](https://github.com/klee-contrib/topmodel/issues/17)) [C#Gen] Utilisation du module pour déterminer le répertoire de génération des APIs

  `apiPath` a été divisé en `apiRootPath` et `apiFilePath`, ce qui permet de gérer proprement les modules dans les répertoires côté serveur (parce qu'il faut insérer un "/Controllers/" entre les deux paramètres). On n'utilise du coup plus le chemin des fichiers de modèles du tout.

- [`9d59eb0`](https://github.com/klee-contrib/topmodel/commit/9d59eb0b6c70f87167f22154f2c04f582ee4658c) (Issue [#17](https://github.com/klee-contrib/topmodel/issues/17)) JS - API dans le répertoire du sous-module

  N'utilise non plus plus le chemin des fichiers de modèle, et un paramètre `apiClientFilePath` a été ajouté gérer les sous-répertoires en fonction du module.

- [`c83b32c`](https://github.com/klee-contrib/topmodel/commit/c83b32c73247b39324194949d9a47317908076fc) (Issue [#13](https://github.com/klee-contrib/topmodel/issues/13)) JS : Mauvais import lorsque les fichiers d'API sont dans des sous répertoires avec une profondeur > 1

## 1.1.1

- [`7a47a3d`](https://github.com/klee-contrib/topmodel/commit/7a47a3d3c347e3896b9aa7e6748b5050e28df0b5) - JS - Fix majuscule dans le nom de package

## 1.1.0

- [`31050e3`](https://github.com/klee-contrib/topmodel/commit/31050e3114fd2808856572946a612545fa15e13b) - JPA-JS : Fix UpperCase in submodules
- [`040abc3`](https://github.com/klee-contrib/topmodel/commit/040abc3183ae26a52d25fae01ca5369a546475df) - JPA : Génération des fields enum
- [`c41c729`](https://github.com/klee-contrib/topmodel/commit/c41c729dd166c22fcbd86a72788196210de26139) - [C#Gen] Maj namespaces Kinetix SQL

## 1.0.1

- [`edb61d1`](https://github.com/klee-contrib/topmodel/commit/edb61d13080cd6d11e1df36c437f8248c0b95309) - SQL - SQL Name

  Le nom des champs en base de données (SqlName) pour les alias persistés de classes persistées prend désormais en compte la valeur du suffixe et du préfixe éventuellement associés à l'alias.

## 1.0.0

Version initiale.
