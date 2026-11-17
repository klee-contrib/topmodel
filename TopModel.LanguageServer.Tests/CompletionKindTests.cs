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
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - name: a
              domain: |
        """,
        nameof(CompletionKind.Domain)
    )]
    [DataRow(
        """
        ---
        module: Test
        uses:
          - |
        """,
        nameof(CompletionKind.File)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          extends: |
        """,
        nameof(CompletionKind.Class)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          defaultProperty: |
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          returns:
            composition: |
        """,
        nameof(CompletionKind.Class)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          returns:
            |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          values:
            A: { Code: A, |}
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          values:
            A:
              |
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          values:
            |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          mappers:
            from:
              - params:
                  - class: B
                    mappings:
                      |
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          returns: |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          returns:|
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test

          params:|
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          params:
            - |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          unique:
            - [A, |]
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          indexes:
            - [|]
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          indexes:
            - properties: [A, |]
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          indexes:
            - unique: |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - alias:
                class: B
                include: [A, |]
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - alias:
                class: B
                exclude:
                  - |
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          params:
            - alias:
                class: B
                property: |
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          annotations:
            - Authenticated
            - RoleRequired:
                ro|
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          annotations:
            - RoleRequired:
                role: ADMIN
                |
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          annotations:
            - RoleRequired: { |}
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - name: a
              annotations:
                - Annot:
                    |
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          annotations:
            - RoleRequired:
                role: |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        endpoint:
          name: Test
          annotations:
            - Role|
        """,
        nameof(CompletionKind.Annotation)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          decorators:
            - DateCreation
            - Deco:
                pa|
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          decorators:
            - Deco: { |}
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          decorators:
            - Deco:
                param: |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          decorators:
            - De|
        """,
        nameof(CompletionKind.Decorator)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - name: a
              domain:
                name: DO_A
                parameters:
                  |
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - name: a
              domain: { name: DO_A, parameters: { |} }
        """,
        nameof(CompletionKind.Parameter)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - name: a
              domain:
                name: |
        """,
        nameof(CompletionKind.Domain)
    )]
    [DataRow(
        """
        ---
        domain:
          name: |
        """,
        nameof(CompletionKind.None)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - association: B
              property: |
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          properties:
            - alias: { class: B, include: [A, |] }
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          mappers:
            to:
              - class: B
                mappings:
                  X: |
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          mappers:
            to:
              - class: B
                mappings: { X: |}
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          mappers:
            to:
              - class: B
                mappings: { X: Y, |}
        """,
        nameof(CompletionKind.OwnProperty)
    )]
    [DataRow(
        """
        ---
        dataFlow:
          name: Test
          sources:
            - class: B
              joinProperties: [A, |]
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        dataFlow:
          name: Test
          sources:
            - class: B
              joinProperties:
                - |
        """,
        nameof(CompletionKind.TargetProperty)
    )]
    [DataRow(
        """
        ---
        class:
          name: Test
          propertyAnnotations:
            - Annot:
                |
        """,
        nameof(CompletionKind.Parameter)
    )]
    public void FromText(string text, string expected)
    {
        var path = CursorText.ParseContext(text).Path;
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
    [DataRow("endpoint.annotations[1].RoleRequired.")]
    [DataRow("class.properties[0].annotations[0].Annot.")]
    [DataRow("class.propertyAnnotations[0].Annot.")]
    [DataRow("domain.annotations[0].Annot.")]
    [DataRow("decorator.annotations[0].Annot.")]
    [DataRow("class.decorators[0].Deco.")]
    [DataRow("endpoint.decorators[1].WithToken.")]
    [DataRow("class.properties[0].domain.parameters.")]
    public void Parameter(string path)
    {
        Assert.AreEqual(CompletionKind.Parameter, CompletionHandler.GetCompletionKind(path));
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
