
using Microsoft.Extensions.Time.Testing;
using MiniHittegodsApi.Interfaces;
using MiniHittegodsApi.Model;
using MiniHittegodsApi.Model.DTO;
using MiniHittegodsApi.Services;

namespace MiniHittegodsCore.Test;

public class MiniHittegodsCoreTest
{
    [Fact]
    public async Task AddFoundItem_CreateAnItemWithValidParameters_ItemCreatedStatusIsSetToAvailableAndTimeIsSet()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();

        var foundItemService = new FoundItemService(repository, clock);
        var title = "Test title";
        var description = "Test description";
        var category = Category.Other;
        var foundLocation = "Attic near crepy doll.";
        var foundItemDTO = new CreateFoundItemDTO
        {
            Title = title,
            Category = category,
            Description = description,
            FoundLocation = foundLocation
        };


        // Action
        var (result, foundItem) = await foundItemService.Add(foundItemDTO);

        // Asserts
        Assert.Equal(FoundItemResultType.Ok, result);
        Assert.NotNull(foundItem);

        // Assert item was created.
        var (getResult, getItem) = await foundItemService.Get(foundItem.Id);
        Assert.Equal(FoundItemResultType.Ok, getResult);
        Assert.NotNull(getItem);

        Assert.NotEqual(Guid.Empty, foundItem.Id);
        Assert.Equal(Status.Available, foundItem.Status);
        Assert.Equal(frozenTime, foundItem.FoundAtUtc);
        Assert.Equal(title, foundItem.Title);
        Assert.Equal(description, foundItem.Description);
        Assert.Equal(Category.Other, foundItem.Category);
    }

    [Fact]
    public async Task ClaimItem_ClaimAnItemWithStatusAvailable_ItemStatusIsChangedToClaimed()
    {
        var frozenTime = DateTime.UtcNow;
        var claimedBy = "Test Claimer";
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);

        Assert.Equal(Status.Available, foundItem.Status);


        var (result, claimedItemDTO) = await foundItemService.Claim(foundItem.Id, claimedBy);

        Assert.Equal(FoundItemResultType.Ok, result);
        Assert.NotNull(claimedItemDTO);
        Assert.Equal(Status.Claimed, claimedItemDTO.Status);
        Assert.Equal(frozenTime, claimedItemDTO.ClaimedAtUtc);
        Assert.Equal(claimedBy, claimedItemDTO.ClaimedBy);
        Assert.Equal(foundItem.Id, claimedItemDTO.Id);
    }
    [Fact]
    public async Task ClaimItem_ClaimAnItemWithStatusClaim_ItemUnchangedAndResultConflictIsReturned()
    {
        var frozenTime = DateTime.UtcNow;
        var claimedBy = "Second Claimer";
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        var (firstResult, firstClaimedItem) = await foundItemService.Claim(foundItem.Id, "First Claimer");

        Assert.Equal(FoundItemResultType.Ok, firstResult);
        Assert.NotNull(firstClaimedItem);
        Assert.Equal(Status.Claimed, firstClaimedItem.Status);

        var (result, claimedItemDTO) = await foundItemService.Claim(foundItem.Id, claimedBy);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(claimedItemDTO);
    }
    [Fact]
    public async Task ClaimItem_ClaimAnItemWithStatusReturned_ItemUnchangedAndNullReturned()
    {
        var frozenTime = DateTime.UtcNow;
        var claimedBy = "Second Claimer";
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        _ = foundItemService.Claim(foundItem.Id, "First Claimer");
        var (firstReturnResult, firstReturnItem) = await foundItemService.Return(foundItem.Id);

        Assert.Equal(FoundItemResultType.Ok, firstReturnResult);
        Assert.NotNull(firstReturnItem);
        Assert.Equal(Status.Returned, firstReturnItem.Status);

        var (result, claimedItemDTO) = await foundItemService.Claim(foundItem.Id, claimedBy);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(claimedItemDTO);
    }

    [Fact]
    public async Task ReturnItem_ReturnAnItemWithStatusClaimed_ItemStatusIsSetToReturnedAndReturnTimeIsSetToNowUTC()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        var (firstResult, firstClaimedItem) = await foundItemService.Claim(foundItem.Id, "First Claimer");

        Assert.Equal(FoundItemResultType.Ok, firstResult);
        Assert.NotNull(firstClaimedItem);
        Assert.Equal(Status.Claimed, firstClaimedItem.Status);

        var (result, returnedItem) = await foundItemService.Return(foundItem.Id);

        Assert.Equal(FoundItemResultType.Ok, result);
        Assert.NotNull(returnedItem);
        Assert.Equal(foundItem.Id, returnedItem.Id);
        Assert.Equal(Status.Returned, returnedItem.Status);
        Assert.Equal(frozenTime, returnedItem.ReturnedAtUtc);
    }

    [Fact]
    public async Task ReturnItem_ReturnAnItemWithStatusAvailable_ItemUnchangedAnd()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);

        Assert.Equal(Status.Available, foundItem.Status);

        var (result, returnedItem) = await foundItemService.Return(foundItem.Id);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(returnedItem);
    }
    [Fact]
    public async Task ReturnItem_ReturnAnItemWithStatusReturned_ItemUnchangedAndConflictResultReturned()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        _ = foundItemService.Claim(foundItem.Id, "First Claimer");
        var (firstReturnResult, firstReturnItem) = await foundItemService.Return(foundItem.Id);

        Assert.Equal(FoundItemResultType.Ok, firstReturnResult);
        Assert.NotNull(firstReturnItem);
        Assert.Equal(Status.Returned, firstReturnItem.Status);

        var (result, returnedItem) = await foundItemService.Return(foundItem.Id);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(returnedItem);
    }

    [Fact]
    public async Task DeleteItem_DeleteAnItemWithStatusAvailable_ItemDeleted()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);

        // Assert item was created.
        var (getResult, getItem) = await foundItemService.Get(foundItem.Id);
        Assert.Equal(FoundItemResultType.Ok, getResult);
        Assert.NotNull(getItem);

        var (result, deletedItem) = await foundItemService.Delete(foundItem.Id);

        Assert.Equal(FoundItemResultType.Ok, result);
        Assert.Null(deletedItem);
    }


    [Fact]
    public async Task DeleteItem_DeleteAnItemWithStatusClaim_ItemUnchangedAndConflictResultReturned()
    {
        var frozenTime = DateTime.UtcNow;
        var claimedBy = "Test Claimer";
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        _ = await foundItemService.Claim(foundItem.Id, claimedBy);

        var (result, deletedItem) = await foundItemService.Delete(foundItem.Id);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(deletedItem);
    }
    [Fact]
    public async Task DeleteItem_DeleteAnItemWithStatusReturned_ItemUnchangedAndConflictResultReturned()
    {
        var frozenTime = DateTime.UtcNow;
        var claimedBy = "Test Claimer";
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();
        var foundItemService = new FoundItemService(repository, clock);
        var foundItem = await AddAFoundItem(foundItemService);
        _ = await foundItemService.Claim(foundItem.Id, claimedBy);
        _ = await foundItemService.Return(foundItem.Id);

        var (result, deletedItem) = await foundItemService.Delete(foundItem.Id);

        Assert.Equal(FoundItemResultType.Conflict, result);
        Assert.Null(deletedItem);
    }


    private async Task<FoundItemDTO> AddAFoundItem(FoundItemService foundItemService)
    {
        var title = "Test title";
        var description = "Test description";
        var category = Category.Other;
        var foundLocation = "Attic near crepy doll.";
        var foundItemDTO = new CreateFoundItemDTO
        {
            Title = title,
            Category = category,
            Description = description,
            FoundLocation = foundLocation
        };
        var (result, foundItem) = await foundItemService.Add(foundItemDTO);

        Assert.Equal(FoundItemResultType.Ok, result);
        Assert.NotNull(foundItem);
        return foundItem;
    }
    private class InMemoryRepository : IFoundItemRepository
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
            return _storage[id];
        }

        public async Task UpdateFoundItem(FoundItem foundItem)
        {
            throw new NotImplementedException();
        }
        public async Task Save() { }

        public async Task<IReadOnlyList<FoundItem>> GetItems(Status? status, Category? category, string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}