using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace TopModel.Generator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var provider = new ServiceCollection()
                .AddModelStore()
                .AddLogging()
                .BuildServiceProvider();

            var modelStore = provider.GetService<ModelStore>();

            var classes = modelStore.GetClassesFromFile(args[0], out var classesToResolve);

            var sb = new StringBuilder();
            sb.Append(@$"digraph ""{args[0].Split("\\").Last()}"" {{
  node [fontname = ""Segoe UI"" fontsize = 8 shape = record]
  edge [fontname = ""Segoe UI"" fontsize = 8]

");
            foreach (var classe in classes)
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
                        var alias = classesToResolve[alp].Split("|");
                        sb.Append($"<tr><td align=\"left\"><b>{alp.Prefix ?? ""}{alias[0]}{alp.Suffix ?? ""}          </b></td><td align=\"left\"><i>[{alias[1]}]     </i></td></tr>");
                    }
                    else
                    {
                        sb.Append($"<tr><td align=\"left\"><b>{prop.Name}{(prop.Required ? "?" : string.Empty)}          </b></td><td align=\"left\"><i>{classesToResolve[prop]}     </i></td></tr>");
                    }
                }

                sb.Append("</table>>]\r\n");

                foreach (var prop in classe.Properties.OfType<AssociationProperty>())
                {
                    sb.Append($"  {classe.Name}->{classesToResolve[prop]}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty headlabel=\"  {(prop.Required ? "1..1" : "0..1")}{(prop.Role != null ? $" {prop.Role}" : string.Empty)}   \"]\r\n");
                }

                foreach (var prop in classe.Properties.OfType<CompositionProperty>())
                {
                    sb.Append($"  {classe.Name}->{classesToResolve[prop]}[color=\"#101088\" fontcolor=\"#101088\" arrowhead=empty arrowtail=odiamond dir=both headlabel=\"  {(prop.Kind == Composition.Object ? "1..1" : "0..n")}  \"]\r\n");
                }

                sb.Append("\r\n");
            }

            sb.Append("}");

            Console.WriteLine(sb.ToString());
        }
    }
}