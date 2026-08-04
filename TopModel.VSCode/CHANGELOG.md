# VSCode

## 4.7.0

- Le Language Server est désormais distribué comme un tool .NET global (`modls`, package NuGet `TopModel.LanguageServer`) publié séparément de l'extension, au lieu d'être embarqué. Il est installé/mis à jour automatiquement comme `modgen` et `tmdgen`.
- Ajout d'une vérification de l'alignement des versions entre `modls`, `modgen` et `tmdgen`, avec proposition de mise à jour en cas de désalignement.
- Affichage d'une erreur au démarrage de l'extension si le language server `modls` n'est pas installé.

Ainsi, les prochaines, et à priori rares, versions de l'extensions VSCode ne concerneront que le client d'extension, spécifique VSCode. Il n'y aura plus de release de l'extension pour un update du Core ou du LanguageServer.

## 3.6.7

- [`311757`](https://github.com/klee-contrib/topmodel/commit/311757821e8b7c11ff0a6cd5b1779e210ff2ae76) - VSCode fix texte status bar en cas d'erreur

## 3.6.6

- MAJ Language Server avec la version 3.6.4 de `TopModel.Core`

## 3.6.5

- [`cc46f55`](https://github.com/klee-contrib/topmodel/commit/cc46f5537f9655bd3c09fbf3fb766d421d7a58d0) - [VSCode] Correction gestion buffer récupération de l'historique des versions

## 2.6.5

- [#465](https://github.com/klee-contrib/topmodel/issues/465)🐛 [VSCode] Add Unix compatibility for tool installation check thanks to @oscarcamilopulidop
