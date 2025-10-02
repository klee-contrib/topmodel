## 3.1.3

- [`f7b3475`](https://github.com/klee-contrib/topmodel/commit/f7b347537de55e0359c27053c9d461458be11f46) - [C#] Fix annotation [Column] en trop sur xxToMany

## 3.1.2

- [`08dfc43`](https://github.com/klee-contrib/topmodel/commit/08dfc43d97603ac527078e8f466fa6d209c3f9c2) - [Core/C#] Fix required en trop sur associations ToMany

## 3.1.1

- [`c7707e6`](https://github.com/klee-contrib/topmodel/commit/c7707e66eaefc0c016c403beab6030df8b1e4d1c) - [Core] Fix imports d'annotations ajoutés à tort avec les annotations de domaines/décorators

## 3.1.0

- [`5c8bb57`](https://github.com/klee-contrib/topmodel/commit/5c8bb577a7a623af452837717c1974e53c597cc0) - [C#] useCancellationTokens

  Cette nouvelle option permet de générer les clients et les contrôleurs pour les endpoints avec des CancellationTokens.

## 3.0.0

Compatibilité avec TopModel 3

La propriété `mapperLocationPriority` vaut désormais `non-persisted` ou `persisted`.
La propriété `useRecords` ne peut plus valoir que `true` ou `false`.

## 1.6.2

Fix bug de génération du DbContext pour un alias de décorateur (modgen 2.8).

## 1.6.1

- [`2fb8b73`](https://github.com/klee-contrib/topmodel/commit/2fb8b73ba4487b94628116d001ef2478406584da) - Fix breaking change involontaire sur les générateurs suite à modgen 2.7

## 1.6.0

Compatibilité avec les fallbacks de langage d'implémentation de TopModel 2.7

## 1.5.2

- [`6b2182b`](https://github.com/klee-contrib/topmodel/commit/6b2182bdb7aaa6574b684e9f5d4af149f2d382ed) - [C#] Fix usings manquants dans les mappers quand on parse une enum.

## 1.5.1

- [`8ea3a07`](https://github.com/klee-contrib/topmodel/commit/8ea3a071b1944b4f1cde1a33240c54f861ebd7a4) - [C#] Ajout `mappersName` + gestion des transforms dans `referenceAccessorsName`

  **breaking changes** : Les mappers de classes non persistées n'ont plus "DTO" dans le nom, et respecteront le nom donné dans le nouveau paramètre (qui vaut `{module}Mappers` par défaut, comme avant).

## 1.5.0

Suite à la gestion de la transformation `:path` dans TopModel 2.5, le générateur C# ne l'applique plus automatiquement sur la variable `{module}` dans les chemins de fichiers. Cela ne devrait avoir d'impact que si vous utilisiez des sous-modules, et peut se résoudre en appliquant le `:path` explicitement.

## 1.4.0

- [`50c3f6b`](https://github.com/klee-contrib/topmodel/commit/50c3f6b86a71da1e76ca1a63d4372120c788732e) - [C#] Mise au propre de la génération des accesseurs de liste de références

  **breaking changes** :

  - Les interfaces d'accesseurs de liste de références sont désormais séparés en 2, entre ceux qui sont sur des classes persistés et ceux sur des classes non-persistés. Cela permet d'arrêter d'avoir à implémenter un `partial` sur l'implémentation générée (qui n'a elle pas changé) avec les autres listes de références, vous pouvez donc simplement implémenter la deuxième interface directement, ou vous voulez.
  - Les interfaces et implémentations persistées sont générées avec un nom préfixé par `Db` (en plus du `referenceAccessorName`).
  - `referenceAccessorsInterfacePath` n'a plus de valeur par défaut et doit être renseigné pour que les accesseurs soit générés (puisqu'on ne veut plus du tout la générer à côté de l'implémentation 😉)
  - (Les commentaires générés dans les accesseurs sont désormais en français, comme le reste).

- [`b8595e4`](https://github.com/klee-contrib/topmodel/commit/b8595e4bc945c6668374985cdb4f4ace16a16623) - [C#] Plus d'options à "true" par défaut

  **breaking changes** : `enumForStaticReferences`, `usePrimaryConstructors` et `useRecords` sont désormais à `true` par défaut.

## 1.3.0

- [`cf05c8c`](https://github.com/klee-contrib/topmodel/commit/cf05c8c31d8f80179741b2c5d6a07888528207f7) - [C# ApiServer] Annotations [Required] sur les paramètres required
- [`865020e`](https://github.com/klee-contrib/topmodel/commit/865020e969ec65535f0aeaca9c7da09b61321710) - [C#] Fix détermination valueType pour enums avec genericType

**breaking change** : Le `required` est désormais correctement pris en compte sur les paramètres d'endpoint, ce qui va en particulier ajouter des annotations `[Required]` sur vos query params obligatoires. Vous devriez donc vérifier que la valeur de `required` dans le modèle correspond bien à la réalité de votre endpoint, ou alors vous pouvez simplement ajouter des `required: false` jusqu'à ce que le code généré ne change pas (mais ce n'est évidemment pas la meilleure solution 😉)

## 1.2.0

Compatibilité avec `ignoredFiles` de TopModel 2.4

## 1.1.2

- [`75ba587`](https://github.com/klee-contrib/topmodel/commit/75ba58725fcf8c3e0abb495bf60cc0d2c68ca3fa) - [C#] Ajout génération usings de converters dans les mappers

## 1.1.1

- [`c1ec016`](https://github.com/klee-contrib/topmodel/commit/c1ec01639dccc17ece05136ffe85ce1618d925fb) - [C# Server API] Fix ? et = null en trop pour bodyparam: true

## 1.1.0

- [`6aeba30`](https://github.com/klee-contrib/topmodel/commit/6aeba30068b86500e9d73b5d474f354e1e384979) - [C# Server API] Paramètres multipart toujours nullables (comme query)

  C'est un **petit breaking change** parce que tous les paramètres multipart (à priori les fichiers à upload, typés `IFormFile`) sont désormais générés nullables avec un `= null` derrière, commes les query params, ce qui nécessite de les mettre en dernier dans la liste des paramètres. La prochaine version de TopModel incluera une mise à jour du warning existant pour prendre en compte ce cas.

## 1.0.4

- [`2f1fe4a`](https://github.com/klee-contrib/topmodel/commit/2f1fe4a6b7d369b45c2b159c9e9f6b323eb225ff) - [C#] Fix using en trop si `requiredNonNullable`

## 1.0.3

Version initiale.
