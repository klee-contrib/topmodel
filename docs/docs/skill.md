# Skill pour agents IA

TopModel dispose d'un **skill** pour agents IA (Claude Code, GitHub Copilot, Cursor…). Il donne à l'agent les connaissances nécessaires pour modéliser avec TopModel : il écrit et corrige des fichiers `.tmd` sans deviner la syntaxe, valide le modèle, puis lance la génération.

Le skill est distribué sous forme de package [APM](https://github.com/microsoft/apm) (_Agent Package Manager_), dans le dépôt [klee-contrib/topmodel-skills](https://github.com/klee-contrib/topmodel-skills).

## Ce que contient le skill

- **La syntaxe** des fichiers `.tmd` : classes, propriétés (`association`, `alias`, `composition`), domaines, endpoints, annotations, avec les clés autorisées pour chaque variante.
- **Les règles de modélisation** : classes persistées ou DTO, listes de référence, mappers, `uses:` circulaires, visibilité entre fichiers grâce aux tags.
- **Les générateurs** : ce que produisent concrètement les générateurs C#, Java et front (noms, emplacements, types).
- **L'outillage** : `modgen`, `tmdgen` et `modls`, avec le décodage des erreurs `TMDxxxx`.
- **Des exemples** pour chaque type d'objet du modèle.

### Boucle de travail

Le skill impose à l'agent le même cycle à chaque modification :

1. Il édite le `.tmd`, jamais le code généré.
2. Il valide le modèle sur une **copie temporaire**
3. Il lance la génération réelle avec `modgen`.
4. Il traite ce qui se trouve autour du modèle (migrations, enums, contrats, store de référence).

Si le code généré semble faux, l'agent suit un protocole « bug du générateur » : il le signale au lieu de retoucher silencieusement le fichier généré.

### Language server

Le package déclare aussi le language server [`modls`](./cli.md) pour les fichiers `.tmd`. Quand l'agent le supporte, il bénéficie des diagnostics, de la navigation et de la recherche de références sur le modèle, par exemple pour savoir ce qui utilise une classe avant de la renommer.

## Installation avec APM

### Prérequis

- [APM](https://github.com/microsoft/apm) installé.
- `modgen` et `modls` disponibles dans le `PATH` :

```bash
dotnet tool install --global TopModel.Generator
dotnet tool install --global TopModel.LanguageServer
```

### Installer le skill

Depuis la racine du projet qui contient le modèle :

```bash
apm install klee-contrib/topmodel-skills
```

APM ajoute le package aux dépendances du projet (`apm.yml`) et déploie le skill dans les dossiers lus par les agents détectés (par exemple `.claude/skills/` pour Claude Code).

Pour que les autres développeurs récupèrent le skill, versionnez `apm.yml` et son fichier de verrouillage, puis lancez :

```bash
apm install
```

### Déclarer la dépendance à la main

Il est aussi possible de déclarer le package dans l'`apm.yml` du projet :

```yaml title="apm.yml"
dependencies:
  apm:
    - klee-contrib/topmodel-skills
```

Puis de lancer `apm install`.

### Mettre à jour

```bash
apm install --update
```

## Utilisation

Une fois installé, il n'y a rien à activer : l'agent charge le skill dès que la demande concerne TopModel (écrire ou modifier un `.tmd`, ajouter une entité, une liste de référence, un endpoint, lancer `modgen`, comprendre un fichier généré obsolète…).

Par exemple :

> Ajoute une classe `Projet` avec un nom, une date de début et une association vers `Utilisateur`, puis régénère le code.

:::tip
Le skill donne la priorité aux `.tmd` existants du projet, qui portent vos conventions locales. Plus votre modèle est cohérent, meilleurs seront les modèles produits par l'agent.
:::

:::note
Les commandes `apm` peuvent évoluer selon la version installée. En cas de doute, consultez `apm --help` ou la documentation d'APM.
:::
