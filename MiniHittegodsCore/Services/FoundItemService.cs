
using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsCore.Services;


public class FoundItemService(IFoundItemRepository foundItemsRepository, TimeProvider clock) : IFoundItemService
{

    private readonly IFoundItemRepository _foundItemsRepository = foundItemsRepository;
    private readonly TimeProvider _clock = clock;

    public async Task<FoundItemResult> Add(CreateFoundItemDTO foundItemDTO)
    {

        var foundItem = new FoundItem
        {
            Id = Guid.NewGuid(),
            Title = foundItemDTO.Title,
            FoundLocation = foundItemDTO.FoundLocation,
            Status = Status.Available,
            FoundAtUtc = _clock.GetUtcNow(),
            Description = foundItemDTO.Description,
            Category = foundItemDTO.Category,
        };

        await _foundItemsRepository.AddFoundItemAsync(foundItem);

        return new FoundItemResult(FoundItemResultType.Ok, new FoundItemDTO
        {
            Id = foundItem.Id,
            Title = foundItem.Title,
            FoundLocation = foundItem.FoundLocation,
            FoundAtUtc = foundItem.FoundAtUtc,
            Status = foundItem.Status,
            Description = foundItem.Description,
            Category = foundItem.Category,
        });
    }

    public async Task<FoundItemResult> Delete(Guid id)
    {
        var toDelete = await _foundItemsRepository.GetFoundItemAsync(id);
        if (toDelete is null)
            return new(FoundItemResultType.NotFound, null);

        if (toDelete.Status != Status.Available)
            return new(FoundItemResultType.Conflict, null);

        await _foundItemsRepository.DeleteFoundItemAsync(id);
        return new(FoundItemResultType.Ok, null);
    }

    public async Task<FoundItemResult> Claim(Guid id, string claimedBy)
    {
        var toClaim = await _foundItemsRepository.GetFoundItemAsync(id);

        if (toClaim is null)
            return new(FoundItemResultType.NotFound, null);

        if (toClaim.Status != Status.Available)
            return new(FoundItemResultType.Conflict, null);

        toClaim.Status = Status.Claimed;
        toClaim.ClaimedBy = claimedBy;
        toClaim.ClaimedAtUtc = _clock.GetUtcNow();

        await _foundItemsRepository.Save();

        return new(FoundItemResultType.Ok, FromModelToDTO(toClaim));
    }
    public async Task<FoundItemResult> Return(Guid id)
    {
        var toReturn = await _foundItemsRepository.GetFoundItemAsync(id);

        if (toReturn is null)
            return new(FoundItemResultType.NotFound, null);

        if (toReturn.Status != Status.Claimed)
            return new(FoundItemResultType.Conflict, null);

        toReturn.Status = Status.Returned;
        toReturn.ReturnedAtUtc = _clock.GetUtcNow();

        await _foundItemsRepository.Save();

        return new(FoundItemResultType.Ok, FromModelToDTO(toReturn));
    }

    public async Task<FoundItemResult> Get(Guid id)
    {
        var toGet = await _foundItemsRepository.GetFoundItemAsync(id);

        if (toGet is null)
            return new(FoundItemResultType.NotFound, null);

        return new(FoundItemResultType.Ok, FromModelToDTO(toGet));
    }

    public async Task<List<FoundItemDTO>> GetAll(Status? status, Category? category, string? searchQuery)
    {
        var all = await _foundItemsRepository.GetItems(status, category, searchQuery);

        return [.. all.Select(FromModelToDTO)];
    }

    private FoundItemDTO FromModelToDTO(FoundItem item) => new()
    {
        Id = item.Id,
        Title = item.Title,
        Category = item.Category,
        Description = item.Description,
        FoundLocation = item.FoundLocation,
        FoundAtUtc = item.FoundAtUtc,
        Status = item.Status,
        ClaimedBy = item.ClaimedBy,
        ClaimedAtUtc = item.ClaimedAtUtc,
        ReturnedAtUtc = item.ReturnedAtUtc,
    };
}

public record FoundItemResult(FoundItemResultType Type, FoundItemDTO? FoundItemDTO);
public enum FoundItemResultType { Ok, NotFound, Conflict }
