using System.Text.Json.Serialization;

namespace MiniHittegodsApi.Model.DTO;

public class FoundItemDTO
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Category Category { get; init; }
    public string? Description { get; init; }
    public required string FoundLocation { get; init; }
    public required DateTimeOffset FoundAtUtc { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required Status Status { get; set; }
    public string? ClaimedBy { get; set; }
    public DateTimeOffset ClaimedAtUtc { get; set; }
    public DateTimeOffset ReturnedAtUtc { get; set; }
}