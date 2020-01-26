using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopModel.Core.FileModel;

namespace TopModel.UI.Graphing
{
    public class Digraph
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public Digraph(ModelFile modelFile)
        {
            _sb.Append(@$"digraph ""{modelFile}"" {{
  charset=utf8
  tooltip="" ""
  node [fontname = ""Segoe UI"" fontsize = 8 shape = record]
  edge [fontname = ""Segoe UI"" fontsize = 8]

");

            foreach (var classe in modelFile.Classes)
            {
                AddNode(classe, n => n.AddLabel());
                AddExtendsEdge(classe, modelFile.Classes);
                foreach (var prop in classe.Properties)
                {
                    AddEdge(classe, prop, modelFile.Classes);
                }
            }
        }

        public override string ToString()
        {
            _sb.Append("}");
            return _sb.ToString();
        }

        private Digraph AddNode(object classe, Action<Node> node)
        {
            var n = new Node(classe);
            node(n);
            _sb.Append(n);
            _sb.Append("\r\n");
            return this;
        }

        private Digraph AddEdge(Class from, IProperty prop, IEnumerable<Class> classes)
        {
            var to = prop is AssociationProperty ap ? ap.Association : prop is CompositionProperty cp ? cp.Composition : null;

            if (to != null)
            {
                if (!classes.Any(c => c.Name == to.Name))
                {
                    AddNode(to, n => n
                        .AddProp("label", $"{to}\\n({to.ModelFile})")
                        .AddProp("URL", to.ModelFile.GetURL()));
                }

                AddNode($"{from}->{to}", n =>
                {
                    n
                        .AddProp("tooltip", prop.Comment.ForTooltip())
                        .AddProp("color", "#101088")
                        .AddProp("fontcolor", "#101088")
                        .AddProp("arrowhead", "vee");

                    if (prop is AssociationProperty ap)
                    {
                        n.AddProp("headlabel", $"  {(ap.Required ? "1..1" : "0..1")}{(ap.Role != null ? $" {ap.Role}" : string.Empty)}  ");
                    }
                    else if (prop is CompositionProperty cp)
                    {
                        n
                           .AddProp("arrowtail", "odiamond")
                           .AddProp("dir", "both")
                           .AddProp("headlabel", $"  {(cp.Kind == Composition.Object ? "1..1" : "0..n")}  ");
                    }
                });
            }

            return this;
        }

        private Digraph AddExtendsEdge(Class classe, IEnumerable<Class> classes)
        {
            if (classe.Extends != null)
            {
                if (!classes.Any(c => c.Name == classe.Extends.Name))
                {
                    AddNode(classe.Extends, n => n
                        .AddProp("label", $"{classe.Extends}\\n({classe.Extends.ModelFile})")
                        .AddProp("URL", classe.Extends.ModelFile.GetURL()));
                }

                AddNode($"{classe}->{classe.Extends}", n => n
                    .AddProp("color", "#101088")
                    .AddProp("fontcolor", "#101088")
                    .AddProp("arrowhead", "onormal")
                    .AddProp("arrowsize", "2"));
            }

            return this;
        }
    }
}