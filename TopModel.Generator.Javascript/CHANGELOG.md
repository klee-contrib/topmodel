## 3.6.0

Compatibilité avec Topmodel 3.8

## 3.6.1

- [`4cf51`](https://github.com/klee-contrib/topmodel/commit/4cf5171f36027b3f9cac860799bf4591d4b9753f) - [Angular] Fix requête delete avec body

## 3.5.1

- [`963d963`](https://github.com/klee-contrib/topmodel/commit/963d96319f4727a67ad40b7dbdabda483837ef62) - [JS / Fetch] Fix type de retour pour arrays

## 3.5.0

- [`#518`](https://github.com/klee-contrib/topmodel/pull/518) - Mode `fetch`, par défaut, pour la génération des endpoints

  **breaking changes** : La valeur par défaut de `apiMode` est désormais `fetch` (le nouveau mode de génération introduit dans la PR), au lieu de `vanilla`, maintenant renommé `legacy`.

## 3.4.0

Compatibilité avec `preservePrimaryKey: true` de TopModel 3.6

## 3.3.1

- [`c6f28d9`](https://github.com/klee-contrib/topmodel/commit/c6f28d9ca2490e03eb1457baec14528872b5eeac) - [JS] Fix régression regroupement import domaine + entityTypesPath
- [`cdbc565`](https://github.com/klee-contrib/topmodel/commit/cdbc5654f785cb559e75cdef9cbb6f9c3af21fac) - [JS] Génération du nom de propriété à la place du label si non traduit

## 3.3.0

- [`be78f05`](https://github.com/klee-contrib/topmodel/commit/be78f056081d47e76b87b9cb6a1a7dd6f9b4c5d6) - [JS] `entityMode: focus`, pour cibler `@focus4/entities`
- [`c0605fb`](https://github.com/klee-contrib/topmodel/commit/c0605fbbd5315fc422991f9346ab5f89615ddf78) - [JS] Regroupe les imports des domaines et des types si nécessaire (fix #499)
- [`343e7d2`](https://github.com/klee-contrib/topmodel/commit/343e7d24ab02baab73a08a331e2812a41d5c826d) - [AngularApiClient] `observe: "response"` si `responseType: "blob"` (fix #508)

## 3.2.1

- [`621c637`](https://github.com/klee-contrib/topmodel/commit/621c637d89f37cea2473219dd3d6c122a2694729) - [All] Fix gestion langage par défaut

## 3.2.0

Compatibilité avec TopModel 3.3

## 3.1.0

Support pour `rootModule` dans la config.

## 3.0.0

Compatibilité avec TopModel 3

## 1.4.4

- [`82e531a`](https://github.com/klee-contrib/topmodel/commit/82e531a78dc114ed4112ce48c6b6a16fc1e26119) - [JS] Fix groupby submodule dans les traductions sans camelCase

## 1.4.3

- [`d4a16e9`](https://github.com/klee-contrib/topmodel/commit/d4a16e9a45cede8403cbaf9d71c1e9da2f2f1983) - [JS] Fix régression fillFormData client angular

## 1.4.2

- [`76cd1c8`](https://github.com/klee-contrib/topmodel/commit/76cd1c868e45828d729d39eeb0c859fb5b4dbb02) - [JS] Génération de fillFormData avec un for...of au lieu de data.forEach

## 1.4.1

- [`2fb8b73`](https://github.com/klee-contrib/topmodel/commit/2fb8b73ba4487b94628116d001ef2478406584da) - Fix breaking change involontaire sur les générateurs suite à modgen 2.7

## 1.4.0

- Compatibilité avec les fallbacks de langage d'implémentation de TopModel 2.7
- [#472](https://github.com/klee-contrib/topmodel/pull/472) - Fix résolution des variables dans FetchPath

## 1.3.2

- [`6c6f2`](https://github.com/klee-contrib/topmodel/commit/6c6f24213f125a88b8a683b5116093faef2825ad) - [JS] {lang} ajouté en trop sur index.ts : Fix [#451](https://github.com/klee-contrib/topmodel/issues/451)

## 1.3.1

- [`da591`](https://github.com/klee-contrib/topmodel/commit/8f9b2535d25ca7918176e7f0c3b62c441612a877) -[JS] Manque `reportProgress`

## 1.3.0

- [`9ad2e0`](https://github.com/klee-contrib/topmodel/commit/8f9b2535d25ca7918176e7f0c3b62c441612a877) - [Angular] : Permettre l'ajout d'un `HttpOptions` lors de la génération d'un service d'upload de fichier
  Fix [#448](https://github.com/klee-contrib/topmodel/issues/448)

- [`e4614`](https://github.com/klee-contrib/topmodel/commit/e4614c7206e701f937cdf17e7fcbfc0d2178280b) - [Angular] Utilisation des nouvelles api inject() dans la génération des services
  Fix [#413](https://github.com/klee-contrib/topmodel/issues/413)

## 1.2.0

Compatibilité avec `ignoredFiles` de TopModel 2.4

## 1.1.7

- [`8f9b253`](https://github.com/klee-contrib/topmodel/commit/8f9b2535d25ca7918176e7f0c3b62c441612a877) - [JS] Config javascript entityTypesPath ne créée pas des imports relatifs : Fix #426

## 1.1.6

- [`f3e4479`](https://github.com/klee-contrib/topmodel/commit/f3e447955bdb11c30deb6c3d55fa51bb0b3890f1) - [JSResourceGen] Fix génération libellés de classes avec le même nom qu'un sous module (...)

## 1.1.5

- [`4037be2`]([Core/JS] Fix "strictIfUppercase" manquant sur camel/pascal case des noms de propriétés) - [Core/JS] Fix "strictIfUppercase" manquant sur camel/pascal case des noms de propriétés

## 1.1.4

- [`437fc7b`](https://github.com/klee-contrib/topmodel/commit/437fc7b20114047d51f6b5100f3214f483920324) - [JS] Support de {module} (et {lang}) dans modelRootPath et resourceRootPath

## 1.1.3

- [ec2934d](https://github.com/klee-contrib/topmodel/commit/ec2934d07f8ddcc64992d0f436212e2a190d6a6f) [Angular] problème check nullité dans le cas d'un boolean en paramètre d'un endPoint Fix #418

## 1.1.1

- [7183e6d](https://github.com/klee-contrib/topmodel/commit/7183e6dfc6261e7e096ebc24f0ebb6b70b819442) Ne pas prendre le chemin relatif si le `fetchPath` ne commence pas par '.' (conformément à la documentation)

## 1.1.0

- [#400](https://github.com/klee-contrib/topmodel/pull/400) - Ajout entityMode "none"

## 1.0.7

- [#389](https://github.com/klee-contrib/topmodel/pull/389) - Prise en compte des returnType Blob et ArrayBuffer Hot fix x3

## 1.0.6

- [#389](https://github.com/klee-contrib/topmodel/pull/389) - Prise en compte des returnType Blob et ArrayBuffer Hot fix x2

## 1.0.5

- [#389](https://github.com/klee-contrib/topmodel/pull/389) - Prise en compte des returnType Blob et ArrayBuffer Hot fix

## 1.0.4

- [#389](https://github.com/klee-contrib/topmodel/pull/389) - Prise en compte des returnType Blob et ArrayBuffer

## 1.0.3

Version initiale.
