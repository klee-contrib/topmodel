namespace TopModel.Core;

using TopModel.Core.FileModel;
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
    /// <param name="message">Message.</param>
    public ModelException(ModelFile modelFile, string message)
        : base($"[{modelFile.Path}] {message}")
    {
    }
}