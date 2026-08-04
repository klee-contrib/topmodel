# TopModel.ModelGenerator (`tmdgen`)

## 4.2.0

**breaking changes**

- Les paramètres de route sont désormais générés en premier (devant le paramètre de body)
- Les paramètres de body sont générés avec `required: true` (ce qui n'a pas d'impact sur le résultat final car ils sont toujours obligatoires de fait)

De plus, puisque l'extension VSCode n'embarque plus les schémas JSON, elle ne contient désormais plus celui du fichier `tmdgen.config`. Par conséquent, vous pouvez désormais lancer la commande `tmdgen -s` ou `tmdgen --schema` pour gérer le schéma, de la même façon que pour `modgen`.

## 4.1.0

Intégration des évolutions sur la gestion des configs de `modgen`, mais `tmdgen` pouvant déjà relancer la génération en mode `--watch`, la seule nouveauté est le mode `--parallel`.

## 4.0.0

- [`4a2d5a6`](https://github.com/klee-contrib/topmodel/commit/4a2d5a6fc0b8878075648c224de3c8d352c8a5a7) - [tmdgen - openapi] Enums générées par défaut en enum: true, + config useEnumClasses pour readonly: true à la place

  Cette évolution est un **breaking change** dans le sens où le code généré n'est compatible avec TopModel 4 (et versions ultérieures), car les enums sont par défaut générées avec `enum: true`, et l'option de config ajoutée qui permet de retrouver le comportement précédent ajoute `readonly: true` sur la classe, qui sont tous les deux des évolutions apportées par TopModel 4.0

  La version de tmdgen est donc montée pour être à la même majeure que TopModel lui-même pour cette raison-là. Il ne s'agit pas d'une "réelle" version majeure de l'outil 😅

- [`cd57bb9`](https://github.com/klee-contrib/topmodel/commit/cd57bb9ecf446ef343ff9c2055f3de50abe31f5d) - [tmdgen] Reprendre les commentaires de la base de données s'ils existents

## 1.13.1

- [`45fb64`](https://github.com/klee-contrib/topmodel/commit/45fb64d95ba24b530d91fafb95146fa318509be0) - [Tmdgen] Ajout de la contrainte d'unicité sur `value` pour la propriété d'enum (1 cas manquant)

## 1.12.9

- [`0bcebea`](https://github.com/klee-contrib/topmodel/commit/0bcebeaae8f1aa50fc9f1af2ea96ebfa4984bc75) - [tmdgen] Gestion récursion dans les références de schéma

## 1.12.8

- Revert du commit mentionné en dessous (😁)

## 1.12.6

- [`c4c14b4`](https://github.com/klee-contrib/topmodel/commit/c4c14b412019f19e81ad75e507ca4ab61f9f9289) - [tmdgen - SQL] Fix association PK pour qu'elle ait "type: oneToOne" aussi.

## 1.12.5

- [`445175b`](https://github.com/klee-contrib/topmodel/commit/445175b02263cf4bf3b893f3a5a83f729593b4e9) - [tmdgen] amélioration algorithme de regroupement

## 1.12.4

- [`2f054e6`](https://github.com/klee-contrib/topmodel/commit/2f054e6520a0d137b692cde8ab8d0412fbbc51d4) - [tmdgen] amélioration algorithme de regroupement

## 1.12.3

- [`85adbf3`](https://github.com/klee-contrib/topmodel/commit/107c04133c7fced949658f791b779141721a328d) - [tmdgen] fix détection du rôle pour une association vers la même classe
- [`107c041`](https://github.com/klee-contrib/topmodel/commit/107c04133c7fced949658f791b779141721a328d) - [tmdgen] fix uses commençant par "./"

## 1.12.1

- [`24a55fc`](https://github.com/klee-contrib/topmodel/commit/24a55fcaa2f7fc66c7220fd53594eeaf266a0716) - [tmdgen] Descriptions manquantes dans les request et responses bodies

## 1.12.0

[tmdgen - OpenAPI] Fixes

- Performance
- Plantage sur certains alias
- Schémas de réponse inlines non pris en compte (comme ceux du Body)
- endpoint.Description si pas de Summary
- Propriétés d'enum dans les endpoints avec la bonne casse
- endpoint.Name généré en PascalCase

## 1.11.0

Montée de version du parser open-api.

- Les schémas ne servant que de passe plat vers un autre schémas sont maintenant ignorés, et seul la classe finale est générée.
- Les endpoint faisant références à ces derniers sont mis à jour pour pointer vers la classe finale.
- Les propriétés sont maintenant facultatives par défaut
