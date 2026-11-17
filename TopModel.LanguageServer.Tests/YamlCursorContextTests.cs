using TopModel.LanguageServer.Handlers;
using YamlDotNet.RepresentationModel;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class YamlCursorContextTests
{
    [TestMethod]
    public void BlockWithOnlyBrokenLineIsUnusable()
    {
        var context = CursorText.ParseContext("class: a: b|");

        Assert.IsNull(context.Find("."));
        Assert.IsNull(context.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void BrokenCursorLineInBlockAliasKeepsClass()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Restaurant
              properties:
                - alias:
                    class: Commande
                    include: [A, Da|
            """
        );

        Assert.AreEqual("Commande", context.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void BrokenCursorLineInInlineAliasKeepsClass()
    {
        // La ligne du curseur est réparée (coupée au séparateur, accolades fermées).
        var context = CursorText.ParseContext(
            """
            class:
              name: Restaurant
              properties:
                - alias: { class: B, include: [A, Da|
            """
        );

        Assert.AreEqual("B", context.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void BrokenCursorLineInInlineDomainKeepsName()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Restaurant
              properties:
                - name: Id
                  domain: { name: DO_ID, parameters: { |
            """
        );

        Assert.AreEqual("DO_ID", context.GetScalar("class.properties[0].domain.", "name"));
    }

    [TestMethod]
    public void BrokenCursorLineKeepsKeysBeforeCursor()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Restaurant
              values:
                Foo: { Code: A, Lib|
            """
        );

        CollectionAssert.AreEquivalent(new[] { "Code" }, context.GetKeys("class.values.Foo.").ToList());
    }

    [TestMethod]
    public void BrokenLineAfterCursorKeepsCursorLine()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Foo
              properties:
                - name: Id
                  domain: |
                - name: x: y
            """
        );

        Assert.AreEqual("Id", context.GetScalar("class.properties[0].", "name"));
    }

    [TestMethod]
    public void BrokenNextLineKeepsPreviousSibling()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Foo
              properties:
                - name: Id
                  domain: |
                - name: x: y
            """
        );

        Assert.AreEqual("Id", context.GetScalar("class.properties[0].", "name"));
        Assert.AreEqual("Foo", context.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void BrokenPreviousLineIsNeutralized()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Foo
              comment: [oops
              properties:
                - name: |
            """
        );

        Assert.AreEqual("Foo", context.GetScalar("class.", "name"));
        Assert.IsInstanceOfType<YamlMappingNode>(context.Find("class.properties[0]."));
        Assert.IsNull(context.GetScalar("class.", "comment"));
        Assert.AreEqual("class.properties[0].name", context.Path);
    }

    [TestMethod]
    public void DuplicateKeyLineBeforeCursorIsNeutralized()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: A
              name: B
              comment: x|
            """
        );

        Assert.AreEqual("A", context.GetScalar("class.", "name"));
        Assert.IsNull(context.Find("class.properties[0]"));
    }

    [TestMethod]
    public void EmptyLineUnderValueParsesDirectly()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Foo
              values:
                A:
                  ResourceKey: x
                  |
            """
        );

        Assert.AreEqual("Foo", context.GetScalar("class.", "name"));
        CollectionAssert.Contains(context.GetKeys("class.values.A.").ToList(), "ResourceKey");
    }

    [TestMethod]
    public void GetKeysExcludesKeyUnderCursorInInlineObject()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: X
              values:
                - Foo: { a: 1, Typ| }
            """
        );

        CollectionAssert.AreEquivalent(new[] { "a" }, context.GetKeys("class.values[0].Foo.").ToList());
    }

    [TestMethod]
    public void GetKeysExcludesKeyUnderCursorInMultilineObject()
    {
        var withColon = CursorText.ParseContext(
            """
            class:
              name: X
              values:
                Foo:
                  a: 1
                  Typ|:
            """
        );
        var withoutColon = CursorText.ParseContext(
            """
            class:
              name: X
              values:
                Foo:
                  a: 1
                  Typ|
            """
        );

        CollectionAssert.AreEquivalent(new[] { "a" }, withColon.GetKeys("class.values.Foo.").ToList());
        CollectionAssert.AreEquivalent(new[] { "a" }, withoutColon.GetKeys("class.values.Foo.").ToList());
    }

    [TestMethod]
    public void HeaderIsFoundWithAndWithoutLeadingSeparator()
    {
        var withSeparator = CursorText.ParseContext(
            """
            ---
            uses:
              - |
            class:
              name: A
            """
        );
        var withoutSeparator = CursorText.ParseContext(
            """
            uses:
              - |
            class:
              name: A
            """
        );

        Assert.IsInstanceOfType<YamlSequenceNode>(withSeparator.Find("uses"));
        Assert.IsInstanceOfType<YamlSequenceNode>(withoutSeparator.Find("uses"));
    }

    [TestMethod]
    public void InlineAliasIsRead()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: A
              properties:
                - alias: { class: B, include: [A, |] }
            """
        );

        Assert.AreEqual("B", context.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void MultipleDocumentsReadCurrentOne()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: A
            ---
            class:
              name: B|
            """
        );

        Assert.AreEqual("B", context.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void QuotedScalarIsUnquotedAndEmptyValueIsNull()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: "Foo"
              comment:|
            """
        );

        Assert.AreEqual("Foo", context.GetScalar("class.", "name"));
        Assert.IsNull(context.GetScalar("class.", "comment"));
    }

    [TestMethod]
    public void UnclosedFlowSequenceOnlyLineIsRepaired()
    {
        var context = CursorText.ParseContext("class: [|");

        Assert.IsInstanceOfType<YamlMappingNode>(context.Find("."));
    }

    [TestMethod]
    public void ValueBeingTypedIsReadWithoutSentinel()
    {
        var context = CursorText.ParseContext(
            """
            class:
              name: Fo|o
            """
        );

        Assert.AreEqual("Foo", context.GetScalar("class.", "name"));
    }
}
