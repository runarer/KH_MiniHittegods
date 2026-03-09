
using MiniHittegodsApi.Model;
using MiniHittegodsApi.Model.DTO;
using MiniHittegodsApi.Services;

namespace MiniHittegodsApi.Interfaces;

public interface IFoundItemService
{
    public Task<FoundItemResult> Add(CreateFoundItemDTO foundItemDTO);
    public Task<FoundItemResult> Delete(Guid id);
    public Task<FoundItemResult> Claim(Guid id, string claimedBy);
    public Task<FoundItemResult> Return(Guid id);
    public Task<FoundItemResult> Get(Guid id);
    public Task<List<FoundItemDTO>> GetAll(Status? status, Category? category, string? searchQuery);
}
