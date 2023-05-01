using System.Text;
using TopModel.Core;

namespace TopModel.UI.Graphing;

public class Node
{
    private readonly Class? _class;
    private readonly StringBuilder _sb = new();

    public Node(object title)
    {
        _sb.Append($"  {title}[");
        if (title is Class classe)
        {
            _class = classe;
            _sb.Append($@"tooltip = ""{_class.Comment?.ForTooltip()}""");
        }
    }

    public Node AddLabel()
    {
        if (_class != null)
        {
            _sb.Append(new Label(_class));
        }

        return this;
    }

    public Node AddProp(string prop, string value)
    {
        _sb.Append($" {prop} = \"{value}\"");
        return this;
    }

    public override string ToString()
    {
        _sb.Append(']');
        return _sb.ToString();
    }
}