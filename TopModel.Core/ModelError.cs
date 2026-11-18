using System.Text;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core;

public class ModelError
{
    private readonly Location? _location;
    private readonly string _message;
    private readonly object _objet;

    internal ModelError(
        ErrorType errorType,
        object objet,
        string message,
        Location? location = null,
        bool isError = true
    )
    {
        _location = location;
        _message = message;
        _objet = objet;

        ErrorType = errorType;
        IsError = isError;
    }

    public bool IsError { get; }

    public ErrorType ErrorType { get; }

    public ModelFile File => _objet.ModelFile;

    public Class? Class =>
        _objet switch
        {
            Class classe => classe,
            IProperty { Class: Class classe } => classe,
            _ => null,
        };

    public Endpoint? Endpoint =>
        _objet switch
        {
            Endpoint endpoint => endpoint,
            IProperty { Endpoint: Endpoint endpoint } => endpoint,
            _ => null,
        };

    public Location? Location => _location ?? _objet.Declaration;

    public IProperty? Property => _objet as IProperty;

    public string Message => string.Format(_message, _location?.Value);

    public bool IsIgnored(ModelConfig config)
    {
        var isIgnoredComment =
            File.Comments.TryGetValue((int)(Location?.Start.Line ?? 1), out var comment)
            && (
                comment.Split(" ").FirstOrDefault()?.Equals("ignore", StringComparison.InvariantCultureIgnoreCase)
                ?? false
            )
            && comment
                .Split(" ")
                .Any(word => word.Equals(Enum.GetName(ErrorType)!, StringComparison.InvariantCultureIgnoreCase));

        return !IsError && (config.NoWarn.Contains(ErrorType) || isIgnoredComment);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(File.Path);
        sb.Append(Location?.Position ?? string.Empty);

        sb.Append(" - ");
        sb.Append(ErrorType.FormatDiagnosticMessage(Message));
        sb.Append(" (");
        sb.Append(File);

        if (Class != null)
        {
            sb.Append($"/{Class.Name}");
        }
        else if (Endpoint != null)
        {
            sb.Append($"/{Endpoint.Name}");
        }
        else if (_objet is Domain d)
        {
            sb.Append($"/{d.Name}");
        }

        switch (Property)
        {
            case RegularProperty rp:
                sb.Append($"/{rp.Name}");
                break;
            case AssociationProperty:
                sb.Append("/{association}");
                break;
            case AliasProperty:
                sb.Append("/{alias}");
                break;
            case CompositionProperty:
                sb.Append("/{composition}");
                break;
        }

        sb.Append(')');

        return sb.ToString();
    }

    internal static ModelError Error(ErrorType errorType, string[] messageArgs, object objet, Location? location = null)
    {
        return new(errorType, objet, errorType.GetMessage(messageArgs), location, isError: true);
    }

    internal static ModelError Warning(
        ErrorType errorType,
        string[] messageArgs,
        object objet,
        Location? location = null
    )
    {
        return new(errorType, objet, errorType.GetMessage(messageArgs), location, isError: false);
    }
}
