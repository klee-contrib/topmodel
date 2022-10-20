# Personnalisation fine

## Pas de templating global

Dans `TopModel`, les fichiers ne sont pas générés à l'aide de templates. Bien qu'il s'agisse d'une pratique courante dans les différents générateurs de code existant, dans le cas de TopModel il n'est pas possible de créer de tels template.

- L'API du `ModelStore` est très complète, et complexe
- `TopModel` gère beaucoup de cas différents selon la configuration

Ces deux raisons ont pour conséquences que les templates créés seraient extrêmement difficiles à maintenir. Leur validation serait très compliquée, et le risque de régression beaucoup plus important qu'actuellement.

Ainsi, il n'est pas possible de personnaliser la génération en surchargeant ces templates. Même si nous arrivions à les créer, toute personnalisation serait très laborieuse, et réservée à une poignée d'utilisateurs. Il serait extrêmement difficile de synchroniser les évolutions de l'API du ModelStore avec les templates personnalisés. Les développeurs resteraient bloqués dans la version d'origine des templates, et c'est dommage ;)

## Mais des templates locaux

Nous souhaitons développer dans `TopModel` des zones très localisées où il sera possible aux développeurs de personnaliser, à l'aide de micro-templates, le code généré. Il déjà possible à certains endroit de renseigner `{module}`, qui sera remplacé par le nom du module dans la génération correspondante. Ce type de comportement ouvrira beaucoup de portes pour des personnalisations très pointues, tout en gardant un comportement standard simple et uniforme.

## Ou des réponses génériques

L'idée est de faire des réponses **génériques** aux différents besoins **spécifiques** qui pourraient être gérés par des templates. C'est l'idée derrière la fonctionnalité des `decorator`, qui permettent d'ajouter des annotations, des propriétés, des classes héritées hors `TopModel` etc. Les `decorator` sont pour le moment réservés aux classes, mais ils pourraient être étendus à d'autres types d'objets dans TopModel.  Les domaines servent également cet objectif. Les évolutions futures pour répondre aux problématiques de personnalisation de plus en plus fines passeront par ces différents canaux. Et cela se fera avec l'aide de la communauté bien entendu.
