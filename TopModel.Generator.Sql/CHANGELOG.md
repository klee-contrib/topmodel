## 3.4.0

Compatibilité avec `preservePrimaryKey: true` de TopModel 3.6

## 3.3.1

- [`1c9ef05`](https://github.com/klee-contrib/topmodel/commit/1c9ef050c3520a42a6d0912016ad0e0a9f52ecee) - [SQL] Fix alias d'association xxxToMany en trop dans les tables générées

## 3.3.0

- [`815054e`](https://github.com/klee-contrib/topmodel/commit/815054eb031b1e3943e9a9d5362b21a63abee4f6) - Classe de traductions explicite dans le modèle

  Le générateur SQL utilise maintenant les classes de traductions explicites dans le modèle pour générer les traductions en SQL, au lieu d'une table en dur.

## 3.2.1

- [`621c637`](https://github.com/klee-contrib/topmodel/commit/621c637d89f37cea2473219dd3d6c122a2694729) - [All] Fix gestion langage par défaut

## 3.2.0

Compatibilité avec TopModel 3.3

## 3.1.0

Compatibilité avec Topmodel 3.2

## 3.0.0

Compatibilité avec TopModel 3

## 1.2.1

- [`4048f2b`](https://github.com/klee-contrib/topmodel/commit/4048f2b4f3a577b582fd579b451b133bf4288b66) - [sqlgen] Pas de clé d'unicité sur les oneToOne primary key

## 1.2.0

Compatibilité avec les fallbacks de langage d'implémentation de TopModel 2.7

## 1.1.1

- [`708248f`](https://github.com/klee-contrib/topmodel/commit/708248f3073167c4ef629824e0c707206bd2f00f) - [SSDT] Fix commentaire de génération avec les mauvais tokens

## 1.1.0

Compatibilité avec `ignoredFiles` de TopModel 2.4

Les deux générateurs SQL ont été entièrement refactorisés pour s'aligner sur la structure des autres générateurs. Aucune différence de comportement n'est à attendre, à l'exception des noms de générateurs affichés dans les logs de génération de fichiers.

## 1.0.4

- [#396](https://github.com/klee-contrib/topmodel/pull/396) - Resource générée en double lorsque l'on utilise des alias de champs dans les entity

## 1.0.3

Version initiale.
