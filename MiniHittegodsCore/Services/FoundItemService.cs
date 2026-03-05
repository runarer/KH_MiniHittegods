
using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsCore.Services;


public class FoundItemService(IFoundItemRepository foundItemsRepository, TimeProvider clock)
{

    private IFoundItemRepository _foundItemsRepository = foundItemsRepository;
    private TimeProvider _clock = clock;

    public async Task<FoundItemDTO> AddFoundItem(CreateFoundItemDTO foundItemDTO)
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




        return new FoundItemDTO
        {
            Id = foundItem.Id,
            Title = foundItem.Title,
            FoundLocation = foundItem.FoundLocation,
            FoundAtUtc = foundItem.FoundAtUtc,
            Status = foundItem.Status,
            Description = foundItem.Description,
            Category = foundItem.Category,
        };
    }



}
