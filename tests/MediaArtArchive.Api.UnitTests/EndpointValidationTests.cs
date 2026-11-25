using MediaArtArchive.Api.Endpoints;

namespace MediaArtArchive.Api.UnitTests;

public sealed class EndpointValidationTests
{
    [Fact]
    public void Tags_are_trimmed_normalized_and_deduplicated()
    {
        var errors = new Dictionary<string, string[]>();

        var tags = EndpointValidation.Tags(errors, [" archive ", "#preservation", "ARCHIVE", "  "]);

        Assert.Empty(errors);
        Assert.Equal(["archive", "preservation"], tags);
    }

    [Fact]
    public void Tags_reject_too_many_values()
    {
        var errors = new Dictionary<string, string[]>();

        var tags = EndpointValidation.Tags(errors, Enumerable.Range(1, 25).Select(index => $"tag-{index}").ToArray());

        Assert.Empty(tags);
        Assert.Contains("tags", errors);
    }

    [Fact]
    public void Tags_reject_values_over_eighty_characters()
    {
        var errors = new Dictionary<string, string[]>();

        var tags = EndpointValidation.Tags(errors, [new string('a', 81)]);

        Assert.Empty(tags);
        Assert.Contains("tags", errors);
    }
}
