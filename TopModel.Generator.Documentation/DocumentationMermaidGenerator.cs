namespace TopModel.Generator.Documentation;

using Microsoft.Extensions.Logging;
using TopModel.Core.Model;
using TopModel.Generator.Core;
using TopModel.Utils;
using TopModel.Utils.Mermaid;

/// <summary>
/// Générateur de documentation Markdown produisant des diagrammes de classes Mermaid.
/// </summary>
public class DocumentationMermaidGenerator(
    ILogger<DocumentationMermaidGenerator> logger,
    IFileWriterProvider writerProvider
) : ClassGroupGeneratorBase<DocumentationConfig>(logger, writerProvider)
{
    public override string Name => "DocMermaidGen";

    protected override IEnumerable<(string FileType, string FileName)> GetFileNames(Class classe, string tag)
    {
        if (!classe.IsPersistent || classe.Abstract)
        {
            yield break;
        }

        yield return ("main", Config.GetMermaidFilePath(tag, classe));
    }

    protected override void HandleFile(string fileType, string fileName, string tag, IEnumerable<Class> classes)
    {
        var classList = classes.OrderBy(c => c.ModelFile.Name + c.Namespace.Module + c.Name).ToList();
        if (classList.Count == 0)
        {
            return;
        }

        var title = Config.MermaidMode switch
        {
            DocumentationGenerationMode.All => "Diagramme global",
            DocumentationGenerationMode.File => $"Diagramme du fichier {classList[0].ModelFile.Name}",
            _ => $"Diagramme du module {classList[0].Namespace.Module}",
        };

        using var md = new MarkdownWriter(OpenFileWriter(fileName));
        md.WriteTitle(1, title);
        md.WriteLine();
        md.WriteLine("```mermaid");
        md.WriteLine("classDiagram");
        md.WriteLine(MermaidUtils.GetDiagramClasses(classList));
        md.WriteLine("```");
    }
}
