using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.DTOs;

public class FoundItemResponseDTO
{
    public Guid Id { get; set; }

    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Category Category { get; set; }
    public required string FoundLocation { get; set; }
    public DateTimeOffset FoundAtUtc { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; set; }
    public string? ClaimedBy { get; set; }
    public DateTimeOffset ClaimedAtUtc { get; set; }
    public DateTimeOffset ReturnedAtUtc { get; set; }
};
