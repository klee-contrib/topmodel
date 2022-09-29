﻿using TopModel.Core.Types;
using TopModel.Utils;

namespace TopModel.Core;

public interface IFieldProperty : IProperty
{
    bool Required { get; }

    Domain Domain { get; }

    string? DefaultValue { get; }

    public LocatedString? Trigram { get; set; }

    TSType TS
    {
        get
        {
            if (Domain.TS == null)
            {
                throw new ModelException(Domain, $"Le type Typescript du domaine doit être renseigné.");
            }

            var fixedType = new TSType { Type = Domain.TS.Type, Import = Domain.TS.Import };

            var prop = this is AliasProperty alp ? alp.Property : this;

            if (prop is AssociationProperty ap && ap.Association.Reference && !ap.Property.Domain.AutoGeneratedValue)
            {
                fixedType.Type = $"{ap.Association.Name}{ap.Property.Name}";
            }
            else if (((prop.Class?.Reference ?? false) || (prop.Class?.ReferenceValues.Any() ?? false)) && prop.Class.PrimaryKey?.Domain.AutoGeneratedValue != true)
            {
                if (prop == (prop.Class.PrimaryKey ?? prop.Class.Properties.OfType<IFieldProperty>().First()))
                {
                    fixedType.Type = $"{prop.Class.Name}{prop.Name}";
                }
            }

            if (this is AliasProperty { ListDomain: not null })
            {
                fixedType.Type += "[]";
            }

            return fixedType;
        }
    }

    IFieldProperty ResourceProperty => this is AliasProperty alp && alp.Label == alp.Property.Label
        ? alp.Property.ResourceProperty
        : this;

    string ResourceKey => $"{string.Join('.', ResourceProperty.Class.Namespace.Module.Split('.').Select(e => e.ToFirstLower()))}.{ResourceProperty.Class.Name.ToFirstLower()}.{ResourceProperty.Name.ToFirstLower()}";

    string SqlName
    {
        get
        {
            var prop = !Class.IsPersistent && this is AliasProperty alp ? alp.Property : this;
            var snakeCaseName = prop.Name.ToSnakeCase();

            string? sqlPrefix = prop.Trigram ?? (prop is AssociationProperty ? string.Empty : null) ?? Class.Trigram;
            sqlPrefix = !string.IsNullOrWhiteSpace(sqlPrefix) ? sqlPrefix + "_" : string.Empty;
            var sqlSuffix = prop is AssociationProperty { Role: string role } ? $"_{role.Replace(" ", "_").ToUpper()}" : string.Empty;
            if (prop.Class.Extends != null && prop.PrimaryKey && sqlPrefix != string.Empty)
            {
                snakeCaseName = snakeCaseName.Replace(prop.Class.SqlName + "_", string.Empty);
            }

            if (prop is AssociationProperty asp && (asp.Association.PrimaryKey is not null || asp.Property != null) )
            {
                snakeCaseName = (asp.Association.PrimaryKey ?? asp.Property).SqlName;
            }

            return $"{sqlPrefix}{snakeCaseName}{sqlSuffix}";
        }
    }

    string JavaName => this is AssociationProperty ap
        ? (ap.Association.Trigram?.ToLower().ToFirstUpper() ?? string.Empty) + ap.Property.Name + (ap.Role?.Replace(" ", string.Empty) ?? string.Empty)
        : (Class.Trigram?.ToLower().ToFirstUpper() ?? string.Empty) + Name;
}