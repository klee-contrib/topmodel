using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Utils;

namespace TopModel.Generator.Javascript;

using static JavascriptUtils;

/// <summary>
/// Générateur de définitions Typescript.
/// </summary>
public class TypescriptReferenceGenerator : ClassGroupGeneratorBase
{
    private readonly JavascriptConfig _config;
    private readonly ILogger<TypescriptReferenceGenerator> _logger;

    private readonly ModelConfig _modelConfig;

    public TypescriptReferenceGenerator(ILogger<TypescriptReferenceGenerator> logger, JavascriptConfig config, ModelConfig modelConfig)
        : base(logger, config)
    {
        _config = config;
        _logger = logger;
        _modelConfig = modelConfig;
    }

    public override string Name => "JSReferenceGen";

    protected override object? GetDomainType(Domain domain)
    {
        return domain.TS;
    }

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (classe.IsJSReference())
        {
            yield return ("main", _config.GetReferencesFileName(classe.Namespace, tag));
        }
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        GenerateReferenceFile(fileName, classes.OrderBy(r => r.NameCamel), tag);
    }

    /// <summary>
    /// Create the template output
    /// </summary>
    private void GenerateReferenceFile(string fileName, IEnumerable<Class> references, string tag)
    {
        using var fw = new FileWriter(fileName, _logger, false);

        var imports = references
            .SelectMany(r => r.ClassDependencies)
            .Select(dep => (
                Import: dep.Source switch
                {
                    IProperty fp => fp.GetPropertyTypeName(Classes),
                    Class c => c.NamePascal,
                    _ => null!
                },
                Path: _config.GetImportPathForClass(dep, tag, Classes)!))
            .Concat(references.SelectMany(r => r.DomainDependencies).Select(p => (Import: p.Domain.TS!.Type.ParseTemplate(p.Source).Replace("[]", string.Empty).Split("<").First(), Path: p.Domain.TS.Import!.ParseTemplate(p.Source))))
            .Where(i => i.Path != null && i.Path != $"./references")
            .GroupAndSort();

        foreach (var import in imports)
        {
            fw.Write("import {");
            fw.Write(import.Import);
            fw.Write("} from \"");
            fw.Write(import.Path);
            fw.Write("\";\r\n");
        }

        if (imports.Any())
        {
            fw.Write("\r\n");
        }

        var first = true;
        foreach (var reference in references)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                fw.WriteLine();
            }

            if (reference.EnumKey != null)
            {
                fw.Write("export type ");
                fw.Write(reference.NamePascal);
                fw.Write($"{reference.EnumKey.Name.ToPascalCase()} = ");
                fw.Write(string.Join(" | ", reference.Values.Select(r => $@"""{r.Value[reference.EnumKey]}""").OrderBy(x => x, StringComparer.Ordinal)));
                fw.WriteLine(";");

                foreach (var uk in reference.UniqueKeys.Where(uk => uk.Count == 1 && uk.Single().Required).Select(uk => uk.Single()))
                {
                    fw.Write("export type ");
                    fw.Write(reference.NamePascal);
                    fw.Write($"{uk} = ");
                    fw.Write(string.Join(" | ", reference.Values.Select(r => $@"""{r.Value[uk]}""").OrderBy(x => x, StringComparer.Ordinal)));
                    fw.WriteLine(";");
                }
            }

            if (reference.FlagProperty != null)
            {
                fw.Write($"export enum {reference.NamePascal}Flag {{\r\n");

                var flagValues = reference.Values.Where(refValue => refValue.Value.ContainsKey(reference.FlagProperty) && int.TryParse(refValue.Value[reference.FlagProperty], out var _)).ToList();
                foreach (var refValue in flagValues)
                {
                    var flag = int.Parse(refValue.Value[reference.FlagProperty]);
                    fw.Write($"    {refValue.Name} = 0b{Convert.ToString(flag, 2)}");
                    if (flagValues.IndexOf(refValue) != flagValues.Count - 1)
                    {
                        fw.WriteLine(",");
                    }
                }

                fw.WriteLine("\r\n}");
            }

            if (reference.Reference)
            {
                fw.Write("export interface ");
                fw.Write(reference.NamePascal);
                fw.Write(" {\r\n");

                foreach (var property in reference.Properties.OfType<IFieldProperty>())
                {
                    fw.Write("    ");
                    fw.Write(property.NameCamel);
                    fw.Write(property.Required || property.PrimaryKey ? string.Empty : "?");
                    fw.Write(": ");
                    fw.Write(property.GetPropertyTypeName(Classes));
                    fw.Write(";\r\n");
                }

                fw.Write("}\r\n");

                if (_config.ReferenceMode == ReferenceMode.VALUES)
                {
                    WriteReferenceValues(fw, reference);
                }
                else
                {
                    WriteReferenceDefinition(fw, reference);
                }
            }
        }
    }

    private void WriteReferenceValues(FileWriter fw, Class reference)
    {
        fw.Write("export const ");
        fw.Write(reference.NameCamel);
        fw.Write($"List: {reference.NamePascal}[] = [");
        fw.WriteLine();
        foreach (var refValue in reference.Values)
        {
            fw.WriteLine("    {");
            fw.Write("        ");
            fw.Write(string.Join(",\n        ", refValue.Value.Where(p => p.Value != "null").Select(property => $"{property.Key.NameCamel}: {(property.Key.Domain.TS!.Type == "string" ? @$"""{(_modelConfig.I18n.TranslateReferences && property.Key == property.Key.Class.DefaultProperty ? refValue.ResourceKey : property.Value)}""" : @$"{property.Value}")}")));
            fw.WriteLine();
            fw.WriteLine("    },");
        }

        fw.WriteLine("];");
        fw.WriteLine();
    }
}