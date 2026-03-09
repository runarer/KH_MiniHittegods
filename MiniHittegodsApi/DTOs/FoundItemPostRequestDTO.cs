using System.ComponentModel.DataAnnotations;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.DTOs;

public record class FoundItemPostRequestDTO(
    [Required(ErrorMessage = "Title required")]
    [StringLength(80,ErrorMessage = "String cant be longer than 80 charaters")]
    string Title,
    string Description,
    Category Category,
    [Required(ErrorMessage = "Found location needed")]
    string FoundLocation
    );

