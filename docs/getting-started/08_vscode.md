# Utiliser l'extension VSCode

Nous l'avons dit et répétés, l'expérience de développement sera extrêmement plus confortable grace à l'utilisation de l'[extension TopModel pour VSCode](https://marketplace.visualstudio.com/items?itemName=JabX.topmodel)

Vous remarquerez la coloration syntaxique, la navigation et l'auto-complétion.
Voici quelques fonctionnalités et raccourcis pour les utilisateurs moins familiers de VSCode.

## Recherche de références (`Ctrl + t`)

Avec le raccoucis `Ctrl + t`, vous pouvez effectuer une recherche sur :

- Les classes
- Les domaines
- Les endpoints
- Les décorateurs

Et ainsi retrouver rapidement les objets de votre modèle.

## Renommage (`F2`)

Avec le raccourcis `F2`, il vous est possible de modifier le nom de n'importe quelle classe, n'importe quel domaine ou décorateur.

## Prévisualisation du schéma UML (`F1` : `Ouvre la prévisualisation UML du modèle`)

Lorsque vous travaillez sur un fichier contenant des classes persistées, vous pouvez avoir besoin de prévisualiser le schéma UML de votre modèle persisté.

Cela est rendu possible en utilisant le bouton de prévisualisation situé en haut à droite de votre IDE.

Vous pouvez également utiliser le raccourcis `F1`, puis rechercher la commande `Ouvrir la prévisualisation UML du modèle`.

[preview](./_media/preview.gif)

Le schéma affiché présente l'ensemble des classes persistées définies dans le fichier courant, avec leurs propriétés et leurs associations. Au clic sur une classe, le curseur de l'éditeur courant se déplace vers la définition de la classe en question. S'il s'agit d'une classe définie dans un autre fichier, l'éditeur change de fichier courant. Le schéma se met à jour en conséquence.

Il est possible de zoomer/dézoomer avec la molette de la souris, ou bien avec les boutons `+` et `-`. Il est également possible de se déplacer dans le schéma en maintenant le bouton gauche de la souris enfoncé et en la déplaçant horizontalement ou verticalement.

## Lancer la génération

### Commande VSCode (`F1` : `Lance la commande de génération du modèle`)

L'extension détecte automatiquement les différents fichiers de configuration ouvert dans votre espace de travail. Vous pouvez lancer la génération de tous les projets avec la commande `Lance la commande de génération du modèle`, après avoir ouvert la recherche de commande avec le raccourcis `F1`.

### Barre d'état

En cliquant sur la barre d'état de l'extension, il vous sera proposé les différentes commandes que vous pouvez lancer

- Génération du modèle sur chacun des projets de l'espace de travail
- Génération du modèle en continu sur chacun des projets de l'espace de travail

## Mettre à jour TopModel (`F1` : `Mettre à jour TopModel.Generator`)

Vous pouvez mettre à jour TopModel en utilisant la commande `Mettre à jour TopModel.Generator`.
