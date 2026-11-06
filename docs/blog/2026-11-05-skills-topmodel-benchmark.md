---
title: "Des Skills IA pour TopModel"
description: "Un skill TopModel pour agents IA existe. Nous avons mesuré son effet sur 9 cas, avec 4 modèles (Haiku, Sonnet, Opus, Qwen), avec et sans skill : qualité, durée et coût."
slug: skills-ia-topmodel
authors:
  - name: Gildéric Deruette
    title: Contributeur
    url: https://github.com/gideruette
    image_url: https://avatars.githubusercontent.com/u/15626856
tags: [IA, benchmark]
date: 2026-11-05
---

Il nous a été posé maintes et maintes fois la question des Skills IA pour TopModel. Sans être IA sceptiques, notre intuition initiale était que les modèles de langages étaient suffisamment forts pour s'appuyer sur les outils que nous avions déjà mis en place : le language server et la documentation.

Néanmoins, nous avons eu l'occasion de réaliser des observations plus concrètes. Un utilisateur nous a proposé un skill relativement simple qu'il avait pu construire pas à pas dans son projet. Nous avons donc cherché à mesurer réellement sa performance.

## Le skill

Un `SKILL.md` (~3,8 k tokens) et cinq références (~26 k tokens : syntaxe, règles, exemples, outillage, générateurs). Il impose une boucle de travail : éditer le `.tmd`, lancer les diagnostics, valider sur copie temporaire (`modgen --check` régénère le code, il n'est donc pas en lecture seule), puis générer.

## Protocole

- **Base** : le modèle de l'`asta-starter-kit`, avec les générateurs JPA, Angular et SQL.
- **Un agent par cas et par configuration**, dans une copie isolée du modèle et de son code généré. Il reçoit en plus le dossier du skill pour les cas avec Skill. La consigne est identique.
- **7 cas** : catalogue de produits, évolution de l'utilisateur, commandes (deux adresses), historique de connexions, groupes (N-N), renommage d'une propriété, listes de référence existantes.
- **Modèles** : Haiku 5.5, Sonnet 5.5, Opus 5.5 et Qwen 27B, chacun avec et sans skill : 56 exécutions.
- **Validité** : `modgen --check` sort en 0 pour les 56. Aucun modèle invalide, aucun fichier généré modifié à la main.
- **Qualité** : note sur 10 par grille (4 points de conventions et de rapport, 6 points propres au cas), attribuée par un modèle correcteur.

## Premiers bancs de test

| Configuration     | Note /10 | Durée    | Coût par cas (API) |
| ----------------- | -------- | -------- | ------------------ |
| Haiku sans skill  | 9,28     | 5,1 min  | 0,042 $            |
| Haiku avec skill  | 9,66     | 4,8 min  | 0,116 $            |
| Sonnet sans skill | 9,34     | 1,7 min  | 0,168 $            |
| Sonnet avec skill | 9,59     | 2,4 min  | 0,264 $            |
| Opus sans skill   | 9,77     | 2,7 min  | 0,478 $            |
| Opus avec skill   | 9,79     | 4,3 min  | 0,839 $            |
| Qwen sans skill   | 8,58     | 30,7 min | 0,5–2,5 $          |
| Qwen avec skill   | 9,24     | 34,7 min | 0,5–2,5 $          |

### À retenir

À part pour Qwen qui a un écart significatif sur la qualité avec et sans skill, on retient essentiellement que les résultats sont équivalents pour des coûts et un temps d'exécution qui doublent.
On retiendra aussi que Opus est un modèle beaucoup trop cher pour effectuer des tâches TopModel, puisque Sonnet obtient des résultats équivalents.

Il nous faut donc un cas de test plus compliqué pour vraiment mettre à l'épreuve nos skills.

Reprenons donc quelques cas tordus de notre projet de test.
Nous allons maintenant tester en parallèle un skill beaucoup plus simple, qui donne des informations générales sur TopModel, le lien vers la documentation, le repo github etc.

## Un cas d'usage plus compliqué

| Configuration             | Critères     | Durée    | Coût API |
| ------------------------- | ------------ | -------- | -------- |
| Haiku sans skill          | 59/67 (88 %) | 17,4 min | 0,74 $   |
| Haiku avec tiny-skill     | 66/67 (99 %) | 14,8 min | 0,58 $   |
| Haiku avec skill complet  | 67/67        | 10,1 min | 0,50 $   |
| Sonnet sans skill         | 65/67 (97 %) | 6,0 min  | 0,76 $   |
| Sonnet avec tiny-skill    | 67/67        | 6,5 min  | 0,89 $   |
| Sonnet avec skill complet | 67/67        | 8,0 min  | 1,13 $   |

Pourquoi Qwen est-il absent ?

Sur ce cas il est parti, mais n'est jamais revenu...

### À retenir

Ici on constate tout de même un écart plus significatif, notamment pour Haiku. Sans skill, Haiku est presque 2 fois plus lent, et 1,5 fois plus cher.

En revanche, Sonnet avec ou sans skill est encore une fois presque aussi performant. Mais la version avec le skill complet est encore une fois significativement plus onéreuse (26 %), et plus lente (33%).

Avec le skill minimaliste, la différence de qualité avec le skill complet est marginale, pour Haiku comme pour Sonnet. En revanche les coûts sont plus avantageux.

On peut déjà tirer une première conclusion : sur des modèles bon marché, l'apport d'un skill est intéressant, mais uniquement pour les tâches complexes, ou les modèles bas de gamme.

## Dernier cas de test

| Configuration             | Critères           | Durée    | Coût API |
| ------------------------- | ------------------ | -------- | -------- |
| Haiku sans skill          | 118,5/129,5 (92 %) | 12,9 min | 0,38 $   |
| Haiku avec tiny-skill     | 126,5/129,5 (98 %) | 20,3 min | 0,44 $   |
| Haiku avec skill complet  | 129,5/129,5        | 13,9 min | 0,67 $   |
| Sonnet sans skill         | 118,5/129,5 (92 %) | 22,3 min | 1,04 $   |
| Sonnet avec tiny-skill    | 127,5/129,5 (98 %) | 11,8 min | 1,31 $   |
| Sonnet avec skill complet | 129,5/129,5        | 11,0 min | 1,50 $   |

### À retenir

En termes de qualité, les modèles sont donc équivalents quelle que soit la configuration. Au niveau du temps passé, n'avoir aucun skill, ou bien la nécessité d'aller chercher des informations, font perdre trop de temps sur ce cas.

## Conclusion

Le skill permet de compenser le manque de connaissances d'un modèle, et l'écart semble se creuser en fonction de la complexité de la tâche. Pour les cas simples il semble complexifier la tâche sans réel gain.

Nous mettons donc à disposition le skill complet, à utiliser de préférence avec un modèle économique 😉, et plutôt pour les cas avancés.
