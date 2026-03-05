namespace MiniHittegodsCore.Model.DTO;

public class FoundItemDTO
{
    public required Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; init; }
    public Category Category { get; init; }
    public string? Description { get; init; }
    public required string FoundLocation { get; init; }
    public required DateTimeOffset FoundAtUtc { get; init; }
    public required Status Status { get; set; } = Status.Available;
    public string? ClaimedBy { get; set; }
    public DateTimeOffset ClaimedAtUtc { get; set; }
    public DateTimeOffset ReturnedAtUtc { get; set; }
}