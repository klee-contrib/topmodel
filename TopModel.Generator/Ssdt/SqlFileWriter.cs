using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Ssdt
{
    /// <summary>
    /// Writer pour l'écriture de fichier.
    /// Spécifique pour les fichiers SQL (usage du token commentaire SQL).
    /// </summary>
    public class SqlFileWriter : FileWriter
    {
        public SqlFileWriter(string fileName, ILogger logger)
            : base(fileName, logger)
        {
        }

        /// <summary>
        /// Renvoie le token de début de ligne de commentaire dans le langage du fichier.
        /// </summary>
        /// <returns>Toket de début de ligne de commentaire.</returns>
        protected override string StartCommentToken => "----";
    }
}
