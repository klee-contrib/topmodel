using System.Linq;
using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.UI.Graphing
{
    public class Label
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public Label(Class classe)
        {
            _sb.Append(" label=<\r\n");
            _sb.Append("    <table border = \"0\" cellspacing = \"0\">\r\n");

            AddRow($"<u><b>{classe}</b></u>");
            AddRow();

            foreach (var prop in classe.Properties.OfType<IFieldProperty>())
            {
                if (prop is AssociationProperty)
                {
                    continue;
                }

                AddRow(
                    ($"<b>{prop.Name}{(!prop.Required ? "?" : string.Empty)}</b>", prop.Comment, null),
                    prop is AliasProperty alp ? ($"<i>[{alp.Property.Class}]</i>", prop.Class.Comment, alp.Property.Class.ModelFile) : ($"<i>{prop.Domain.Name}</i>", prop.Domain.Label, null));
            }
        }

        public override string ToString()
        {
            _sb.Append("    </table>\r\n");
            _sb.Append("  >");
            return _sb.ToString();
        }

        private Label AddRow(string title)
        {
            return AddRow((title, string.Empty, null));
        }

        private Label AddRow(params (string Label, string Tooltip, ModelFile? Href)[] cells)
        {
            _sb.Append("      <tr>");

            if (cells.Length <= 1)
            {
                _sb.Append($"<td colspan=\"2\">");

                if (cells.Length == 1)
                {
                    _sb.Append(cells[0].Label);
                }

                _sb.Append("</td>");
            }
            else
            {
                foreach (var cell in cells)
                {
                    _sb.Append($"<td align=\"left\" href=\"{cell.Href?.GetURL()}\" tooltip=\"{cell.Tooltip.ForTooltip()}\">{cell.Label}</td>");
                }
            }

            _sb.Append("</tr>\r\n");
            return this;
        }
    }
}