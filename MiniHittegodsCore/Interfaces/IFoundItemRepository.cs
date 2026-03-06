
using MiniHittegodsCore.Model;

namespace MiniHittegodsCore.Interfaces;

public interface IFoundItemRepository
{
    Task AddFoundItemAsync(FoundItem foundItem);

    Task DeleteFoundItemAsync(Guid id);

    Task<FoundItem?> GetFoundItemAsync(Guid id);
    Task<List<FoundItem>> GetAllFoundItemsAsync();

    Task UpdateFoundItem(FoundItem foundItem);

    Task Save();
}
