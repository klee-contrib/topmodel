using System.Linq;
using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.UI
{
    public static class ModelFileToDot
    {
        public static string ToDot(ModelFile modelFile)
        {
            var sb = new StringBuilder();
            sb.Append(@$"digraph ""{modelFile}"" {{
  node [fontname = ""Segoe UI"" fontsize = 8 shape = record]
  edge [fontname = ""Segoe UI"" fontsize = 8]

");
            var relationships = modelFile.Relationships.ToDictionary(r => r.Source, r => r.Target);
            foreach (var classe in modelFile.Classes)
            {
                sb.Append("  ");
                sb.Append(classe.Name);
                sb.Append("[");
                sb.Append($@"tooltip = ""{classe.Comment ?? string.Empty}"" label =");
                sb.Append(@"<<table border = ""0"" cellspacing = ""0"">");
                sb.Append(@$"<tr><td colspan=""2""><u><b>{classe.Name}</b></u></td></tr>");
                sb.Append(@"<tr><td colspan=""2""></td></tr>");

                foreach (var prop in classe.Properties.OfType<IFieldProperty>())
                {
                    if (prop is AssociationProperty)
                    {
                        continue;
                    }

                    if (prop is AliasProperty alp)
                    {
                        var alias = relationships[alp];
                        sb.Append($"<tr><td align=\"left\"><b>{alp.Prefix ?? string.Empty}{alias.Value}{alp.Suffix ?? string.Empty}          </b></td><td align=\"left\"><i>[{alias.Peer!.Value}]     </i></td></tr>");
                    }
                    else
                    {
                        sb.Append($"<tr><td align=\"left\"><b>{prop.Name}{(prop.Required ? "?" : string.Empty)}          </b></td><td align=\"left\"><i>{relationships[prop].Value}     </i></td></tr>");
                    }
                }

                sb.Append("</table>>]\r\n");

                foreach (var prop in classe.Properties.OfType<AssociationProperty>())
                {
                    sb.Append($"  {classe.Name}->{relationships[prop].Value}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty headlabel=\"  {(prop.Required ? "1..1" : "0..1")}{(prop.Role != null ? $" {prop.Role}" : string.Empty)}   \"]\r\n");
                }

                foreach (var prop in classe.Properties.OfType<CompositionProperty>())
                {
                    sb.Append($"  {classe.Name}->{relationships[prop].Value}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty arrowtail=odiamond dir=both headlabel=\"  {(prop.Kind == Composition.Object ? "1..1" : "0..n")}  \"]\r\n");
                }

                sb.Append("\r\n");
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}