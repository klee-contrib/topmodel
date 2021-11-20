# Visualisation

**/!\ Expérimental /!\\**

**TopModel.UI** (TMUI) est un outil de visualisation d'un modèle TopModel. Il s'agit également d'une application .NET 6, mais qui n'est à ce jour pas publiée. Vous pouvez néanmoins l'utiliser en téléchargeant les sources et en utilisant la commande `dotnet run` sur le projet C#, en lui passant le fichier de config du générateur.

Une fois l'application lancée, elle est accessible par défaut sous [https://localhost:5001](https://localhost:5001) (le lien apparaît dans la console et sera cliquable depuis VSCode, si jamais).

Elle affichera dans le menu de gauche la liste des fichiers chargés, et il sera possible de cliquer dessus pour afficher un diagramme type UML généré automatiquement correspondant aux classes définies dans le fichier. Le diagramme sera mis à jour à la volée en cas de modification du fichier ouvert.

TMUI utilise [Graphviz](http://graphviz.org/) pour dessiner les diagrammes. **Il suppose que Graphviz est installé sur la machine qui l'exécute et accessible dans le PATH**. Comme l'indique la [page d'installation](https://graphviz.gitlab.io/_pages/Download/Download_windows.html), l'installer **ne renseigne pas le PATH par défaut**. Par défaut, le chemin a mettre dans le path est `C:\Program Files (x86)\Graphviz2.38\bin`.
