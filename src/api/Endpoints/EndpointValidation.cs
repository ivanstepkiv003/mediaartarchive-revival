namespace MediaArtArchive.Api.Endpoints;

internal static class EndpointValidation
{
    public static Dictionary<string, string[]> Required(string field, string? value, int maxLength)
    {
        var errors = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(value))
        {
            errors[field] = ["The field is required."];
        }
        else if (value.Trim().Length > maxLength)
        {
            errors[field] = [$"The field cannot exceed {maxLength} characters."];
        }

        return errors;
    }

    public static void Optional(Dictionary<string, string[]> errors, string field, string? value, int maxLength)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Trim().Length > maxLength)
        {
            errors[field] = [$"The field cannot exceed {maxLength} characters."];
        }
    }

    public static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
