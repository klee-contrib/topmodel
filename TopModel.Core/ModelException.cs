using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

/// <summary>
/// Exception dans la lecture/parsing du modèle.
/// </summary>
public class ModelException : LegitException
{
    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="message">Message.</param>
    public ModelException(string message)
        : base(message) { }

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="objet">Objet de modèle concerné.</param>
    /// <param name="message">Message.</param>
    /// <param name="location">Localisation éventuelle vers l'erreur (si pas liée à l'objet)</param>
    public ModelException(object objet, string message, Location? location = null)
    {
        ModelError = new ModelError(ErrorType.TMD0000, objet, message, location);
    }

    public override string Message => ModelError != null ? ModelError.ToString() : base.Message;

    public ModelError? ModelError { get; }
}
