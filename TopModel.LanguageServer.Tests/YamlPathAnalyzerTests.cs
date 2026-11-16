using TopModel.LanguageServer.Handlers;
using static TopModel.LanguageServer.Tests.CursorText;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class YamlPathAnalyzerTests
{
    [TestMethod]
    public void BlankLinesAndComments_AreIgnored()
    {
        AssertPath(
            "class.properties[0].domain",
            """
            ---
            class:
              name: Test

              # Propriétés
              properties:

                # Première propriété
                - name: a

                  domain: |
            """
        );
    }

    [TestMethod]
    public void Class_AliasClass()
    {
        AssertPath(
            "class.properties[0].alias.class",
            """
            ---
            class:
              name: Test
              properties:
                - alias:
                    class: |
            """
        );
    }

    [TestMethod]
    public void Class_AliasIncludeBlockItem()
    {
        AssertPath(
            "class.properties[0].alias.include[1]",
            """
            ---
            class:
              name: Test
              properties:
                - alias:
                    class: Personne
                    include:
                      - a
                      - |
            """
        );
    }

    [TestMethod]
    public void Class_AliasIncludeInlineItem()
    {
        AssertPath(
            "class.properties[0].alias.include[1]",
            """
            ---
            class:
              name: Test
              properties:
                - alias:
                    class: Personne
                    include: [a, |]
            """
        );
    }

    [TestMethod]
    public void Class_Association()
    {
        AssertPath(
            "class.properties[0].association",
            """
            ---
            class:
              name: Test
              properties:
                - association: |
            """
        );
    }

    [TestMethod]
    public void Class_DecoratorsItem()
    {
        AssertPath(
            "class.decorators[0]",
            """
            ---
            class:
              name: Test
              decorators:
                - |
            """
        );
    }

    [TestMethod]
    public void Class_DefaultProperty()
    {
        AssertPath(
            "class.defaultProperty",
            """
            ---
            class:
              name: Test
              defaultProperty: |
            """
        );
    }

    [TestMethod]
    public void Class_Extends()
    {
        AssertPath(
            "class.extends",
            """
            ---
            class:
              name: Test
              extends: |
            """
        );
    }

    [TestMethod]
    public void Class_ImplementsItem()
    {
        AssertPath(
            "class.implements[0]",
            """
            ---
            class:
              name: Test
              implements:
                - |
            """
        );
    }

    [TestMethod]
    public void Class_IndexesPropertiesItem()
    {
        AssertPath(
            "class.indexes[0].properties[0]",
            """
            ---
            class:
              name: Test
              indexes:
                - properties: [|]
            """
        );
    }

    [TestMethod]
    public void Class_InlineAliasClass()
    {
        AssertPath(
            "class.properties[0].alias.class",
            """
            ---
            class:
              name: Test
              properties:
                - alias: { class: | }
            """
        );
    }

    [TestMethod]
    public void Class_MappingsValue()
    {
        AssertPath(
            "class.mappers.from[0].params[0].mappings.Libelle",
            """
            ---
            class:
              name: Test
              mappers:
                from:
                  - params:
                      - class: Personne
                        mappings:
                          Libelle: |
            """
        );
    }

    [TestMethod]
    public void Class_PropertyAnnotationsItem()
    {
        AssertPath(
            "class.properties[0].annotations[0]",
            """
            ---
            class:
              name: Test
              properties:
                - name: a
                  domain: DO_ID
                  annotations:
                    - |
            """
        );
    }

    [TestMethod]
    public void Class_PropertyDomainValue()
    {
        AssertPath(
            "class.properties[2].domain",
            """
            ---
            class:
              name: Test
              properties:
                - name: a
                  domain: DO_ID
                - name: b
                  domain: DO_ID
                - name: c
                  domain: |
            """
        );
    }

    [TestMethod]
    public void Class_PropertyDomainValue_CursorInsideValue()
    {
        AssertPath(
            "class.properties[0].domain",
            """
            ---
            class:
              name: Test
              properties:
                - name: a
                  domain: DO|_ID
            """
        );
    }

    [TestMethod]
    public void Class_UniqueInlineItem()
    {
        AssertPath(
            "class.unique[1][1]",
            """
            ---
            class:
              name: Test
              unique:
                - [A, B]
                - [Code, |]
            """
        );
    }

    [TestMethod]
    public void Class_ValuesInlineNewKey()
    {
        AssertPath(
            "class.values.A.",
            """
            ---
            class:
              name: Test
              values:
                A: { Code: A, Lib| }
            """
        );
    }

    [TestMethod]
    public void Converter_FromItem()
    {
        AssertPath(
            "converter.from[0]",
            """
            ---
            converter:
              from:
                - |
            """
        );
    }

    [TestMethod]
    public void Converter_FromValue()
    {
        AssertPath(
            "converter.from",
            """
            ---
            converter:
              from: |
            """
        );
    }

    [TestMethod]
    public void DataFlow_DependsOnItem()
    {
        AssertPath(
            "dataFlow.dependsOn[0]",
            """
            ---
            dataFlow:
              name: Flux
              dependsOn:
                - |
            """
        );
    }

    [TestMethod]
    public void Decorator_PropertyDomain()
    {
        AssertPath(
            "decorator.properties[0].domain",
            """
            ---
            decorator:
              name: Audit
              properties:
                - name: a
                  domain: |
            """
        );
    }

    [TestMethod]
    public void Domain_AsDomainsValue()
    {
        AssertPath(
            "domain.asDomains.list",
            """
            ---
            domain:
              name: DO_CODE
              asDomains:
                list: |
            """
        );
    }

    [TestMethod]
    public void Endpoint_AliasEndpoint()
    {
        AssertPath(
            "endpoint.params[0].alias.endpoint",
            """
            ---
            endpoint:
              name: Get
              params:
                - alias:
                    endpoint: |
            """
        );
    }

    [TestMethod]
    public void Header_TagsItem_WithoutLeadingSeparator()
    {
        AssertPath(
            "tags[0]",
            """
            module: Test
            tags:
              - x|
            """
        );
    }

    [TestMethod]
    public void Header_UsesItem()
    {
        AssertPath(
            "uses[1]",
            """
            ---
            module: Test
            uses:
              - a
              - b|
            """
        );
    }

    [TestMethod]
    public void Header_UsesNewItem_ListAtSameIndentAsKey()
    {
        AssertPath(
            "uses[1]",
            """
            ---
            module: Test
            uses:
            - a
            - |
            """
        );
    }
}
