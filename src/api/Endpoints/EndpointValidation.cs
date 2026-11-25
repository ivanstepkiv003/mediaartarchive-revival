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

    public static void Required(Dictionary<string, string[]> errors, string field, string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors[field] = ["The field is required."];
        }
        else if (value.Trim().Length > maxLength)
        {
            errors[field] = [$"The field cannot exceed {maxLength} characters."];
        }
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

    public static void ImageUrl(Dictionary<string, string[]> errors, string? value)
    {
        const string field = "imageUrl";
        Optional(errors, field, value, 2_000);
        if (Clean(value) is not { } imageUrl || errors.ContainsKey(field)) return;

        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
            uri.Scheme is not ("https" or "http") ||
            string.IsNullOrEmpty(uri.Host) || !string.IsNullOrEmpty(uri.UserInfo))
        {
            errors[field] = ["Enter a direct HTTP or HTTPS image URL."];
        }
    }

    public static string[] Tags(Dictionary<string, string[]> errors, IReadOnlyCollection<string>? values)
    {
        const string field = "tags";
        if (values is null) return [];
        if (values.Count > 24)
        {
            errors[field] = ["A record cannot have more than 24 tags."];
            return [];
        }

        var tags = values
            .Select(value => value.Trim().TrimStart('#').Trim())
            .Where(value => value.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (tags.Any(value => value.Length > 80))
        {
            errors[field] = ["Each tag cannot exceed 80 characters."];
            return [];
        }

        return tags;
    }
}
