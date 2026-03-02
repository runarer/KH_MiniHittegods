
using System.ComponentModel.DataAnnotations;

namespace MiniHittegodsCore.Model;

public class Item
{
    [Key]
    public required Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 80 characters")]
    public required string Title { get; init; }
    public Category Category { get; init; }
    public string? Description { get; init; }
    public required string FoundLocation { get; init; }

    public Status Status { get; private set; } = Status.Available;

    public string? ClaimedBy { get; private set; }

    public DateTime ClaimedAtUtc { get; private set; }
    public DateTime ReturnedAtUtc { get; private set; }

}
