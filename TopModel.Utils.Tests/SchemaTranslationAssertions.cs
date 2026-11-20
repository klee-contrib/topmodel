using System.Text.Json.Nodes;

namespace TopModel.Utils.Tests;

public static class SchemaTranslationAssertions
{
    public static void AssertEquivalent(string defaultFileName, string localizedFileName)
    {
        var defaultSchema = NormalizeDescriptions(ReadSchema(defaultFileName));
        var localizedSchema = NormalizeDescriptions(ReadSchema(localizedFileName));

        Assert.IsTrue(
            JsonNode.DeepEquals(defaultSchema, localizedSchema),
            $"Les schémas {defaultFileName} et {localizedFileName} diffèrent en dehors de leurs descriptions."
        );
    }

    private static JsonNode NormalizeDescriptions(JsonNode schema)
    {
        if (schema is JsonObject schemaObject)
        {
            foreach (var property in schemaObject.ToList())
            {
                if (property.Key == "description" && property.Value is JsonValue)
                {
                    schemaObject[property.Key] = "localized";
                }
                else if (property.Value != null)
                {
                    NormalizeDescriptions(property.Value);
                }
            }
        }
        else if (schema is JsonArray schemaArray)
        {
            foreach (var item in schemaArray)
            {
                if (item != null)
                {
                    NormalizeDescriptions(item);
                }
            }
        }

        return schema;
    }

    private static JsonNode ReadSchema(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, fileName);
        return JsonNode.Parse(File.ReadAllText(path))!;
    }
}
