using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;
using TopModel.Core.Model;
using TopModel.Utils;

namespace TopModel.Generator.Core;

public abstract class TranslationGeneratorBase<T>(
    ILogger<TranslationGeneratorBase<T>> logger,
    TranslationStore translationStore,
    IFileWriterProvider writerProvider
) : GeneratorBase<T>(logger, writerProvider)
    where T : GeneratorConfigBase
{
    public override IEnumerable<string> GeneratedFiles =>
        Config
            .Tags.SelectMany(tag =>
            {
                var properties = Config.Classes.Where(c => c.Tags.Contains(tag)).SelectMany(Config.GetProperties);

                return properties
                    .SelectMany(p => GetResourceFileNames(p, tag))
                    .Concat(properties.SelectMany(p => GetCommentResourceFileNames(p, tag)))
                    .Concat(GetMainResourceFileNames(tag))
                    .Select(p => p.FilePath);
            })
            .Distinct();

    protected virtual string? GetCommentResourceFilePath(IProperty property, string tag, string lang)
    {
        return null;
    }

    protected virtual string? GetMainResourceFilePath(string tag, string lang)
    {
        return null;
    }

    protected abstract string? GetResourceFilePath(IProperty property, string tag, string lang);

    protected virtual void HandleCommentResourceFile(
        string filePath,
        string lang,
        IEnumerable<IProperty> properties
    ) { }

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        var modules = new List<(string MainFilePath, string ModuleFilePath, string ModuleName)>();

        Parallel.ForEach(
            Config
                .Classes.SelectMany(classe =>
                    Config
                        .Tags.Intersect(classe.Tags)
                        .SelectMany(tag =>
                            Config
                                .GetProperties(classe)
                                .SelectMany(p =>
                                    GetResourceFileNames(p, tag)
                                        .Select(f =>
                                            (
                                                key: (
                                                    MainFilePath: GetMainResourceFilePath(tag, f.Lang),
                                                    ModuleFilePath: f.FilePath,
                                                    f.Lang
                                                ),
                                                value: (p, tag)
                                            )
                                        )
                                )
                        )
                )
                .GroupBy(f => f.key),
            resources =>
            {
                if (CancellationToken?.IsCancellationRequested ?? false)
                {
                    return;
                }

                var properties = resources.Select(r => (r.value.p.ResourceProperty)).Distinct();
                var tag = resources.First().value.tag;
                HandleResourceFile(resources.Key.ModuleFilePath, tag, resources.Key.Lang, properties);

                if (resources.Key.MainFilePath != null)
                {
                    modules.Add(
                        (
                            resources.Key.MainFilePath,
                            resources.Key.ModuleFilePath,
                            Config.GetRootModule(properties.First().Parent.Namespace)
                        )
                    );
                }
            }
        );

        Parallel.ForEach(
            Config
                .Classes.SelectMany(classe =>
                    Config
                        .Tags.Intersect(classe.Tags)
                        .SelectMany(tag =>
                            Config
                                .GetProperties(classe)
                                .SelectMany(p =>
                                    GetCommentResourceFileNames(p, tag)
                                        .Select(f =>
                                            (
                                                key: (
                                                    MainFilePath: GetMainResourceFilePath(tag, f.Lang),
                                                    ModuleFilePath: f.FilePath,
                                                    f.Lang
                                                ),
                                                p
                                            )
                                        )
                                )
                        )
                )
                .GroupBy(f => f.key),
            resources =>
            {
                if (CancellationToken?.IsCancellationRequested ?? false)
                {
                    return;
                }

                var properties = resources.Select(r => r.p.CommentResourceProperty).Distinct();
                HandleCommentResourceFile(resources.Key.ModuleFilePath, resources.Key.Lang, properties);

                if (resources.Key.MainFilePath != null)
                {
                    modules.Add(
                        (
                            resources.Key.MainFilePath,
                            resources.Key.ModuleFilePath,
                            $"{Config.GetRootModule(properties.First().Parent.Namespace)}Comments"
                        )
                    );
                }
            }
        );

        Parallel.ForEach(
            modules.GroupBy(m => m.MainFilePath),
            g =>
            {
                if (CancellationToken?.IsCancellationRequested ?? false)
                {
                    return;
                }

                HandleMainResourceFile(
                    g.Key,
                    g.Select(l => (l.ModuleFilePath, l.ModuleName)).OrderBy(m => m.ModuleFilePath)
                );
            }
        );
    }

    protected virtual void HandleMainResourceFile(
        string mainFilePath,
        IEnumerable<(string ModuleFilePath, string ModuleName)> modules
    ) { }

    protected abstract void HandleResourceFile(
        string filePath,
        string tag,
        string lang,
        IEnumerable<IProperty> properties
    );

    private IEnumerable<(string Lang, string FilePath)> GetCommentResourceFileNames(IProperty property, string tag)
    {
        if (Config.TranslateProperties != true)
        {
            return [];
        }

        return translationStore
            .Translations.Select(lang =>
                (lang: lang.Key, file: GetCommentResourceFilePath(property.CommentResourceProperty, tag, lang.Key)!)
            )
            .Where(g => g.file != null);
    }

    private IEnumerable<(string Lang, string FilePath)> GetMainResourceFileNames(string tag)
    {
        return translationStore
            .Translations.Select(lang => (lang: lang.Key, file: GetMainResourceFilePath(tag, lang.Key)!))
            .Where(g => g.file != null);
    }

    private IEnumerable<(string Lang, string FilePath)> GetResourceFileNames(IProperty property, string tag)
    {
        if (
            Config.TranslateProperties != true
            && (Config.TranslateReferences != true || !(property.Class?.Values.Any() ?? false))
        )
        {
            return [];
        }

        return translationStore
            .Translations.Select(lang =>
                (lang: lang.Key, file: GetResourceFilePath(property.ResourceProperty, tag, lang.Key)!)
            )
            .Where(g => g.file != null);
    }
}
