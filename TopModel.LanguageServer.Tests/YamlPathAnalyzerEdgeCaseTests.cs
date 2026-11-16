using TopModel.LanguageServer.Handlers;
using static TopModel.LanguageServer.Tests.CursorText;

namespace TopModel.LanguageServer.Tests;

/// <summary>
/// Cas limites de <see cref="YamlPathAnalyzer.GetPath" /> sur des positions où une complétion est attendue :
/// position exacte du curseur, remontée dans l'arborescence, flux inline, guillemets et commentaires.
/// </summary>
[TestClass]
public class YamlPathAnalyzerEdgeCaseTests
{
    [TestMethod]
    public void Comment_AfterValue_CursorBeforeComment()
    {
        AssertPath(
            "class.properties[0].domain",
            """
            ---
            class:
              properties:
                - name: a
                  domain: DO|_ID # commentaire
            """
        );
    }

    [TestMethod]
    public void Cursor_BeyondLineLength_IsClampedToEndOfLine()
    {
        var text = new[] { "---", "class:", "  extends: Pers" };
        Assert.AreEqual("class.extends", YamlPathAnalyzer.GetPath(text, 2, 200));
    }

    [TestMethod]
    public void Cursor_DirectlyAfterColon_IsValue()
    {
        AssertPath(
            "class.extends",
            """
            ---
            class:
              extends:|
            """
        );
    }

    [TestMethod]
    public void Flow_ClosedInlineList_ThenCursorAfter()
    {
        AssertPath(
            "class.unique[0][2]",
            """
            ---
            class:
              unique:
                - [A, B, |]
            """
        );
    }

    [TestMethod]
    public void Flow_FirstItemOfInlineList()
    {
        AssertPath(
            "class.properties[0].alias.include[0]",
            """
            ---
            class:
              properties:
                - alias:
                    class: Personne
                    include: [No|]
            """
        );
    }

    [TestMethod]
    public void Flow_NewKeyAfterComma()
    {
        AssertPath(
            "class.values.A.",
            """
            ---
            class:
              values:
                A: { Code: A, | }
            """
        );
    }

    [TestMethod]
    public void Flow_QuotedCommaAndColon_AreIgnored()
    {
        AssertPath(
            "class.values.A.",
            """
            ---
            class:
              values:
                A: { Libelle: "a, b: c", Co| }
            """
        );
    }

    [TestMethod]
    public void List_IndexCountsOnlySiblings_NotNestedLists()
    {
        AssertPath(
            "class.properties[1].domain",
            """
            ---
            class:
              properties:
                - name: a
                  domain: DO_ID
                  annotations:
                    - A1
                    - A2
                - name: b
                  domain: |
            """
        );
    }

    [TestMethod]
    public void List_NestedListOfObjects()
    {
        AssertPath(
            "class.mappers.from[0].params[1].class",
            """
            ---
            class:
              mappers:
                from:
                  - params:
                      - class: Personne
                      - class: |
            """
        );
    }

    [TestMethod]
    public void List_SameIndentAsKey_Nested()
    {
        AssertPath(
            "class.properties[0].alias.include[1]",
            """
            ---
            class:
              properties:
                - alias:
                    class: Personne
                    include:
                    - Nom
                    - |
            """
        );
    }

    [TestMethod]
    public void List_ScalarItemBeingTyped()
    {
        AssertPath(
            "class.decorators[1]",
            """
            ---
            class:
              decorators:
                - Audit
                - Dat|
            """
        );
    }

    [TestMethod]
    public void List_SecondListAfterNestedList()
    {
        AssertPath(
            "class.mappers.to[0].class",
            """
            ---
            class:
              mappers:
                from:
                  - params:
                      - class: Personne
                to:
                  - class: |
            """
        );
    }

    [TestMethod]
    public void MultilineScalar_CursorInText()
    {
        AssertPath(
            "class.comment",
            """
            ---
            class:
              comment: >
                Une longue des|cription
            """
        );
    }

    [TestMethod]
    public void MultilineScalar_FollowingKeyIsNotAffected()
    {
        AssertPath(
            "class.extends",
            """
            ---
            class:
              comment: >
                Une longue description
                sur plusieurs lignes
              extends: |
            """
        );
    }

    [TestMethod]
    public void MultilineScalar_TextLookingLikeAKey_IsNotAKey()
    {
        AssertPath(
            "class.properties[0].comment",
            """
            ---
            class:
              properties:
                - name: a
                  comment: >-
                    Exemple :
                    domain: DO_|ID
            """
        );
    }

    [TestMethod]
    public void Sibling_AfterNestedList()
    {
        AssertPath(
            "class.extends",
            """
            ---
            class:
              name: Test
              properties:
                - name: a
                  domain: DO_ID
                  annotations:
                    - A1
              extends: |
            """
        );
    }
}
