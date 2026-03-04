
using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsCore.Services;


public class FoundItemService(IFoundItemRepository foundItemsRepository, TimeProvider clock)
{

    public async Task<FoundItemDTO> AddFoundItem(CreateFoundItemDTO foundItemCTO)
    {
        return new FoundItemDTO
        {
            Title = "Red",
            FoundLocation = "Red",
            Id = Guid.Empty,
            FoundAtUtc = DateTime.UtcNow,
            Status = Model.Status.Available,
        };
    }

}
