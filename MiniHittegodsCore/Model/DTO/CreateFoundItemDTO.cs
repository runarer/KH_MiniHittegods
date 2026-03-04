
namespace MiniHittegodsCore.Model.DTO;

public class CreateFoundItemDTO
{
    public required string Title { get; init; }
    public required string FoundLocation { get; init; }
    public Category Category { get; init; }
    public string Description { get; init; } = string.Empty;

}
