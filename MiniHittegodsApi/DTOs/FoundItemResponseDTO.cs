using MiniHittegodsCore.Model;

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


/*

    public DateTimeOffset FoundAtUtc { get; init; }


    public Status Status { get; set; } = Status.Available;

    public string? ClaimedBy { get; set; }

    public DateTimeOffset ClaimedAtUtc { get; set; }
    public DateTimeOffset ReturnedAtUtc { get; set; }

*/