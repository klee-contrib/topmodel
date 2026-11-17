using TopModel.LanguageServer.Handlers;
using static TopModel.LanguageServer.Tests.CursorText;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class YamlCursorContextPathTests
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

    [TestMethod]
    public void Template_Annotation_EscapedQuotes_AfterColon()
    {
        AssertPath(
            "annotation.java[0].text",
            """
            ---
            annotation:
              name: RoleRequired
              java:
                - text: 'PreAuthorize("hasRole(''{role:|
            """
        );
    }

    [TestMethod]
    public void Template_Annotation_SingleQuotedWithEscapedQuotes()
    {
        AssertPath(
            "annotation.java[0].text",
            """
            ---
            annotation:
              name: RoleRequired
              java:
                - text: 'PreAuthorize("hasRole(''{ro|
            """
        );
    }

    [TestMethod]
    public void Template_ClosedBraces_CursorInsideClosedVariable()
    {
        AssertPath(
            "domain.csharp.genericType",
            """
            ---
            domain:
              name: DO_LISTE
              csharp:
                genericType: ICollection<{T|}>
              java:
                genericType: List<{T}>
            """
        );
    }

    [TestMethod]
    public void Template_ClosedBraces_DoNotBreakFollowingLines()
    {
        AssertPath(
            "domain.csharp.type",
            """
            ---
            domain:
              name: DO_LISTE
              csharp:
                genericType: ICollection<{T}>
                collector: "{x}.ToList()"
                type: |
              java:
                genericType: List<{T}>
            """
        );
    }

    [TestMethod]
    public void Template_Converter_DoubleQuoted_AfterDot()
    {
        AssertPath(
            "converter.csharp.text",
            """
            ---
            converter:
              from:
                - DO_ID
              csharp:
                text: "{from.|
            """
        );
    }

    [TestMethod]
    public void Template_Decorator_Extends_Unquoted()
    {
        AssertPath(
            "decorator.csharp.extends",
            """
            ---
            decorator:
              name: Audit
              csharp:
                extends: Base<{|
            """
        );
    }

    [TestMethod]
    public void Template_Decorator_Implements_InlineQuoted()
    {
        AssertPath(
            "decorator.csharp.implements[0]",
            """
            ---
            decorator:
              name: Audit
              csharp:
                implements: [ "I{name|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_Converter_DoubleQuotedValue()
    {
        AssertPath(
            "converter.java.text",
            """
            ---
            converter:
              from:
                - DO_ID
              java:
                text: "{value|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_GenericType_DoubleQuoted()
    {
        AssertPath(
            "domain.ts.genericType",
            """
            ---
            domain:
              name: DO_LISTE
              ts:
                genericType: "{|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_GenericType_Unquoted()
    {
        AssertPath(
            "domain.csharp.genericType",
            """
            ---
            domain:
              name: DO_LISTE
              csharp:
                genericType: List<{|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_SequenceName_AfterDot()
    {
        AssertPath(
            "domain.autoGeneratedValue.sequenceName",
            """
            ---
            domain:
              name: DO_SEQ_ID
              autoGeneratedValue:
                sequenceName: SEQ_{class.|
                start: 1000
            """
        );
    }

    [TestMethod]
    public void Template_Domain_Values_Overrides_UnquotedBrace()
    {
        // Un "{" non quoté en début de valeur ouvre un flux YAML : le curseur est sur une clé de ce flux.
        // C'est au détecteur de template de reconnaître ce cas.
        AssertPath(
            "domain.java.values.overrides.null.value.",
            """
            ---
            domain:
              name: DO_PRIX
              java:
                type: BigDecimal
                values:
                  overrides:
                    "null":
                      value: {|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_Values_Template_AfterColon()
    {
        AssertPath(
            "domain.java.values.template",
            """
            ---
            domain:
              name: DO_PRIX
              java:
                type: BigDecimal
                values:
                  template: new BigDecimal({value:|
            """
        );
    }

    [TestMethod]
    public void Template_Domain_Values_Template_DoubleQuoted_AfterColon()
    {
        AssertPath(
            "domain.java.values.template",
            """
            ---
            domain:
              name: DO_PRIX
              java:
                values:
                  template: "new BigDecimal({value:|
            """
        );
    }

    [TestMethod]
    public void Template_FlowMapping_KeyColon_StillAddsSpace()
    {
        // Le ":" d'une clé de flux n'est pas une transformation : le curseur est sur la valeur de "name".
        AssertPath(
            "class.properties[0].domain.name",
            """
            ---
            class:
              name: Test
              properties:
                - name: a
                  domain: { name:|
            """
        );
    }

    [TestMethod]
    public void Template_OpenBrace_WithFollowingLines()
    {
        AssertPath(
            "domain.csharp.genericType",
            """
            ---
            domain:
              name: DO_LISTE
              csharp:
                genericType: List<{|
                collector: ToList()
              java:
                genericType: List<{T}>
            """
        );
    }
}
