using MiniHittegodsApi.Interfaces;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Repository;

public class InMemoryRepository : IFoundItemRepository
{
    private readonly Dictionary<Guid, FoundItem> _storage = [];
    public async Task<IReadOnlyList<FoundItem>> GetItems(Status? status, Category? category, string? searchQuery)
    {
        var query = _storage.Select(item => item.Value);

        if (status is not null)
            query = query.Where(item => item.Status == status.Value);

        if (category is not null)
            query = query.Where(item => item.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(item => item.Title.Contains(searchQuery) || item.Description.Contains(searchQuery));

        return [.. query];
    }
    public async Task AddFoundItemAsync(FoundItem foundItem)
    {
        _storage[foundItem.Id] = foundItem;
    }

    public async Task DeleteFoundItemAsync(Guid id)
    {
        _storage.Remove(id);
    }

    public async Task<FoundItem?> GetFoundItemAsync(Guid id)
    {
        var found = _storage.TryGetValue(id, out FoundItem? item);

        if (!found)
            return null;

        return item;
    }

    public async Task Save() { }
}