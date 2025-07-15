# TopModel.ModelGenerator (`tmdgen`)

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
