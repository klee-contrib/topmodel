using System.Text;
using Microsoft.Extensions.Localization;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Core.Utils;
using TopModel.Utils;

namespace TopModel.Core;

public class ModelError
{
    private readonly string _message;
    private readonly object _objet;
    private readonly Reference? _reference;

    internal ModelError(
        ErrorType errorType,
        object objet,
        string message,
        Reference? reference = null,
        bool isError = true
    )
    {
        _message = message;
        _objet = objet;
        _reference = reference;

        ErrorType = errorType;
        IsError = isError;
    }

    internal ModelError(
        IStringLocalizer localizer,
        ErrorType errorType,
        string[] messageArgs,
        object objet,
        Reference? reference = null,
        bool isError = true
    )
        : this(errorType, objet, localizer.GetString(Enum.GetName(errorType)!, messageArgs), reference, isError) { }

    public bool IsError { get; }

    public ErrorType ErrorType { get; }

    public ModelFile File => _objet.GetFile();

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

    public Reference? Location => _reference ?? _objet.GetLocation();

    public IProperty? Property => _objet as IProperty;

    public string Message => string.Format(_message, _reference?.ReferenceName);

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

        sb.Append(" - {");
        sb.Append(ErrorType);
        sb.Append("} ");
        sb.Append(Message);
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
}
