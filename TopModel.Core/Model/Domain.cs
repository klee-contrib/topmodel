using System.Linq;
using System.Text.RegularExpressions;
using TopModel.Core.Types;
using YamlDotNet.Serialization;

namespace TopModel.Core
{
    public class Domain
    {
#nullable disable
        public string Name { get; set; }

        public string Label { get; set; }
#nullable enable

        [YamlMember(Alias = "csharp")]
        public CSharpType? CSharp { get; set; }

        [YamlMember(Alias = "ts")]
        public TSType? TS { get; set; }

        public string? JavaType { get; set; }

        public string? SqlType { get; set; }

        public string CSharpName => Name.Replace("DO_", string.Empty).ToPascalCase();

        public bool ShouldQuoteSqlValue =>
            (SqlType ?? string.Empty).Contains("varchar")
            || SqlType == "text"
            || CSharp?.Type == "string";

        public (int Length, int Precision)? SqlTypePrecision
        {
            get
            {
                if (SqlType == null)
                {
                    return null;
                }

                var match = Regex.Match(SqlType, @".+\((\d+),.*(\d+)\)");
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
