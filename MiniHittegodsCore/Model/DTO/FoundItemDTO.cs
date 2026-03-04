namespace MiniHittegodsCore.Model.DTO;

public class FoundItemDTO
{
    public required Guid Id;
    public required string Title { get; init; }
    public required string FoundLocation { get; init; }
    public required DateTime FoundAtUtc { get; init; }
    public required Status Status { get; set; }
    public Category Category { get; init; }
    public string Description { get; init; } = string.Empty;

}