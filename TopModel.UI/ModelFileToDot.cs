using System.Linq;
using System.Net;
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
  charset=utf8
  tooltip="" ""
  node [fontname = ""Segoe UI"" fontsize = 8 shape = record]
  edge [fontname = ""Segoe UI"" fontsize = 8]

");
            foreach (var classe in modelFile.Classes)
            {
                sb.Append("  ");
                sb.Append(classe.Name);
                sb.Append("[");
                sb.Append($@"tooltip = ""{classe.Comment?.Replace("\"", "\\\"") ?? string.Empty}"" label =");
                sb.Append(@"<<table border = ""0"" cellspacing = ""0"">");
                sb.Append(@$"<tr><td colspan=""2""><u><b>{classe.Name}</b></u></td></tr>");
                sb.Append(@"<tr><td colspan=""2""></td></tr>");

                foreach (var prop in classe.Properties.OfType<IFieldProperty>())
                {
                    if (prop is AssociationProperty)
                    {
                        continue;
                    }

                    sb.Append($"<tr><td align=\"left\" href=\"\" tooltip=\"{prop.Comment.Replace("\"", "\\\"")}\"><b>");
                    sb.Append(prop.Name);
                    if (prop.Required)
                    {
                        sb.Append("?");
                    }

                    sb.Append("</b></td><td align=\"left\"");

                    if (prop is AliasProperty alp)
                    {
                        sb.Append($" href=\"./{GetURL(alp.Property.Class)}\" tooltip=\"{prop.Class.Comment.Replace("\"", "\\\"")}\">");
                        sb.Append($"<i>[{alp.Property.Class}]");
                    }
                    else
                    {
                        sb.Append($" href=\"\" tooltip=\"{prop.Domain.Label}\"><i>");
                        sb.Append(prop.Domain.Name);
                    }

                    sb.Append("     </i></td></tr>");
                }

                sb.Append("</table>>]\r\n");

                foreach (var prop in classe.Properties.OfType<AssociationProperty>())
                {
                    if (!modelFile.Classes.Any(c => c.Name == prop.Association.Name))
                    {
                        sb.Append($"  {prop.Association}[tooltip=\"{prop.Association.Comment.Replace("\"", "\\\"")}\" label=\"{prop.Association}\\n({prop.Association.ModelFile})\" URL=\"{GetURL(prop.Association)}\"]\r\n");
                    }

                    sb.Append($"  {classe.Name}->{prop.Association}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty headlabel=\"  {(prop.Required ? "1..1" : "0..1")}{(prop.Role != null ? $" {prop.Role}" : string.Empty)}   \"]\r\n");
                }

                foreach (var prop in classe.Properties.OfType<CompositionProperty>())
                {
                    if (!modelFile.Classes.Any(c => c.Name == prop.Composition.Name))
                    {
                        sb.Append($"  {prop.Composition}[tooltip=\"{prop.Composition.Comment.Replace("\"", "\\\"")}\" label=\"{prop.Composition}\\n({prop.Composition.ModelFile})\" URL=\"{GetURL(prop.Composition)}\"]\r\n");
                    }

                    sb.Append($"  {classe.Name}->{prop.Composition}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty arrowtail=odiamond dir=both headlabel=\"  {(prop.Kind == Composition.Object ? "1..1" : "0..n")}  \"]\r\n");
                }

                sb.Append("\r\n");
            }

            sb.Append("}");

            return sb.ToString();
        }

        private static string GetURL(Class classe)
        {
            return $"{classe.ModelFile.Descriptor.Module}/{classe.ModelFile.Descriptor.Kind}/{WebUtility.UrlEncode(classe.ModelFile.Descriptor.File)}";
        }
    }
}