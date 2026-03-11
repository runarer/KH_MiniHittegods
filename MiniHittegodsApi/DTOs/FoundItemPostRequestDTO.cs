using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.DTOs;

// public record class FoundItemPostRequestDTO(
//     [Required(ErrorMessage = "Title required")]
//     [StringLength(80,ErrorMessage = "String cant be longer than 80 charaters")]
//     string Title,
//     string Description,
//     Category Category,
//     [Required(ErrorMessage = "Found location needed")]
//     string FoundLocation
//     );


public class FoundItemPostRequestDTO
{
    [Required(ErrorMessage = "Title required")]
    [StringLength(80, ErrorMessage = "String cant be longer than 80 charaters")]
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Category Category { get; set; }

    [Required(ErrorMessage = "Found location needed")]
    public required string FoundLocation { get; set; }
}
