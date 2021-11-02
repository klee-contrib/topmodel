using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopModel.Core.FileModel;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator.Jpa
{
    /// <summary>
    /// Générateur de fichiers de modèles JPA.
    /// </summary>
    public class JpaModelGenerator : GeneratorBase
    {
        private readonly JpaConfig _config;
        private readonly ILogger<JpaModelGenerator> _logger;
        private readonly IDictionary<string, ModelFile> _files = new Dictionary<string, ModelFile>();

        public JpaModelGenerator(ILogger<JpaModelGenerator> logger, JpaConfig config)
            : base(logger, config)
        {
            _config = config;
            _logger = logger;
        }

        public override string Name => "JpaModelGen";

        protected override void HandleFiles(IEnumerable<ModelFile> files)
        {
            foreach (var file in files)
            {
                _files[file.Name] = file;
            }

            var modules = files.SelectMany(f => f.Classes.Select(c => c.Namespace.Module)).Distinct();

            foreach (var module in modules)
            {
                GenerateModule(module);
            }
        }

        private void GenerateModule(string module)
        {
            var classes = _files.Values
                .SelectMany(f => f.Classes)
                .Distinct()
                .Where(c => c.Namespace.Module == module);

            foreach (var classe in classes)
            {
                var entityDto = classe.IsPersistent ? "entities" : "dtos";
                var destFolder = Path.Combine(_config.ModelOutputDirectory, Path.Combine(_config.DaoPackageName.Split(".")), entityDto, classe.Namespace.Module.ToLower());
                var dirInfo = Directory.CreateDirectory(destFolder);
                var packageName = $"{_config.DaoPackageName}.{entityDto}.{classe.Namespace.Module.ToLower()}";
                using var fw = new JavaWriter($"{destFolder}/{classe.Name}.java", _logger, null);
                fw.WriteLine($"package {packageName};");
                fw.WriteLine();

                WriteImports(fw, classe);
                fw.WriteLine();

                WriteAnnotations(module, fw, classe);

                fw.WriteClassDeclaration(classe.Name, null, null, "Serializable");

                fw.WriteLine("	/** Serial ID */");
                fw.WriteLine("    private static final long serialVersionUID = 1L;");
                fw.WriteLine();
                WriteProperties(module, fw, classe);
                WriteGetters(fw, classe);
                fw.WriteLine("}");
            }

            GenerateEnums(module);
        }

        private void GenerateEnums(string module)
        {
            var classes = _files.Values
               .SelectMany(f => f.Classes)
               .Distinct()
               .Where(c => c.Namespace.Module == module);

            foreach (var classe in classes.Where(c => c.ReferenceValues?.Any() ?? false))
            {
                var destFolder = Path.Combine(_config.ModelOutputDirectory, Path.Combine(_config.DaoPackageName.Split(".")), "references", classe.Namespace.Module.ToLower());
                var dirInfo = Directory.CreateDirectory(destFolder);
                var packageName = $"{_config.DaoPackageName}.references.{classe.Namespace.Module.ToLower()}";
                using var fw = new JavaWriter($"{destFolder}/{classe.Name}Code.java", _logger, null);
                fw.WriteLine($"package {packageName};");
                fw.WriteLine();

                fw.WriteLine();

                fw.WriteLine($"public enum {classe.Name}Code {{");

                var i = 0;

                foreach (var refValue in classe.ReferenceValues!.OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    ++i;
                    var code = classe.PrimaryKey?.Domain.Name != "DO_ID"
                        ? (string)refValue.Value[classe.PrimaryKey ?? classe.Properties.OfType<IFieldProperty>().First()]
                        : (string)refValue.Value[classe.UniqueKeys!.First().First()];
                    var label = classe.LabelProperty != null
                        ? (string)refValue.Value[classe.LabelProperty]
                        : refValue.Name;

                    fw.WriteDocStart(1, label);
                    fw.WriteDocEnd(1);
                    fw.WriteLine(1, code.ToUpper() + (i == classe.ReferenceValues!.Count ? ";" : ","));
                }

                fw.WriteLine();
                fw.WriteLine("}");
            }
        }

        private void WriteImports(JavaWriter fw, Class classe)
        {
            var imports = new List<string>
            {
                "lombok.NoArgsConstructor",
                "lombok.Builder",
                "lombok.Setter",
                "lombok.ToString",
                "lombok.EqualsAndHashCode",
                "lombok.AllArgsConstructor",
                "java.io.Serializable"
            };
            if (classe.IsPersistent)
            {
                imports.Add("javax.persistence.Entity");
                imports.Add("javax.persistence.Table");
            }

            foreach (var property in classe.Properties.OfType<IFieldProperty>())
            {
                if (property.Domain.Java!.Import != null)
                {
                    imports.Add($"{property.Domain.Java!.Import}.{property.Domain.Java!.Type}");
                }
            }

            if (classe.Properties.Any(property => property is CompositionProperty))
            {
                foreach (var cp in classe.Properties.OfType<CompositionProperty>())
                {
                    var entityDto = classe.IsPersistent ? "entities" : "dtos";
                    var packageName = $"{_config.DaoPackageName}.{entityDto}.{cp.Composition.Namespace.Module.ToLower()}";
                    imports.Add($"{packageName}.{cp.Composition.Name}");
                }
            }

            if (!classe.IsPersistent)
            {
                if (classe.Properties.Any(p => p is IFieldProperty { Required: true })
                    || classe.Properties.Any(p => p is AliasProperty { Required: true }))
                {
                    imports.Add("javax.validation.constraints.NotNull");
                }

                imports.Sort();
                fw.WriteImports(imports.Distinct().ToArray());
                return;
            }

            if (classe.Properties.OfType<IFieldProperty>().Any())
            {
                imports.Add("javax.persistence.Column");
            }

            if (classe.Properties.Any(property =>
                    property is IFieldProperty { Name: "CreatedDate" }
                || property is IFieldProperty { Name: "createdDate" }))
            {
                imports.Add("org.springframework.data.annotation.CreatedDate");
                imports.Add("javax.persistence.EntityListeners");
                imports.Add("org.springframework.data.jpa.domain.support.AuditingEntityListener");
            }

            if (classe.Properties.Any(property =>
                  property is IFieldProperty { Name: "lastModifiedDate" }
                || property is IFieldProperty { Name: "LastModifiedDate" }))
            {
                imports.Add("org.springframework.data.annotation.LastModifiedDate");
                imports.Add("javax.persistence.EntityListeners");
                imports.Add("org.springframework.data.jpa.domain.support.AuditingEntityListener");
            }

            if (classe.PrimaryKey is not null)
            {
                imports.Add("javax.persistence.Id");
                if (
                    classe.PrimaryKey.Domain.Java!.Type == "Long"
                || classe.PrimaryKey.Domain.Java.Type == "long"
                || classe.PrimaryKey.Domain.Java.Type == "int"
                || classe.PrimaryKey.Domain.Java.Type == "Integer")
                {
                    imports.Add("javax.persistence.GeneratedValue");
                    imports.Add("javax.persistence.SequenceGenerator");
                    imports.Add("javax.persistence.GenerationType");
                }
            }

            if (classe.Properties.Any(property => property is AssociationProperty))
            {
                foreach (var ap in classe.Properties.OfType<AssociationProperty>())
                {
                    imports.Add($"javax.persistence.{((AssociationProperty)ap).Type}");
                    if (ap.Association.Namespace.Module != classe.Namespace.Module)
                    {
                        var entityDto = classe.IsPersistent ? "entities" : "dtos";
                        var packageName = $"{_config.DaoPackageName}.{entityDto}.{ap.Association.Namespace.Module.ToLower()}";
                        imports.Add($"{packageName}.{ap.Association.Name}");
                    }
                }

                if (classe.Properties.Any(property => property is AssociationProperty { Type: AssociationType.OneToOne }))
                {
                    imports.Add("javax.persistence.FetchType");
                }

                if (classe.Properties.Any(property => property is AssociationProperty { Type: AssociationType.OneToMany }))
                {
                    imports.Add("java.util.Set");
                    imports.Add("java.util.HashSet");
                    imports.Add("javax.persistence.FetchType");
                    imports.Add("javax.persistence.CascadeType");
                }

                if (classe.Properties.Any(property => property is AssociationProperty { Type: AssociationType.ManyToOne }))
                {
                    imports.Add("javax.persistence.FetchType");
                    imports.Add("javax.persistence.JoinColumn");
                }

                if (classe.Properties.Any(property => property is AssociationProperty { Type: AssociationType.ManyToMany }))
                {
                    imports.Add("java.util.Set");
                    imports.Add("java.util.HashSet");
                    imports.Add("javax.persistence.FetchType");
                    imports.Add("javax.persistence.JoinColumn");
                    imports.Add("javax.persistence.JoinTable");
                }
            }

            if (classe.Reference)
            {
                imports.Add("javax.persistence.Enumerated");
                imports.Add("javax.persistence.EnumType");
                imports.Add("org.hibernate.annotations.Cache");
                imports.Add("org.hibernate.annotations.Cache");
                imports.Add("org.hibernate.annotations.Immutable");
                imports.Add("org.hibernate.annotations.CacheConcurrencyStrategy");
                imports.Add($"{_config.DaoPackageName}.references.{classe.Namespace.Module.ToLower()}.{classe.Name}Code");
            }

            if (classe.UniqueKeys?.Count > 0)
            {
                imports.Add("javax.persistence.UniqueConstraint");
            }

            fw.WriteImports(imports.Distinct().ToArray());
        }

        private void WriteAnnotations(string module, JavaWriter fw, Class classe)
        {
            fw.WriteDocStart(0, classe.Comment);
            fw.WriteDocEnd(0);
            fw.WriteLine("@Builder");
            fw.WriteLine("@Setter");
            fw.WriteLine("@NoArgsConstructor");
            fw.WriteLine("@AllArgsConstructor");
            fw.WriteLine("@EqualsAndHashCode");
            fw.WriteLine("@ToString");

            if (classe.IsPersistent)
            {
                var table = @$"@Table(name = ""{classe.SqlName}""";
                if (classe.UniqueKeys?.Count > 0)
                {
                    table += ", uniqueConstraints = {";
                    var isFirstConstraint = true;
                    foreach (var unique in classe.UniqueKeys)
                    {
                        if (!isFirstConstraint)
                        {
                            table += ",";
                        }

                        table += "\n    ";
                        isFirstConstraint = false;
                        table += "@UniqueConstraint(columnNames = {";
                        var isFirstColumn = true;
                        foreach (var u in unique)
                        {
                            if (!isFirstColumn)
                            {
                                table += ",";
                            }

                            isFirstColumn = false;
                            table += $"\"{u.SqlName}\"";
                        }

                        table += "})";
                    }

                    table += "}";
                }

                table += ")";
                fw.WriteLine("@Entity");
                fw.WriteLine(table);
            }

            if (classe.Reference)
            {
                fw.WriteLine("@Cache(usage = CacheConcurrencyStrategy.READ_ONLY)");
                fw.WriteLine("@Immutable");
            }

            if (classe.Properties.Any(property =>
                     property is IFieldProperty { Name: "CreatedDate" }
                 || property is IFieldProperty { Name: "createdDate" }
                 || property is IFieldProperty { Name: "updatedDate" }
                 || property is IFieldProperty { Name: "UpdatedDate" }))
            {
                fw.WriteLine("@EntityListeners(AuditingEntityListener.class)");
            }
        }

        private void WriteProperties(string module, JavaWriter fw, Class classe)
        {
            foreach (var property in classe.Properties)
            {
                if (property is AssociationProperty ap)
                {
                    if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
                    {
                        fw.WriteLine(1, $"private Set<{ap.Association.Name}> {ap.GetAssociationName()};");
                    }
                    else
                    {
                        fw.WriteLine(1, $"private {ap.Association.Name} {ap.GetAssociationName()};");
                    }
                }
                else if (property is IFieldProperty field)
                {
                    var isRefCode = classe.Reference && field.PrimaryKey;
                    fw.WriteLine(1, $"private {(isRefCode ? $"{classe.Name.ToFirstUpper()}Code" : field.Domain.Java!.Type)} {field.Name.ToFirstLower()};");
                }
                else if (property is CompositionProperty cp)
                {
                    if (cp.Kind == "List")
                    {
                        fw.WriteLine(1, $"private Set<{cp.Composition.Name}> {cp.Name};");
                    }
                    else
                    {
                        fw.WriteLine(1, $"private {cp.Composition.Name} {cp.Name};");
                    }
                }
            }
        }

        private void WriteGetters(JavaWriter fw, Class classe)
        {
            foreach (var property in classe.Properties)
            {
                fw.WriteLine();
                fw.WriteDocStart(1, property.Comment);

                if (property is AssociationProperty ap)
                {
                    fw.WriteReturns(1, $"value of {ap.GetAssociationName()}");
                    fw.WriteDocEnd(1);
                    var fk = (ap.Role is not null ? ModelUtils.ConvertCsharp2Bdd(ap.Role) + "_" : string.Empty) + ap.Association.PrimaryKey!.SqlName;
                    var pk = ap.Association.PrimaryKey.SqlName;
                    switch (ap.Type)
                    {
                        case AssociationType.ManyToOne:
                            fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY)");
                            fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{pk}"")");
                            break;
                        case AssociationType.OneToMany:
                            fw.WriteLine(1, @$"@{ap.Type}(cascade=CascadeType.ALL, mappedBy = ""{classe.Name.ToFirstLower()}"", orphanRemoval = true)");
                            break;
                        case AssociationType.ManyToMany:
                            fw.WriteLine(1, @$"@{ap.Type}");
                            fw.WriteLine(1, @$"@JoinTable(name = ""{ap.Class.SqlName}_{ap.Association.SqlName}"", joinColumns = @JoinColumn(name = ""{pk}""), inverseJoinColumns = @JoinColumn(name = ""{fk}""))");
                            break;
                        case AssociationType.OneToOne:
                            fw.WriteLine(1, @$"@{ap.Type}(fetch = FetchType.LAZY)");
                            fw.WriteLine(1, @$"@JoinColumn(name = ""{fk}"", referencedColumnName = ""{pk}"")");
                            break;
                    }

                    if (ap.Type == AssociationType.OneToMany || ap.Type == AssociationType.ManyToMany)
                    {
                        fw.WriteLine(1, @$"public Set<{ap.Association.Name}> get{ap.GetAssociationName().ToFirstUpper()}() {{");
                        fw.WriteLine(2, @$"if({ap.GetAssociationName()} == null) this.{ap.GetAssociationName()}= new HashSet<>();");
                    }
                    else
                    {
                        fw.WriteLine(1, @$"public {ap.Association.Name} get{ap.GetAssociationName().ToFirstUpper()}() {{");
                    }

                    fw.WriteLine(2, @$"return this.{ap.GetAssociationName()};");
                }
                else if (property is CompositionProperty cp)
                {
                    fw.WriteReturns(1, $"value of {cp.Composition.Name}");
                    fw.WriteDocEnd(1);

                    if (cp.Kind == "list")
                    {
                        fw.WriteLine(1, @$"public Set<{cp.Composition.Name}> get{cp.Composition.Name.ToFirstUpper()}() {{");
                        fw.WriteLine(2, @$"if({cp.Composition.Name.ToFirstUpper()} == null) this.{cp.Composition.Name.ToFirstUpper()}= new HashSet<>();");
                    }
                    else
                    {
                        fw.WriteLine(1, @$"public {cp.Composition.Name.ToFirstUpper()} get{cp.Name.ToFirstUpper()}() {{");
                    }

                    fw.WriteLine(2, @$"return this.{cp.Name};");
                }
                else if (property is IFieldProperty field)
                {
                    fw.WriteReturns(1, $"value of {property.Name.ToFirstLower()}");
                    fw.WriteDocEnd(1);
                    if (field.PrimaryKey && classe.IsPersistent)
                    {
                        fw.WriteLine(1, "@Id");
                        if (
                            field.Domain.Java!.Type == "Long"
                            || field.Domain.Java.Type == "long"
                            || field.Domain.Java.Type == "int"
                            || field.Domain.Java.Type == "Integer")
                        {
                            var seqName = $"SEQ_{classe.SqlName}";
                            fw.WriteLine(1, @$"@SequenceGenerator(name = ""{seqName}"", sequenceName = ""{seqName}"", allocationSize = 1)");
                            fw.WriteLine(1, @$"@GeneratedValue(strategy = GenerationType.SEQUENCE, generator = ""{seqName}"")");
                        }
                    }

                    if (classe.IsPersistent)
                    {
                        var column = @$"@Column(name = ""{field.SqlName}"", nullable = {(!field.Required).ToString().ToFirstLower()}";
                        if (field.Domain.Length != null)
                        {
                            if (field.Domain.Java!.Type == "String" || field.Domain.Java.Type == "string")
                            {
                                column += $", length = {field.Domain.Length}";
                            }
                            else
                            {
                                column += $", precision = {field.Domain.Length}";
                            }
                        }

                        if (field.Domain.Scale != null)
                        {
                            column += $", scale = {field.Domain.Scale}";
                        }

                        column += ")";
                        fw.WriteLine(1, column);
                    }
                    else if (field.Required && !field.PrimaryKey)
                    {
                        fw.WriteLine(1, @$"@NotNull");
                    }

                    if (field.PrimaryKey && classe.Reference)
                    {
                        fw.WriteLine(1, "@Enumerated(EnumType.STRING)");
                    }

                    if (property.Name == "lastModifiedDate")
                    {
                        fw.WriteLine(1, "@LastModifiedDate");
                    }

                    if (property.Name == "createdDate")
                    {
                        fw.WriteLine(1, "@CreatedDate");
                    }

                    fw.WriteLine(1, @$"public {(classe.Reference && field.PrimaryKey ? $"{classe.Name.ToFirstUpper()}Code" : field.Domain.Java!.Type.Split('.').Last())} get{field.Name.ToFirstUpper()}() {{");
                    fw.WriteLine(2, @$" return this.{property.Name.ToFirstLower()};");
                }

                fw.WriteLine(1, "}");
            }
        }
    }
}