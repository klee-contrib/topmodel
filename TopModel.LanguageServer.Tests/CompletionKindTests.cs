using TopModel.LanguageServer.Handlers;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class CompletionKindTests
{
    [TestMethod]
    [DataRow("class.annotations[0]")]
    [DataRow("class.properties[0].annotations[0]")]
    [DataRow("class.properties[0].excludedAnnotations[0]")]
    public void Annotation(string path)
    {
        Assert.AreEqual(CompletionKind.Annotation, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.properties[0].association")]
    [DataRow("class.properties[0].composition")]
    [DataRow("class.properties[0].alias.class")]
    [DataRow("class.extends")]
    [DataRow("class.implements[0]")]
    [DataRow("class.mappers.from[0].params[0].class")]
    public void Class(string path)
    {
        Assert.AreEqual(CompletionKind.Class, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("dataFlow.dependsOn[0]")]
    public void DataFlow(string path)
    {
        Assert.AreEqual(CompletionKind.DataFlow, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.decorators[0]")]
    [DataRow("class.properties[0].alias.decorator")]
    public void Decorator(string path)
    {
        Assert.AreEqual(CompletionKind.Decorator, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.properties[0].domain")]
    [DataRow("decorator.properties[0].domain")]
    [DataRow("domain.asDomains.list")]
    [DataRow("converter.from")]
    [DataRow("converter.from[0]")]
    [DataRow("converter.to[1]")]
    public void Domain(string path)
    {
        Assert.AreEqual(CompletionKind.Domain, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("endpoint.params[0].alias.endpoint")]
    public void Endpoint(string path)
    {
        Assert.AreEqual(CompletionKind.Endpoint, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("uses[1]")]
    public void File(string path)
    {
        Assert.AreEqual(CompletionKind.File, CompletionHandler.GetCompletionKind(path));
    }

    /// <summary>
    /// Enchaînement des étapes 1 et 2 sur des snippets complets.
    /// </summary>
    [TestMethod]
    [DataRow("---\nclass:\n  name: Test\n  properties:\n    - name: a\n      domain: |", nameof(CompletionKind.Domain))]
    [DataRow("---\nmodule: Test\nuses:\n  - |", nameof(CompletionKind.File))]
    [DataRow("---\nclass:\n  name: Test\n  extends: |", nameof(CompletionKind.Class))]
    [DataRow("---\nclass:\n  name: Test\n  defaultProperty: |", nameof(CompletionKind.Property))]
    public void FromText(string text, string expected)
    {
        var cursor = CursorText.Parse(text);
        var path = YamlPathAnalyzer.GetPath(cursor.Lines, cursor.Line, cursor.Character);
        Assert.AreEqual(expected, CompletionHandler.GetCompletionKind(path).ToString(), $"Chemin : {path}");
    }

    [TestMethod]
    [DataRow("domain.")]
    [DataRow("decorator.")]
    [DataRow("annotation.")]
    [DataRow("class.properties[0].")]
    public void NewKey_IsNotCompletedAsReference(string path)
    {
        Assert.AreEqual(CompletionKind.Property, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.defaultProperty")]
    [DataRow("class.unique[1][1]")]
    [DataRow("class.properties[0].alias.include[1]")]
    [DataRow("class.mappers.from[0].params[0].mappings.Libelle")]
    public void Property(string path)
    {
        Assert.AreEqual(CompletionKind.Property, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("tags[0]")]
    public void Tag(string path)
    {
        Assert.AreEqual(CompletionKind.Tag, CompletionHandler.GetCompletionKind(path));
    }
}
