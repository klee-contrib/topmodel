---
title: "Un nouveau générateur de documentation intégré"
description: "Le module de génération documentation produit du Markdown vivant (dictionnaire de données, endpoints, Mermaid) à partir du modèle. Il s'appuie sur une capitalisation d'un module custom conçu pour un projet concret."
slug: nouveau-generateur-documentation
authors:
  - name: Gildéric Deruette
    title: Contributeur
    url: https://github.com/klee-contrib/topmodel
    image_url: https://avatars.githubusercontent.com/u/15626856
tags: [génération, documentation]
date: 2026-04-24
---

TopModel compte désormais un **générateur de documentation** dédié, pensé pour produire de la **documentation structurée et maintenue automatiquement** à partir du modèle.

## Origine

Ce module est une **capitalisation** d'un **générateur custom** qui avait été mis en place au sein d'un projet, ainsi que de la prévisualisation UML initialement disponible uniquement dans l'extension VSCode.

## Contenu généré

Trois briques, activables indépendamment, couvrent des usages basiques :

- **Dictionnaire de données** (tables et propriétés) ;
- **Liste des endpoints** (métadonnées HTTP, description, rôles éventuels) ;
- **Diagrammes Mermaid** (modèle de classes) au format prêt à l'emploi (Docusaurus, GitHub, etc.).

Sortie : fichiers **Markdown** vers un site statique ou tout autre canal qui consomme du MD.

## Pour aller plus loin

Toute la **configuration** (chemins, découpage par module ou par fichier, désactivation ciblée des sous-générateurs) est détaillée sur la page [Génération > Documentation](/generator/documentation).
