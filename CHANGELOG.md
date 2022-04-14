# TopModel.Generator (`modgen`)

## 1.5.4

- [07aa8982](https://github.com/klee-contrib/topmodel/commit/07aa898215c64dc99dfa1c1b98b0616f7d50b4af) JPA : Générer des alias constructor #84

## 1.5.3

- [79305ff](https://github.com/klee-contrib/topmodel/commit/79305ff8f65f50e1025ebb2b3d3ede32ee6efeaa) Gestion de la biderectionnalité avec choix du sens arbitraire (fix #82) Cas des oneToMany

## 1.5.2

- [5f379b](https://github.com/klee-contrib/topmodel/commit/5f379b3bf9b2d243a9af51bb05a5ddf0927bd7d8) Gestion de la biderectionnalité avec choix du sens arbitraire (fix #82)

## 1.5.1

- [5f379b3b](https://github.com/klee-contrib/topmodel/commit/5f379b3bf9b2d243a9af51bb05a5ddf0927bd7d8) JPA : Retirer la dépendance à Lombok (fix #77)
- [4fea1d49](https://github.com/klee-contrib/topmodel/commit/4fea1d497b848da6780a83fc2f6421401b63f450) JPA : Générer les associations manyToMany avec des tables de correspondances identiques (fix #80)
- [2268df33](https://github.com/klee-contrib/topmodel/commit/2268df33cef6d7eb374bbb89cdc146915ba540e5) Warning sur les domaines non utilisés (fix #73)
- [c83420e7](https://github.com/klee-contrib/topmodel/commit/c83420e7c7451e007a0258d85b0b4d19cb31dfb3) Warning sur l'ordre des paramètres d'un endpoint (fix #74)
- [35c04911](https://github.com/klee-contrib/topmodel/commit/35c04911081166715946564aa38419f4d5997155)  Ignore les fichiers vides (fix #75)

## 1.5.0

- [550179a9](https://github.com/klee-contrib/topmodel/commit/550179a9fa45b821538231f8426d1fc85711eea6) VSCode : Prévisualisation du schéma UML

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
