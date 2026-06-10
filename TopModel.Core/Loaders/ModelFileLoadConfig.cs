namespace TopModel.Core.Loaders;

public record ModelFileLoadConfig(
    string App,
    string ModelRoot,
    bool DefaultAssociationUseClass,
    bool PluralizeTableNames,
    bool UseLegacyRoleNames
)
{
    public string GetFileName(string filePath)
    {
        return Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), ModelRoot), filePath)
            .Replace(".tmd", string.Empty)
            .Replace('\\', '/');
    }
}
