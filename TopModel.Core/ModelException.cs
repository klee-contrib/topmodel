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
}