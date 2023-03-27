using Microsoft.Extensions.Logging;
using TopModel.Core;
using TopModel.Core.FileModel;

namespace TopModel.Generator.Core;

public abstract class TranslationGeneratorBase<T> : GeneratorBase<T>
    where T : GeneratorConfigBase
{
    private readonly TranslationStore _translationStore;

    public TranslationGeneratorBase(ILogger<TranslationGeneratorBase<T>> logger, TranslationStore translationStore)
        : base(logger)
    {
        _translationStore = translationStore;
    }

    public override IEnumerable<string> GeneratedFiles => Config.Tags
        .SelectMany(tag =>
        {
            var properties = Classes
                .Where(c => GetClassTags(c).Contains(tag))
                .SelectMany(c => c.Properties.OfType<IFieldProperty>());

            return properties.SelectMany(p => GetResourceFileNames(p, tag))
                .Concat(properties.SelectMany(p => GetCommentResourceFileNames(p, tag)))
                .Select(p => p.FilePath);
        })
        .Distinct();

    protected abstract string? GetResourceFilePath(IFieldProperty property, string tag, string lang);

    protected virtual string? GetCommentResourceFilePath(IFieldProperty property, string tag, string lang)
    {
        return null;
    }

    protected virtual void HandleCommentResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties)
    {
    }

    protected abstract void HandleResourceFile(string filePath, string lang, IEnumerable<IFieldProperty> properties);

    protected override void HandleFiles(IEnumerable<ModelFile> files)
    {
        foreach (var resources in Classes
            .SelectMany(classe => Config.Tags.Intersect(GetClassTags(classe))
                .SelectMany(tag => classe.Properties.OfType<IFieldProperty>()
                    .SelectMany(p => GetResourceFileNames(p, tag)
                    .Select(f => (key: (f.FilePath, f.Lang), p)))))
                .GroupBy(f => f.key))
        {
            HandleResourceFile(resources.Key.FilePath, resources.Key.Lang, resources.Select(r => r.p.ResourceProperty).Distinct());
        }

        foreach (var resources in Classes
            .SelectMany(classe => Config.Tags.Intersect(GetClassTags(classe))
                .SelectMany(tag => classe.Properties.OfType<IFieldProperty>()
                    .SelectMany(p => GetCommentResourceFileNames(p, tag)
                    .Select(f => (key: (f.FilePath, f.Lang), p)))))
                .GroupBy(f => f.key))
        {
            HandleCommentResourceFile(resources.Key.FilePath, resources.Key.Lang, resources.Select(r => r.p.CommentResourceProperty).Distinct());
        }
    }

    private IEnumerable<(string Lang, string FilePath)> GetCommentResourceFileNames(IFieldProperty property, string tag)
    {
        return _translationStore.Translations
            .Select(lang => (lang: lang.Key, file: GetCommentResourceFilePath(property.CommentResourceProperty, tag, lang.Key)!))
            .Where(g => g.file != null);
    }

    private IEnumerable<(string Lang, string FilePath)> GetResourceFileNames(IFieldProperty property, string tag)
    {
        return _translationStore.Translations
            .Select(lang => (lang: lang.Key, file: GetResourceFilePath(property.ResourceProperty, tag, lang.Key)!))
            .Where(g => g.file != null);
    }
}