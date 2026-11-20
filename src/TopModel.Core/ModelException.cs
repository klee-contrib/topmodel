using TopModel.Core.FileModel;
using TopModel.Utils;

namespace TopModel.Core;

/// <summary>
/// Exception dans la lecture/parsing du modèle.
/// </summary>
public class ModelException : LegitException
{
    private ModelException(ModelError modelError)
        : base(modelError.ToString())
    {
        ModelError = modelError;
    }

    public override string Message => ModelError.ToString();

    public ModelError ModelError { get; }

    public static ModelException From<T>(T messageType, object objet, Location? location, params string[] args)
        where T : struct, Enum
    {
        var modelError = messageType is ErrorType errorType
            ? ModelError.Error(errorType, args, objet, location)
            : new ModelError(ErrorType.TMD0000, objet, messageType.GetMessage(args), location);
        return new ModelException(modelError);
    }
}
