using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model;

namespace MiniHittegodsCore.Repository;

public class InMemoryRepository : IFoundItemRepository
{
    private Dictionary<Guid, FoundItem> _storage = [];
    public async Task AddFoundItemAsync(FoundItem foundItem)
    {
        _storage[foundItem.Id] = foundItem;
    }

    public async Task DeleteFoundItemAsync(Guid id)
    {
        _storage.Remove(id);
    }

    public async Task<List<FoundItem>> GetAllFoundItemsAsync()
    {
        return [.. _storage.Values];
    }

    public async Task<FoundItem?> GetFoundItemAsync(Guid id)
    {
        var found = _storage.TryGetValue(id, out FoundItem? item);

        if (!found)
            return null;

        return item;
    }

    public async Task UpdateFoundItem(FoundItem foundItem)
    {
        throw new NotImplementedException();
    }
    public async Task Save() { }
}