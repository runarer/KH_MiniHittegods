
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Interfaces;

public interface IFoundItemRepository
{
    Task AddFoundItemAsync(FoundItem foundItem);

    Task DeleteFoundItemAsync(Guid id);

    Task<FoundItem?> GetFoundItemAsync(Guid id);
    Task<IReadOnlyList<FoundItem>> GetItems(Status? status, Category? category, string? searchQuery);

    Task Save();
}
