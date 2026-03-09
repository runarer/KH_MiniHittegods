using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.DTOs;

public record class FoundItemResponseDTO(
    Guid Id,
    string Title,
    string Description,
    Category Category,
    string FoundLocation,
    DateTimeOffset FoundAtUtc,
    Status Status,
    string? ClaimedBy,
    DateTimeOffset ClaimedAtUtc,
    DateTimeOffset ReturnedAtUtc
);
