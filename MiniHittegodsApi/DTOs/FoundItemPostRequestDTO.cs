using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.DTOs;

public record class FoundItemPostRequestDTO(string Title, string Description, Category Category, string FoundLocation);

