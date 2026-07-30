#pragma warning disable KTA1200

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace TopModel.LanguageServer;

public static class TextDocumentSelectorExtensions
{
    /// <summary>
    /// Racines de modèle (chemins absolus) gérées par ce process serveur, alimentées au démarrage des workers.
    /// </summary>
    private static readonly HashSet<string> _modelRoots = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Verrou sur <see cref="_modelRoots"/> : les workers sont démarrés en parallèle (l'option `-p` est
    /// forcée au lancement du serveur), donc <see cref="AddModelRoot"/> est appelé simultanément depuis
    /// plusieurs threads, ce qu'un HashSet ne supporte pas.
    /// </summary>
    private static readonly object _modelRootsLock = new();

    /// <summary>
    /// Enregistre une racine de modèle (chemin absolu) gérée par ce serveur, pour scoper le sélecteur des
    /// fichiers .tmd (cf. <see cref="TmdFiles"/>).
    /// </summary>
    /// <param name="modelRoot">Racine de modèle absolue (cf. <c>ModelConfig.ModelRoot</c>).</param>
    public static void AddModelRoot(string modelRoot)
    {
        lock (_modelRootsLock)
        {
            _modelRoots.Add(modelRoot);
        }
    }

    extension(TextDocumentSelector)
    {
        /// <summary>
        /// Sélecteur des fichiers .tmd gérés par ce serveur, scopé aux racines de modèle de ses configs.
        ///
        /// Dans un workspace multi-dossiers, chaque dossier lance son propre process modls. Sans ce scoping,
        /// tous les serveurs enregistreraient leurs handlers avec le même sélecteur global ("**/*.tmd") :
        /// VSCode verrait autant de providers que de dossiers pour un même fichier. Pour les features "fusion"
        /// (navigation, code lens, complétion...) c'est sans conséquence (les résultats sont agrégés), mais
        /// pour les features "provider unique" (coloration sémantique, rename, formatting) VSCode ne retient
        /// qu'un seul provider, qui peut être celui d'un autre dossier - lequel ne connaît pas le fichier et
        /// renvoie un résultat vide, sans repli. En scopant le sélecteur aux racines de modèle de ce serveur,
        /// chaque provider ne matche que les fichiers qu'il sait réellement traiter.
        ///
        /// Tant qu'aucune racine n'est connue (les workers sont démarrés pendant l'initialisation, qui peut
        /// n'être pas terminée quand les options d'enregistrement des handlers sont calculées), on se replie
        /// sur le répertoire courant du process, et non sur un glob global : l'extension lance chaque modls
        /// avec pour répertoire courant le workspace folder dont il a la charge. Le repli reste donc confiné
        /// à ce dossier, là où un glob global ferait revendiquer à ce serveur les fichiers des autres dossiers
        /// et lui ferait voler leur coloration.
        /// </summary>
        public static TextDocumentSelector TmdFiles
        {
            get
            {
                lock (_modelRootsLock)
                {
                    List<string> roots = _modelRoots.Count == 0 ? [Directory.GetCurrentDirectory()] : [.. _modelRoots];
                    return TextDocumentSelector.ForPattern(
                        [.. roots.Select(root => $"{root.Replace('\\', '/').TrimEnd('/')}/**/*.tmd")]
                    );
                }
            }
        }
    }
}

