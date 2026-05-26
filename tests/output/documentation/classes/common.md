# Dictionnaire de données du module Common

| Schéma  | Table       | Désignation | Nom SQL          | Libellé | Type SQL | Longueur | Description                     | Contraintes  | Obligatoire | Valeurs possibles |
| ------- | ----------- | ----------- | ---------------- | ------- | -------- | -------- | ------------------------------- | ------------ | ----------- | ----------------- |
| gestion | TRANSLATION | ResourceKey | TRA_RESOURCE_KEY |         | varchar  | 100      | Clé de traduction.              | Clé primaire | Oui         |                   |
|         |             | Value       | TRA_VALUE        |         | varchar  | 100      | Valeur de la clé de traduction. |              | Oui         |                   |
|         |             | Lang        | TRA_LANG         |         | varchar  | 100      | Langue de traduction            | Clé primaire | Oui         |                   |
