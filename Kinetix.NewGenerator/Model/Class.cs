using System.Collections.Generic;
using System.Linq;
using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Model
{
    public class Class
    {
        private string? _sqlName;

        public string? Trigram { get; set; }

#nullable disable
        public string Name { get; set; }
        public string Comment { get; set; }
#nullable enable
        public Class? Extends { get; set; }
        public string? Label { get; set; }
        public Stereotype? Stereotype { get; set; }
        public string? OrderProperty { get; set; }
        public string? DefaultProperty { get; set; }

        public IList<IProperty> Properties { get; } = new List<IProperty>();
        public Namespace Namespace { get; set; }

        public IFieldProperty? PrimaryKey => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.PrimaryKey);
        public IFieldProperty? LabelProperty => Properties.OfType<IFieldProperty>().SingleOrDefault(p => p.Name == (DefaultProperty ?? "Libelle"));

        public IDictionary<string, (string code, string label)>? ReferenceValues { get; set; }

        public string SqlName
        {
            get => _sqlName ?? TSUtils.ConvertCsharp2Bdd(Name);
            set => _sqlName = value;
        }

        public string TrigramPrefix => string.IsNullOrEmpty(Trigram) ? string.Empty : $"{Trigram}_";
    }
}
