using Json.Schema;
using System.Text.Json.Nodes;

namespace Application.Parametros.Validation;

public static class JsonSchemaValidator
{
    public static (bool IsValid, List<string> Errors) Validate(string json, string schemaJson)
    {
        var errors = new List<string>();

        try
        {
            var schema = JsonSchema.FromText(schemaJson);
            var instance = JsonNode.Parse(json);
            var results = schema.Evaluate(instance, new EvaluationOptions
            {
                OutputFormat = OutputFormat.List
            });

            if (!results.IsValid)
            {
                CollectErrors(results, errors);
            }

            return (results.IsValid, errors);
        }
        catch (Exception ex)
        {
            errors.Add($"Error validando JSON: {ex.Message}");
            return (false, errors);
        }
    }

    private static void CollectErrors(EvaluationResults results, List<string> errors)
    {
        if (results.Errors != null)
        {
            foreach (var error in results.Errors)
            {
                errors.Add($"{error.Key}: {error.Value}");
            }
        }

        foreach (var detail in results.Details)
        {
            CollectErrors(detail, errors);
        }
    }
}