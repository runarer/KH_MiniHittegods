
using System.ComponentModel.DataAnnotations;

namespace MiniHittegodsApi.Model.DTO;

public class CreateFoundItemDTO
{
    [Required]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Title need to be between 2 and 80 characters")]
    public required string Title { get; init; }
    [Required]
    public required string FoundLocation { get; init; }
    public Category Category { get; init; }
    public string Description { get; init; } = string.Empty;

}
