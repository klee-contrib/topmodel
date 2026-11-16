namespace TopModel.LanguageServer.IntegrationTests.Features;

[TestClass]
public class CompletionTests : LspTestBase
{
    private static readonly string DtoCommandeFile = Path.Combine("Restaurant", "Dtos", "Commande.tmd");

    private static readonly string EndpointPersonneFile = Path.Combine("Restaurant", "Endpoints", "Personne.tmd");

    private static readonly string FlowMenuFile = Path.Combine("Restaurant", "Flows", "Menu.tmd");

    private static readonly string TranslationFile = Path.Combine("meta", "Translation.tmd");

    [TestMethod]
    public async Task Completion_AliasExcludeList_ProposesTargetClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "          - |Reservation", occurrence: 0);

        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_AliasIncludeBlockList_ProposesTargetClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "          - |DateCommande", occurrence: 0);

        Assert.Contains("MontantTotal", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_AliasIncludeOfEndpoint_ProposesEndpointProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "        include: |Id");

        Assert.Contains("Id", labels);
        Assert.DoesNotContain("DateCommande", labels);
    }

    [TestMethod]
    public async Task Completion_AliasIncludeScalar_ProposesTargetClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "        include: |Lignes");

        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    /// <summary>
    /// Divergence assumée avec l'ancienne implémentation (heuristiques d'indentation), qui ne proposait rien ici.
    /// </summary>
    [TestMethod]
    public async Task Completion_AliasInlineInclude_ProposesTargetClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);
        doc.Change(
            Edit(
                doc,
                "  properties:\n    - alias:\n        class: Commande\n        exclude:\n          - Id\n",
                "  properties:\n    - alias: { class: Commande, include: [Id, Da] }\n    - alias:\n        class: Commande\n        exclude:\n          - Id\n"
            )
        );

        var labels = await CompleteAsync(doc, "include: [Id, Da|]");

        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_AliasPropertyOfDecorator_ProposesDecoratorProperties()
    {
        await using var doc = await OpenAsync(EndpointPersonneFile);
        doc.Change(
            Edit(
                doc,
                "        class: Personne\n        property: Nom\n",
                "        class: Personne\n        property: Nom\n    - alias:\n        decorator: WithToken\n        property: Token\n"
            )
        );

        var labels = await CompleteAsync(doc, "        property: |Token");

        Assert.Contains("Token", labels);
        Assert.DoesNotContain("Nom", labels);
    }

    [TestMethod]
    public async Task Completion_AliasProperty_FiltersOnSearchText()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "        property: Cli|ent", occurrence: 0);

        Assert.Contains("Client", labels);
    }

    // --- Propriétés d'une autre classe (TargetProperty) ---

    [TestMethod]
    public async Task Completion_AliasProperty_ProposesTargetClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);

        var labels = await CompleteAsync(doc, "        property: |Client", occurrence: 0);

        Assert.Contains("DateCommande", labels);
        Assert.Contains("MontantTotal", labels);
        Assert.DoesNotContain("Quantite", labels);
        Assert.DoesNotContain("Nom", labels);
    }

    [TestMethod]
    public async Task Completion_AssociationProperty_ProposesAssociatedClassProperties()
    {
        await using var doc = await OpenAsync(CommandeFile);
        doc.Change(Edit(doc, "    - association: Plat\n", "    - association: Plat\n      property: Nom\n"));

        var labels = await CompleteAsync(doc, "      property: |Nom");

        Assert.Contains("Nom", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_DataFlowActiveProperty_ProposesClassProperties()
    {
        await using var doc = await OpenAsync(FlowMenuFile);
        doc.Change(
            Edit(
                doc,
                "  type: insert\n  sources:\n    - class: Menu\n",
                "  type: insert\n  activeProperty: \n  sources:\n    - class: Menu\n"
            )
        );

        var labels = await CompleteAsync(doc, "  activeProperty: |", occurrence: 0);

        Assert.IsNotEmpty(labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_DataFlowJoinPropertiesBlockList_ProposesSourceClassProperties()
    {
        await using var doc = await OpenAsync(FlowMenuFile);
        doc.Change(
            Edit(
                doc,
                "    - class: Menu\n      mode: queryAll\n",
                "    - class: Menu\n      mode: queryAll\n      joinProperties:\n        - \n"
            )
        );

        var labels = await CompleteAsync(doc, "        - |");

        Assert.Contains("Nom", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_DataFlowJoinPropertiesInlineList_ProposesSourceClassProperties()
    {
        await using var doc = await OpenAsync(FlowMenuFile);
        doc.Change(
            Edit(
                doc,
                "    - class: Menu\n      mode: queryAll\n",
                "    - class: Menu\n      mode: queryAll\n      joinProperties: [Id, ]\n"
            )
        );

        var labels = await CompleteAsync(doc, "      joinProperties: [Id, |]");

        Assert.Contains("Nom", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_DataFlowJoinPropertiesScalar_ProposesNothing()
    {
        await using var doc = await OpenAsync(FlowMenuFile);
        doc.Change(
            Edit(
                doc,
                "    - class: Menu\n      mode: queryAll\n",
                "    - class: Menu\n      mode: queryAll\n      joinProperties: Id\n"
            )
        );

        var labels = await CompleteAsync(doc, "      joinProperties: |Id");

        Assert.IsEmpty(labels);
    }

    // --- Propriétés de la classe courante (OwnProperty) ---

    [TestMethod]
    public async Task Completion_DefaultProperty_ProposesOwnProperties()
    {
        await using var doc = await OpenAsync(TranslationFile);

        var labels = await CompleteAsync(doc, "  defaultProperty: Val|ue");

        Assert.Contains("Value", labels);

        labels = await CompleteAsync(doc, "  defaultProperty: |Value");

        Assert.Contains("ResourceKey", labels);
        Assert.Contains("Lang", labels);
        Assert.DoesNotContain("Nom", labels);
    }

    [TestMethod]
    public async Task Completion_IndexesProperties_ProposesOwnProperties()
    {
        await using var doc = await OpenAsync(CommandeFile);
        doc.Change(
            Edit(
                doc,
                "  unique:\n    - [AvisClient]\n",
                "  unique:\n    - [AvisClient]\n  indexes:\n    - properties: [DateCommande, ]\n"
            )
        );

        var labels = await CompleteAsync(doc, "    - properties: [DateCommande, |]");

        Assert.Contains("MontantTotal", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_MapperParamTarget_ProposesOwnProperties()
    {
        await using var doc = await OpenAsync(CommandeFile);
        doc.Change(
            Edit(doc, "          - class: CommandeRead\n", "          - class: CommandeRead\n            target: \n")
        );

        var labels = await CompleteAsync(doc, "            target: |");

        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Quantite", labels);
    }

    [TestMethod]
    public async Task Completion_MappingValueEmpty_ProposesMappedClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);
        doc.Change(
            Edit(
                doc,
                "    to:\n      - class: Commande\n",
                "    to:\n      - class: Commande\n        mappings:\n          Id: \n"
            )
        );

        var labels = await CompleteAsync(doc, "          Id: |");

        Assert.Contains("Id", labels);
        Assert.Contains("DateCommande", labels);
    }

    [TestMethod]
    public async Task Completion_MappingValue_ProposesMappedClassProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);
        doc.Change(
            Edit(
                doc,
                "    to:\n      - class: Commande\n",
                "    to:\n      - class: Commande\n        mappings:\n          Id: Id\n"
            )
        );

        var labels = await CompleteAsync(doc, "          Id: I|d");

        // "Id" n'existe que dans Commande (exclu de CommandeWrite) : on est bien sur la classe mappée.
        Assert.Contains("Id", labels);
    }

    [TestMethod]
    public async Task Completion_MappingsKey_ProposesOwnProperties()
    {
        await using var doc = await OpenAsync(DtoCommandeFile);
        doc.Change(
            Edit(
                doc,
                "    to:\n      - class: Commande\n",
                "    to:\n      - class: Commande\n        mappings:\n          Id: Id\n"
            )
        );

        var labels = await CompleteAsync(doc, "          |Id: Id");

        // CommandeWrite exclut "Id" : on est bien sur la classe courante et pas sur la classe mappée.
        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Id", labels);
    }

    [TestMethod]
    public async Task Completion_OnDomainAfterColonAndSpace_DoesNotAddSpace()
    {
        await using var doc = await OpenAsync(CommandeFile);
        doc.Change(doc.Text.Replace("domain: DO_ID_2", "domain: ", StringComparison.Ordinal));

        var completion = await Client.TextDocument.RequestCompletion(
            new CompletionParams { TextDocument = doc.Id, Position = doc.PositionOf("domain: |", occurrence: 0) },
            Ct
        );

        var item = completion?.FirstOrDefault(i => i.Label == "DO_ID_2");
        Assert.IsNotNull(item);
        Assert.AreEqual("DO_ID_2", item.TextEdit?.TextEdit?.NewText);
    }

    [TestMethod]
    public async Task Completion_OnDomainRightAfterColon_PrefixesInsertedTextWithSpace()
    {
        await using var doc = await OpenAsync(CommandeFile);
        doc.Change(doc.Text.Replace("domain: DO_ID_2", "domain:", StringComparison.Ordinal));

        var completion = await Client.TextDocument.RequestCompletion(
            new CompletionParams { TextDocument = doc.Id, Position = doc.PositionOf("domain:|", occurrence: 0) },
            Ct
        );

        var item = completion?.FirstOrDefault(i => i.Label == "DO_ID_2");
        Assert.IsNotNull(item);
        Assert.AreEqual(" DO_ID_2", item.TextEdit?.TextEdit?.NewText);
    }

    [TestMethod]
    public async Task Completion_OnDomain_ProposesDomains()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var completion = await Client.TextDocument.RequestCompletion(
            new CompletionParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf("domain: DO_|ID_2", occurrence: 0),
            },
            Ct
        );

        Assert.IsNotNull(completion);
        Assert.Contains(i => i.Label == "DO_ID_2", completion);
    }

    [TestMethod]
    public async Task Completion_Unique_ProposesPropertiesOfTheCurrentClass()
    {
        await using var doc = await OpenAsync(CommandeFile);

        var labels = await CompleteAsync(doc, "    - [|AvisClient]");

        Assert.Contains("DateCommande", labels);
        Assert.DoesNotContain("Quantite", labels);

        // Deuxième classe du fichier : on ne doit pas voir les propriétés de la première.
        labels = await CompleteAsync(doc, "    - [Commande, |Plat]");

        Assert.Contains("Quantite", labels);
        Assert.DoesNotContain("DateCommande", labels);
    }

    [TestMethod]
    public async Task Completion_ValuesInlineKey_ProposesOwnProperties()
    {
        await using var doc = await OpenAsync(TranslationFile);
        doc.Change(Edit(doc, "  properties:\n", "  values:\n    A: { ResourceKey: a, Val }\n  properties:\n"));

        var labels = await CompleteAsync(doc, "ResourceKey: a, Val|");

        Assert.Contains("Value", labels);
    }

    [TestMethod]
    public async Task Completion_ValuesKey_ProposesExtendedOwnProperties()
    {
        await using var doc = await OpenAsync(TranslationFile);
        doc.Change(Edit(doc, "  properties:\n", "  values:\n    A:\n      ResourceKey: a\n      \n  properties:\n"));

        var labels = await CompleteAsync(doc, "      |\n  properties:");

        Assert.Contains("Value", labels);
        Assert.Contains("Lang", labels);
        Assert.DoesNotContain("Nom", labels);
    }

    [TestMethod]
    public async Task Completion_ValuesValue_ProposesNothing()
    {
        await using var doc = await OpenAsync(TranslationFile);
        doc.Change(Edit(doc, "  properties:\n", "  values:\n    A:\n      ResourceKey: a\n  properties:\n"));

        var labels = await CompleteAsync(doc, "      ResourceKey: |a");

        Assert.IsEmpty(labels);
    }

    /// <summary>
    /// Remplace un extrait du document en normalisant ses fins de ligne sur celles du document.
    /// </summary>
    private static string Edit(OpenDocument doc, string oldText, string newText)
    {
        var eol = doc.Text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var old = oldText.Replace("\n", eol, StringComparison.Ordinal);
        Assert.Contains(old, doc.Text, $"'{oldText}' introuvable dans {doc.Path}.");
        return doc.Text.Replace(old, newText.Replace("\n", eol, StringComparison.Ordinal), StringComparison.Ordinal);
    }

    /// <summary>
    /// Demande une complétion à la position du marqueur <c>|</c> d'un extrait (fins de ligne normalisées)
    /// et retourne les labels proposés.
    /// </summary>
    private async Task<List<string>> CompleteAsync(OpenDocument doc, string snippet, int? occurrence = null)
    {
        var eol = doc.Text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var completion = await Client.TextDocument.RequestCompletion(
            new CompletionParams
            {
                TextDocument = doc.Id,
                Position = doc.PositionOf(snippet.Replace("\n", eol, StringComparison.Ordinal), occurrence),
            },
            Ct
        );

        return completion?.Select(i => i.Label).ToList() ?? [];
    }
}
