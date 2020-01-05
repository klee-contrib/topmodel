using System.Linq;
using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Model
{
    public interface IFieldProperty : IProperty
    {
        bool Required { get; }
        Domain Domain { get; }
        string Comment { get; }

        string GetTSType() { 
            switch (Domain.CsharpType)
            {
                case "ICollection<string>":
                    return "string[]";
                case "ICollection<int>":
                    return "number[]";
            }

            if (Domain.CsharpType == "string" && this is AssociationProperty ap && ap.Association.Stereotype == "Statique")
            {
                return $"{ap.Association.Name}Code";
            }
            else if (Domain.CsharpType == "string" && (Domain.SqlType?.Contains("json") ?? false))
            {
                return "{}";
            }

            return TSUtils.CSharpToTSType(Domain.CsharpType);
        }
    }
}
