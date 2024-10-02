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
