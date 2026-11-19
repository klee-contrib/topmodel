using System.Text.Json;

namespace TopModel.LanguageServer.Tests;

[TestClass]
public class SchemaTranslationTests
{
    [TestMethod]
    public void SchemaFr_HasSameKeysAsSchemaEn()
    {
        var en = GetKeys("schema.json");
        var fr = GetKeys("schema.fr.json");

        var missingInFr = en.Except(fr).Order(StringComparer.Ordinal).ToList();
        var missingInEn = fr.Except(en).Order(StringComparer.Ordinal).ToList();

        Assert.IsEmpty(
            missingInFr,
            $"Clés présentes dans schema.json mais absentes de schema.fr.json :\n{string.Join('\n', missingInFr)}"
        );
        Assert.IsEmpty(
            missingInEn,
            $"Clés présentes dans schema.fr.json mais absentes de schema.json :\n{string.Join('\n', missingInEn)}"
        );
    }

    private static void Collect(JsonElement element, string path, HashSet<string> keys)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in element.EnumerateObject())
                {
                    var propPath = $"{path}/{prop.Name}";
                    keys.Add(propPath);
                    Collect(prop.Value, propPath, keys);
                }

                break;
            case JsonValueKind.Array:
                var i = 0;
                foreach (var item in element.EnumerateArray())
                {
                    Collect(item, $"{path}[{i++}]", keys);
                }

                break;
        }
    }

    private static HashSet<string> GetKeys(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, fileName);
        using var doc = JsonDocument.Parse(
            File.ReadAllText(path),
            new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip }
        );
        var keys = new HashSet<string>();
        Collect(doc.RootElement, string.Empty, keys);
        return keys;
    }
}
