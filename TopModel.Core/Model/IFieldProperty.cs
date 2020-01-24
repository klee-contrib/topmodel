namespace TopModel.Core
{
    public interface IFieldProperty : IProperty
    {
        bool Required { get; }

        Domain Domain { get; }

        string? DefaultValue { get; }

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

                return ModelUtils.CSharpToTSType(Domain.CsharpType);
            }
        }

        string SqlName
        {
            get
            {
                var prop = this is AliasProperty alp ? alp.Property : this;

                return prop.Class.Extends != null && prop.PrimaryKey
                    ? $"{Class.Trigram}_{ModelUtils.ConvertCsharp2Bdd(Name).Replace(prop.Class.SqlName + "_", string.Empty)}"
                    : prop is AssociationProperty ap
                    ? ap.Association.PrimaryKey!.SqlName + (ap.Role != null ? $"_{ap.Role.Replace(" ", "_").ToUpper()}" : string.Empty)
                    : $"{prop.Class.Trigram}_{ModelUtils.ConvertCsharp2Bdd(prop.Name)}";
            }
        }
    }
}
