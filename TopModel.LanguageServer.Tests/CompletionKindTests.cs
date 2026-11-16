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
    [DataRow("endpoint.annotations[1].RoleRequired.")]
    [DataRow("class.properties[0].annotations[0].Annot.")]
    [DataRow("class.propertyAnnotations[0].Annot.")]
    [DataRow("domain.annotations[0].Annot.")]
    [DataRow("decorator.annotations[0].Annot.")]
    public void AnnotationParameter(string path)
    {
        Assert.AreEqual(CompletionKind.AnnotationParameter, CompletionHandler.GetCompletionKind(path));
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
    [DataRow("class.decorators[0].Deco.")]
    [DataRow("endpoint.decorators[1].WithToken.")]
    public void DecoratorParameter(string path)
    {
        Assert.AreEqual(CompletionKind.DecoratorParameter, CompletionHandler.GetCompletionKind(path));
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
    [DataRow("---\nclass:\n  name: Test\n  defaultProperty: |", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nendpoint:\n  name: Test\n  returns:\n    composition: |", nameof(CompletionKind.Class))]
    [DataRow("---\nendpoint:\n  name: Test\n  returns:\n    |", nameof(CompletionKind.None))]
    [DataRow("---\nclass:\n  name: Test\n  values:\n    A: { Code: A, |}", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nclass:\n  name: Test\n  values:\n    A:\n      |", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nclass:\n  name: Test\n  values:\n    |", nameof(CompletionKind.None))]
    [DataRow(
        "---\nclass:\n  name: Test\n  mappers:\n    from:\n      - params:\n          - class: B\n            mappings:\n              |",
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow("---\nendpoint:\n  name: Test\n  returns: |", nameof(CompletionKind.None))]
    [DataRow("---\nendpoint:\n  name: Test\n  returns:|", nameof(CompletionKind.None))]
    [DataRow("---\nendpoint:\n  name: Test\n\n  params:|", nameof(CompletionKind.None))]
    [DataRow("---\nendpoint:\n  name: Test\n  params:\n    - |", nameof(CompletionKind.None))]
    [DataRow("---\nclass:\n  name: Test\n  unique:\n    - [A, |]", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nclass:\n  name: Test\n  indexes:\n    - [|]", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nclass:\n  name: Test\n  indexes:\n    - properties: [A, |]", nameof(CompletionKind.OwnProperty))]
    [DataRow("---\nclass:\n  name: Test\n  indexes:\n    - unique: |", nameof(CompletionKind.None))]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - alias:\n        class: B\n        include: [A, |]",
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - alias:\n        class: B\n        exclude:\n          - |",
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        "---\nendpoint:\n  name: Test\n  params:\n    - alias:\n        class: B\n        property: |",
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        "---\nendpoint:\n  name: Test\n  annotations:\n    - Authenticated\n    - RoleRequired:\n        ro|",
        nameof(CompletionKind.AnnotationParameter)
    )]
    [DataRow(
        "---\nendpoint:\n  name: Test\n  annotations:\n    - RoleRequired:\n        role: ADMIN\n        |",
        nameof(CompletionKind.AnnotationParameter)
    )]
    [DataRow(
        "---\nendpoint:\n  name: Test\n  annotations:\n    - RoleRequired: { |}",
        nameof(CompletionKind.AnnotationParameter)
    )]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - name: a\n      annotations:\n        - Annot:\n            |",
        nameof(CompletionKind.AnnotationParameter)
    )]
    [DataRow(
        "---\nendpoint:\n  name: Test\n  annotations:\n    - RoleRequired:\n        role: |",
        nameof(CompletionKind.None)
    )]
    [DataRow("---\nendpoint:\n  name: Test\n  annotations:\n    - Role|", nameof(CompletionKind.Annotation))]
    [DataRow(
        "---\nclass:\n  name: Test\n  decorators:\n    - DateCreation\n    - Deco:\n        pa|",
        nameof(CompletionKind.DecoratorParameter)
    )]
    [DataRow("---\nclass:\n  name: Test\n  decorators:\n    - Deco: { |}", nameof(CompletionKind.DecoratorParameter))]
    [DataRow("---\nclass:\n  name: Test\n  decorators:\n    - Deco:\n        param: |", nameof(CompletionKind.None))]
    [DataRow("---\nclass:\n  name: Test\n  decorators:\n    - De|", nameof(CompletionKind.Decorator))]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - name: a\n      domain:\n        name: DO_A\n        parameters:\n          |",
        nameof(CompletionKind.DomainParameter)
    )]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - name: a\n      domain: { name: DO_A, parameters: { |} }",
        nameof(CompletionKind.DomainParameter)
    )]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - name: a\n      domain:\n        name: |",
        nameof(CompletionKind.Domain)
    )]
    [DataRow("---\ndomain:\n  name: |", nameof(CompletionKind.None))]
    [DataRow(
        "---\nclass:\n  name: Test\n  properties:\n    - association: B\n      property: |",
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        "---\nclass:\n  name: Test\n  propertyAnnotations:\n    - Annot:\n        |",
        nameof(CompletionKind.AnnotationParameter)
    )]
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
    [DataRow("class.values.")]
    [DataRow("domain.java.values.")]
    [DataRow("endpoint.returns.")]
    public void NewKey_None(string path)
    {
        Assert.AreEqual(CompletionKind.None, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.values.A.")]
    [DataRow("class.mappers.from[0].params[0].mappings.")]
    [DataRow("class.mappers.to[0].mappings.")]
    public void NewKey_OwnProperty(string path)
    {
        Assert.AreEqual(CompletionKind.OwnProperty, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.name")]
    [DataRow("class.properties[0].name")]
    [DataRow("class.properties[0].alias")]
    [DataRow("class.unique[0]")]
    [DataRow("class.indexes[0].unique")]
    [DataRow("class.values.A.Code")]
    [DataRow("endpoint.params[0]")]
    [DataRow("endpoint.annotations[0].RoleRequired.role")]
    [DataRow("decorator.target[0]")]
    [DataRow("dataFlow.target")]
    [DataRow("class")]
    [DataRow("endpoint")]
    [DataRow("domain")]
    [DataRow("decorator")]
    [DataRow("annotation")]
    [DataRow("domain.name")]
    [DataRow("annotation.target[0]")]
    [DataRow("annotation.parameters[0].name")]
    [DataRow("decorator.parameters[0].name")]
    [DataRow("domain.java.values.template")]
    [DataRow("class.properties[0].domain.parameters.precision")]
    public void None(string path)
    {
        Assert.AreEqual(CompletionKind.None, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.defaultProperty")]
    [DataRow("class.orderProperty")]
    [DataRow("class.flagProperty")]
    [DataRow("class.localeProperty")]
    [DataRow("class.discriminatorProperty")]
    [DataRow("class.unique[1][1]")]
    [DataRow("class.indexes[0][1]")]
    [DataRow("class.indexes[1].properties[0]")]
    [DataRow("class.mappers.from[0].params[1].target")]
    public void OwnProperty(string path)
    {
        Assert.AreEqual(CompletionKind.OwnProperty, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("class.properties[0].alias.property")]
    [DataRow("class.properties[0].alias.include")]
    [DataRow("class.properties[0].alias.include[1]")]
    [DataRow("class.properties[0].alias.exclude[0]")]
    [DataRow("endpoint.returns.alias.include[0]")]
    [DataRow("decorator.properties[0].alias.property")]
    [DataRow("class.mappers.from[0].params[0].mappings.Libelle")]
    [DataRow("class.mappers.to[0].mappings.Libelle")]
    [DataRow("class.properties[0].property")]
    [DataRow("class.properties[0].alias.property[1]")]
    [DataRow("dataFlow.activeProperty")]
    [DataRow("dataFlow.sources[0].joinProperties[1]")]
    public void Property(string path)
    {
        Assert.AreEqual(CompletionKind.TargetProperty, CompletionHandler.GetCompletionKind(path));
    }

    [TestMethod]
    [DataRow("tags[0]")]
    public void Tag(string path)
    {
        Assert.AreEqual(CompletionKind.Tag, CompletionHandler.GetCompletionKind(path));
    }
}
