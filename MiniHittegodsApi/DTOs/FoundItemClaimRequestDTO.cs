using System.ComponentModel.DataAnnotations;

namespace MiniHittegodsApi.DTOs;

// public record class FoundItemClaimRequestDTO([Required] string ClaimedBy);
public class FoundItemClaimRequestDTO
{ [Required] public required string ClaimedBy { get; set; } };