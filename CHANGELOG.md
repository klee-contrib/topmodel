# TopModel.Generator (`modgen`)

## 1.2.0

- [`2a5bff1`](https://github.com/klee-contrib/topmodel/commit/2a5bff154869787f2e637bcdc62b9af0af70984c) #17 [C#Gen] Utilisation du module pour déterminer le répertoire de génération des APIs

  `apiPath` a été divisé en `apiRootPath` et `apiFilePath`, ce qui permet de gérer proprement les modules dans les répertoires côté serveur (parce qu'il faut insérer un "/Controllers/" entre les deux paramètres). On n'utilise du coup plus le chemin des fichiers de modèles du tout.
- [`9d59eb0`](https://github.com/klee-contrib/topmodel/commit/9d59eb0b6c70f87167f22154f2c04f582ee4658c) #17 JS - API dans le répertoire du sous-module

  N'utilise non plus plus le chemin des fichiers de modèle, et un paramètre `apiClientFilePath` a été ajouté gérer les sous-répertoires en fonction du module.

- [`c83b32c`](https://github.com/klee-contrib/topmodel/commit/c83b32c73247b39324194949d9a47317908076fc) #13 JS : Mauvais import lorsque les fichiers d'API sont dans des sous répertoires avec une profondeur > 1



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
