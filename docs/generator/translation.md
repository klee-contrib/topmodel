# Générateur de traductions manquantes

Le générateur de tranductions manquantes peut se configurer ainsi :

```yaml
translation:
  - tags: # Liste des tags pour lesquels on souhaite générer les traductions manquantes
      - dto
    outputDirectory: ./ # Dossier racine dans lesquels les fichiers de traductions manquantes seront générés
    rootPath: i18n/{lang}/out  # Template des dossiers dans lesquels les fichiers de traductions manquantes seront générés
    langs: # Liste des langues pour lesquelles on souhaite obtenir une traduction
      - en_EN
```

Avec cette configuration, tous les `Label` de toutes les propriétés de toutes les classes de tous les fichiers `*.tmd` avec le tag `dto` sont considérés comme *à traduire*. Il en va de même pour les valeurs des listes de références pour lesquelles TopModel a détecté une `defaultProperty`. Ceux-ci se verront attribués les clés `{module}.{classe}.values.{clé d'unicité de la valeur}`.

Le générateur créera alors des fichiers `{module}_{lang}.properties` contenant l'ensemble des traductions à effectuer.
