# Dictionnaire de données du module Common

| Schéma  | Table       | Désignation | Nom SQL          | Libellé | Type SQL | Longueur | Description                     | Contraintes  | Obligatoire | Valeurs possibles |
| ------- | ----------- | ----------- | ---------------- | ------- | -------- | -------- | ------------------------------- | ------------ | ----------- | ----------------- |
| gestion | translation | ResourceKey | tra_resource_key |         | varchar  | 100      | Clé de traduction.              | Clé primaire | Oui         |                   |
|         |             | Value       | tra_value        |         | varchar  | 100      | Valeur de la clé de traduction. |              | Oui         |                   |
|         |             | Lang        | tra_lang         |         | varchar  | 100      | Langue de traduction            | Clé primaire | Oui         |                   |
