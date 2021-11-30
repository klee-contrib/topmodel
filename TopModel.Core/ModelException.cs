namespace TopModel.Core;

/// <summary>
/// Exception dans la lecture/parsing du modèle.
/// </summary>
public class ModelException : Exception
{
    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="message">Message.</param>
    public ModelException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Constructeur.
    /// </summary>
    /// <param name="objet">Objet de modèle concerné.</param>
    /// <param name="message">Message.</param>
    public ModelException(object objet, string message)
        : base(new ModelError(objet, message).ToString())
    {
    }
}