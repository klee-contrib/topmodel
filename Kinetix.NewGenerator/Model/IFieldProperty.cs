using Kinetix.Tools.Common;

namespace Kinetix.NewGenerator.Model
{
    public interface IFieldProperty : IProperty
    {
        bool Required { get; }
        Domain Domain { get; }

        string TSType
        {
            get
            {
                if (Domain.CsharpType == "string")
                {
                    var prop = this is AliasProperty alp ? alp.Property : this;

                    if (prop is AssociationProperty ap && ap.Association.Stereotype == Stereotype.Statique)
                    {
                        return $"{ap.Association.Name}{ap.Association.PrimaryKey!.Name}";
                    }

                    if (prop.PrimaryKey && prop.Class.Stereotype == Stereotype.Statique)
                    {
                        return $"{prop.Class.Name}{prop.Name}";
                    }

                    if (Domain.SqlType?.Contains("json") ?? false)
                    {
                        return "{}";
                    }
                }

                return TSUtils.CSharpToTSType(Domain.CsharpType);
            }
        }

        string SqlName
        {
            get
            {
                var prop = this is AliasProperty alp ? alp.Property : this;
                return prop is AssociationProperty ap
                    ? ap.Association.PrimaryKey!.SqlName + (ap.Role != null ? $"_{ap.Role.ToUpper()}" : string.Empty)
                    : $"{Class.Trigram}_{TSUtils.ConvertCsharp2Bdd(Name)}";
            }
        }
    }
}
