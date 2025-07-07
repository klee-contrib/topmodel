# TopModel.ModelGenerator (`tmdgen`)

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
