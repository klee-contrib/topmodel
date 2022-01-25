# TopModel.Generator (`modgen`)

## 1.2.4

- [8139acc971fa6214164f84](https://github.com/klee-contrib/topmodel/commit/8139acc971fa6214164f841ac37df9933283293d) JS - Mauvais import lorsqu'on référérence un type dans un sous-module [#26](https://github.com/klee-contrib/topmodel/issues/26)

- [2804a70b308ffaf0c](https://github.com/klee-contrib/topmodel/commit/2804a70b308ffaf0c8e853655400428242d0c686)  Core : Doit remonter une erreur si un import est dupliqué [#24](https://github.com/klee-contrib/topmodel/issues/24)

## 1.2.3

- [1c783b66d2bbd73d010](https://github.com/klee-contrib/topmodel/commit/1c783b66d2bbd73d010a11ababcd9844e7d25af4) JPA - Ajout du cascade type dans les oneToOne et généralisation de l'utilisation des Set.

## 1.2.2

- [4de0ae72a00b35](https://github.com/klee-contrib/topmodel/commit/4de0ae72a00b35729d90949c57f915db35e066c8) JPA - Fix referenced column dans les associations oneToOne qui ont un rôle

## 1.2.1

- [b5c7d2553c0be](https://github.com/klee-contrib/topmodel/commit/b5c7d2553c0bea9d0e65df43a360cab817305913) JS - Api client : Correction import des liste de ref si elles se trouvent dans un sous-module

## 1.2.0

- [192dbf67e68da](https://github.com/klee-contrib/topmodel/commit/192dbf67e68da23871956d3fc05e667103602d4f) JS - Ajout du mode `VALUES` pour la génération des listes de ref

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
