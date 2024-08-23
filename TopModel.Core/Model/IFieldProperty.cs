namespace TopModel.Core;

public interface IFieldProperty : IProperty
{
    string? DefaultValue { get; }

    IFieldProperty ResourceProperty => Decorator != null && Parent != Decorator
        ? Decorator.Properties.OfType<IFieldProperty>().First(p => p.Name == Name).ResourceProperty
        : this is AliasProperty alp && alp.Label == alp.OriginalProperty?.Label
        ? alp.OriginalProperty!.ResourceProperty
        : this;

    string ResourceKey => $"{ResourceProperty.Parent.Namespace.ModuleCamel}.{ResourceProperty.Parent.NameCamel}.{ResourceProperty.NameCamel}";

    IFieldProperty CommentResourceProperty => Decorator != null && Parent != Decorator
        ? Decorator.Properties.OfType<IFieldProperty>().First(p => p.Name == Name).CommentResourceProperty
        : this is AliasProperty alp && alp.Comment == alp.OriginalProperty?.Comment
        ? alp.OriginalProperty!.CommentResourceProperty
        : this;

    string CommentResourceKey => $"comments.{CommentResourceProperty.Parent.Namespace.ModuleCamel}.{CommentResourceProperty.Parent.NameCamel}.{CommentResourceProperty.NameCamel}";
}