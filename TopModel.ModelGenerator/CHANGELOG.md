# TopModel.ModelGenerator (`tmdgen`)

## 1.11.0

Montée de version du parser open-api.
- Les schémas ne servant que de passe plat vers un autre schémas sont maintenant ignorés, et seul la classe finale est générée.
- Les endpoint faisant références à ces derniers sont mis à jour pour pointer vers la classe finale.
- Les propriétés sont maintenant facultatives par défaut