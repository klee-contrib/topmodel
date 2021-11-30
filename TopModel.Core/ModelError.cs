using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.Core;

internal class ModelError
{
    private readonly string _message;
    private readonly object _objet;
    private readonly Reference? _reference;

    public ModelError(object objet, string message, Reference? reference = null)
    {
        _message = message;
        _objet = objet;
        _reference = reference;
    }

    public ModelFile File => _objet switch
    {
        ModelFile file => file,
        Class classe => classe.ModelFile,
        Endpoint endpoint => endpoint.ModelFile,
        IProperty { Class: Class classe } => classe.ModelFile,
        IProperty { Endpoint: Endpoint endpoint } => endpoint.ModelFile,
        Alias alias => alias.ModelFile,
        _ => throw new ArgumentException("Type d'objet non supporté.")
    };

    public Class? Class => _objet switch
    {
        Class classe => classe,
        IProperty { Class: Class classe } => classe,
        _ => null
    };

    public Endpoint? Endpoint => _objet switch
    {
        Endpoint endpoint => endpoint,
        IProperty { Endpoint: Endpoint endpoint } => endpoint,
        _ => null
    };

    public IProperty? Property => _objet as IProperty;

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append(File.Path);

        if (_reference != null)
        {
            sb.Append(_reference.Position);
        }

        sb.Append(" - ");
        sb.Append(string.Format(_message, _reference?.ReferenceName));
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
        else
        {
            sb.Append("/{{alias}}");
        }

        switch (Property)
        {
            case RegularProperty rp:
                sb.Append($"/{rp.Name}");
                break;
            case AssociationProperty rp:
                sb.Append($"/{{association}}");
                break;
            case AliasProperty rp:
                sb.Append($"/{{alias}}");
                break;
            case CompositionProperty rp:
                sb.Append($"/{{composition}}");
                break;
        }

        sb.Append(')');

        return sb.ToString();
    }
}
