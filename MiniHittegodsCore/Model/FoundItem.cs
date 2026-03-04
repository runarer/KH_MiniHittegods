
using System.ComponentModel.DataAnnotations;

namespace MiniHittegodsCore.Model;

public class FoundItem
{
    [Key]
    public required Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 80 characters")]
    public required string Title { get; init; }
    public Category Category { get; init; }
    public string? Description { get; init; }

    [Required(ErrorMessage = "Item need a location of where it was found.")]
    public required string FoundLocation { get; init; }

    public DateTime FoundAtUtc { get; init; }


    public Status Status { get; set; } = Status.Available;

    public string? ClaimedBy { get; set; }

    public DateTime ClaimedAtUtc { get; set; }
    public DateTime ReturnedAtUtc { get; set; }

}
