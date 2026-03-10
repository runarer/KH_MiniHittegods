using System.ComponentModel.DataAnnotations;

namespace MiniHittegodsApi.DTOs;

public record class FoundItemClaimRequestDTO([Required] string ClaimedBy);