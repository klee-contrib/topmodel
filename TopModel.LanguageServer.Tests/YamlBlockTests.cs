using TopModel.LanguageServer.Handlers;
using YamlDotNet.RepresentationModel;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class YamlBlockTests
{
    [TestMethod]
    public void BlockWithOnlyBrokenLineIsUnusable()
    {
        var block = ParseBlock("class: a: b|");

        Assert.IsNull(block.Find("."));
        Assert.IsNull(block.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void BrokenCursorLineInBlockAliasKeepsClass()
    {
        var block = ParseBlock(
            """
            class:
              name: Restaurant
              properties:
                - alias:
                    class: Commande
                    include: [A, Da|
            """
        );

        Assert.AreEqual("Commande", block.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void BrokenCursorLineInInlineAliasKeepsClass()
    {
        // La ligne du curseur est réparée (coupée au séparateur, accolades fermées) au lieu d'être retirée.
        var block = ParseBlock(
            """
            class:
              name: Restaurant
              properties:
                - alias: { class: B, include: [A, Da|
            """
        );

        Assert.AreEqual("B", block.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void BrokenCursorLineInInlineDomainKeepsName()
    {
        var block = ParseBlock(
            """
            class:
              name: Restaurant
              properties:
                - name: Id
                  domain: { name: DO_ID, parameters: { |
            """
        );

        Assert.AreEqual("DO_ID", block.GetScalar("class.properties[0].domain.", "name"));
    }

    [TestMethod]
    public void BrokenCursorLineKeepsKeysBeforeCursor()
    {
        var block = ParseBlock(
            """
            class:
              name: Restaurant
              values:
                Foo: { Code: A, Lib|
            """
        );

        CollectionAssert.AreEquivalent(new[] { "Code" }, block.GetKeys("class.values.Foo.").ToList());
    }

    [TestMethod]
    public void BrokenLineAfterCursorKeepsCursorLine()
    {
        var block = ParseBlock(
            """
            class:
              name: Foo
              properties:
                - name: Id
                  domain: |
                - name: x: y
            """
        );

        Assert.AreEqual("Id", block.GetScalar("class.properties[0].", "name"));
    }

    [TestMethod]
    public void BrokenNextLineKeepsPreviousSibling()
    {
        var block = ParseBlock(
            """
            class:
              name: Foo
              properties:
                - name: Id
                  domain: |
                - name: x: y
            """
        );

        Assert.AreEqual("Id", block.GetScalar("class.properties[0].", "name"));
        Assert.AreEqual("Foo", block.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void BrokenPreviousLineTruncatesAndRefusesIndexedPaths()
    {
        var block = ParseBlock(
            """
            class:
              name: Foo
              comment: [oops
              properties:
                - name: |
            """
        );

        Assert.AreEqual("Foo", block.GetScalar("class.", "name"));
        Assert.IsNull(block.Find("class.properties[0]."));
        Assert.IsNull(block.GetScalar("class.properties[0].", "name"));
    }

    [TestMethod]
    public void DuplicateKeyBeforeCursorFallsBackToTruncation()
    {
        var block = ParseBlock(
            """
            class:
              name: A
              name: B
              comment: x|
            """
        );

        Assert.AreEqual("A", block.GetScalar("class.", "name"));
        Assert.IsNull(block.Find("class.properties[0]"));
    }

    [TestMethod]
    public void EmptyLineUnderValueParsesDirectly()
    {
        var block = ParseBlock(
            """
            class:
              name: Foo
              values:
                A:
                  ResourceKey: x
                  |
            """
        );

        Assert.AreEqual("Foo", block.GetScalar("class.", "name"));
        CollectionAssert.Contains(block.GetKeys("class.values.A.").ToList(), "ResourceKey");
    }

    [TestMethod]
    public void GetKeysExcludesKeyUnderCursorInInlineObject()
    {
        var block = ParseBlock(
            """
            class:
              name: X
              values:
                - Foo: { a: 1, Typ| }
            """
        );

        CollectionAssert.AreEquivalent(new[] { "a" }, block.GetKeys("class.values[0].Foo.").ToList());
    }

    [TestMethod]
    public void GetKeysExcludesKeyUnderCursorInMultilineObject()
    {
        var withColon = ParseBlock(
            """
            class:
              name: X
              values:
                Foo:
                  a: 1
                  Typ|:
            """
        );
        var withoutColon = ParseBlock(
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
        var withSeparator = ParseBlock(
            """
            ---
            uses:
              - |
            class:
              name: A
            """
        );
        var withoutSeparator = ParseBlock(
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
        var block = ParseBlock(
            """
            class:
              name: A
              properties:
                - alias: { class: B, include: [A, |] }
            """
        );

        Assert.AreEqual("B", block.GetScalar("class.properties[0].alias.", "class"));
    }

    [TestMethod]
    public void MultipleDocumentsReadCurrentOne()
    {
        var block = ParseBlock(
            """
            class:
              name: A
            ---
            class:
              name: B|
            """
        );

        Assert.AreEqual("B", block.GetScalar("class.", "name"));
    }

    [TestMethod]
    public void QuotedScalarIsUnquotedAndEmptyValueIsNull()
    {
        var block = ParseBlock(
            """
            class:
              name: "Foo"
              comment:|
            """
        );

        Assert.AreEqual("Foo", block.GetScalar("class.", "name"));
        Assert.IsNull(block.GetScalar("class.", "comment"));
    }

    [TestMethod]
    public void UnclosedFlowSequenceOnlyLineIsRepaired()
    {
        var block = ParseBlock("class: [|");

        Assert.IsInstanceOfType<YamlMappingNode>(block.Find("."));
    }

    private static YamlBlock ParseBlock(string text)
    {
        var cursor = CursorText.Parse(text);
        return YamlBlock.Parse(cursor.Lines, cursor.Line, cursor.Character);
    }
}
