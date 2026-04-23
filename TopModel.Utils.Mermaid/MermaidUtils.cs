using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;

namespace TopModel.Utils.Mermaid;

public static class MermaidUtils
{
    public static IEnumerable<Class> GetClassesForScope(ModelStore modelStore, ModelFile? file, MermaidScope scope)
    {
        return scope switch
        {
            MermaidScope.File => file?.Classes ?? Enumerable.Empty<Class>(),
            MermaidScope.Module => modelStore
                .Files.Where(f => f.Namespace.Module == file?.Namespace.Module)
                ?.SelectMany(f => f.Classes)
            ?? [],
            MermaidScope.Model => modelStore.Files.SelectMany(f => f.Classes),
            _ => [],
        };
    }

    public static string GetDiagram(ModelStore modelStore, ModelFile? file, MermaidScope scope)
    {
        var classes = GetClassesForScope(modelStore, file, scope).Where(c => c.IsPersistent);
        var diagram = "classDiagram\n";
        diagram += GetDiagramClasses(classes);
        return diagram;
    }

    public static string GetDiagramClasses(IEnumerable<Class> classes)
    {
        string diagram = string.Empty;
        var externalClasses = new List<Class>();
        foreach (var classe in classes)
        {
            if (classe.Properties.Any(p => p.Association == null && p.Composition == null))
            {
                diagram += @$"%% {classe.Comment.Replace("\n", "\n%% ")}" + '\n';

                diagram += @$"class {classe.Name}{{" + '\n';
                if (classe.EnumKey != null)
                {
                    diagram += "&lt;&lt;Enum&gt;&gt;\n";
                    foreach (var refValue in classe.Values.OrderBy(x => x.Name, StringComparer.Ordinal).Take(10))
                    {
                        diagram += refValue.Value[classe.EnumKey];
                        if (classe.DefaultProperty != null)
                        {
                            diagram += $" {refValue.Value[classe.DefaultProperty]}";
                        }
                        diagram += "\n";
                    }
                    if (classe.Values.Count > 10)
                    {
                        diagram += "...\n";
                    }

                    diagram += "}\n";
                    continue;
                }

                foreach (var property in classe.Properties.Where(p => p.Association == null && p.Composition == null))
                {
                    diagram += $" {property.Domain.Name} {property.Name}\n";
                }

                diagram += "}\n";
            }

            foreach (var property in classe.Properties.Where(p => p.Association != null))
            {
                if (!classes.Contains(property.Association))
                {
                    externalClasses.Add(property.Association!);
                }

                string cardLeft;
                string cardRight;

                if (property.AssociationMultiple)
                {
                    cardLeft = property.Required ? "1..*" : "0..*";
                    cardRight = property.Required ? "1" : "0..1";
                }
                else if (property.Unique)
                {
                    cardLeft = property.Required ? "1" : "0..1";
                    cardRight = "1";
                }
                else
                {
                    cardLeft = property.Required ? "1" : "0..1";
                    cardRight = "0..*";
                }

                diagram +=
                    @$"{property.Class.Name} ""{cardLeft}"" --> ""{cardRight}"" {property.Association!.Name}{(property.AssociationRole != null ? " : " + property.AssociationRole : string.Empty)}"
                    + '\n';
            }

            foreach (var property in classe.Properties.Where(c => c.Composition != null))
            {
                diagram += $"{property.Class.Name} --* {property.Composition!.Name}\n";
            }
        }

        foreach (var classe in externalClasses)
        {
            diagram += @$"%% {classe.Comment.Replace("\n", "\n%% ")}" + '\n';
            diagram += @$"class {classe.Name}:::fileReference" + '\n';
        }

        foreach (var classe in classes.Where(c => c.Extends is not null))
        {
            diagram += @$"{classe.Extends!.Name} <|--  {classe.Name}" + '\n';
        }

        diagram += "\n";
        return diagram;
    }
}
