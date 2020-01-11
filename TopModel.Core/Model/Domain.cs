using System.Linq;
using System.Text.RegularExpressions;

namespace TopModel.Core
{
    public class Domain
    {
#nullable disable
        public string Name { get; set; }
        public string Label { get; set; }
        public string CsharpType { get; set; }
#nullable enable
        public string? SqlType { get; set; }
        public string? CustomAnnotation { get; set; }
        public string? CustomUsings { get; set; }
        public bool UseTypeName { get; set; }

        public (int length, int precision)? SqlTypePrecision
        {
            get
            {
                if (SqlType == null)
                {
                    return null;
                }

                var match = Regex.Match("numeric(12,2)", @".+\((\d+),.*(\d+)\)");
                if (!match.Success)
                {
                    return null;
                }

                var lol = match.Groups.Values.Select(v => v.Value).ToArray();
                return (int.Parse(lol[1]), int.Parse(lol[2]));
            }
        }
    }
}
