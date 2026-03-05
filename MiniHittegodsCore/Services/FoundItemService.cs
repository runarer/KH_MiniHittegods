
using System.ComponentModel.DataAnnotations;
using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsCore.Services;


public class FoundItemService(IFoundItemRepository foundItemsRepository, TimeProvider clock)
{

    private IFoundItemRepository _foundItemsRepository = foundItemsRepository;
    private TimeProvider _clock = clock;

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
        return new(FoundItemResultType.Ok, null);
    }

    public async Task<FoundItemResult> Claim(Guid id, string claimedBy)
    {
        return new(FoundItemResultType.Ok, null);
    }
    public async Task<FoundItemResult> Return(Guid id, string claimedBy)
    {
        return new(FoundItemResultType.Ok, null);
    }

    public async Task<FoundItemResult> Get(Guid id)
    {
        return new(FoundItemResultType.Ok, null);
    }

    public async Task<List<FoundItemDTO>> GetAll()
    {
        return [];
    }

}

public record FoundItemResult(FoundItemResultType Type, FoundItemDTO? FoundItemDTO);
public enum FoundItemResultType { Ok, NotFound, Conflict }