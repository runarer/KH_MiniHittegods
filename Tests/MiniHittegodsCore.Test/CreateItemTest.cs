

using Microsoft.Extensions.Time.Testing;
using MiniHittegodsCore.Interfaces;
using MiniHittegodsCore.Model;
using MiniHittegodsCore.Model.DTO;
using MiniHittegodsCore.Services;

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
        var foundItem = await foundItemService.AddFoundItem(foundItemDTO);

        Assert.NotNull(foundItem);
        Assert.NotEqual(Guid.Empty, foundItem.Id);
        Assert.Equal(Status.Available, foundItem.Status);
        Assert.Equal(frozenTime, foundItem.FoundAtUtc);
        Assert.Equal(title, foundItem.Title);
        Assert.Equal(description, foundItem.Description);
        Assert.Equal(Category.Other, foundItem.Category);
    }

    [Fact]
    public async Task CreateItem_CreateAnItemWithATitleLongerThan80Characters_ItemNotCreatedAndExceptionIsThrown()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);
        var repository = new InMemoryRepository();

        var foundItemService = new FoundItemService(repository, clock);
        var title = "Test title that is more than 80 characters long, this should not be accepted by the constructor!";
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

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => foundItemService.AddFoundItem(foundItemDTO));
    }

    [Fact(Skip = "Test not implemented")]
    public void ClaimItem_ClaimAnItemWithStatusAvailable_ItemStatusIsChangedToClaimed()
    {

    }
    [Fact(Skip = "Test not implemented")]
    public void ClaimItem_ClaimAnItemWithStatusClaim_ItemUnchangedAndThrowException()
    {

    }
    [Fact(Skip = "Test not implemented")]
    public void ClaimItem_ClaimAnItemWithStatusReturned_ItemUnchangedAndThrowException()
    {

    }

    [Fact(Skip = "Test not implemented")]
    public void ReturnItem_ReturnAnItemWithStatusClaimed_ItemStatusIsSetToReturnedAndReturnTimeIsSetToNowUTC()
    {

    }

    [Fact(Skip = "Test not implemented")]
    public void ReturnItem_ReturnAnItemWithStatusAvailable_ItemUnchangedAndThrowsException()
    {

    }
    [Fact(Skip = "Test not implemented")]
    public void ReturnItem_ReturnAnItemWithStatusReturned_ItemUnchangedAndThrowsException()
    {

    }

    [Fact(Skip = "Test not implemented")]
    public void DeleteItem_DeleteAnItemWithStatusAvailable_ItemDeleted()
    {

    }
    [Fact(Skip = "Test not implemented")]
    public void DeleteItem_DeleteAnItemWithStatusClaim_ItemUnchangedAndThrowException()
    {

    }
    [Fact(Skip = "Test not implemented")]
    public void DeleteItem_DeleteAnItemWithStatusReturned_ItemUnchangedAndThrowException()
    {

    }

    private class InMemoryRepository : IFoundItemRepository
    {
        public Task AddFoundItemAsync(FoundItem foundItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFoundItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<FoundItem>> GetAllFoundItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FoundItem> GetFoundItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFoundItem(FoundItem foundItem)
        {
            throw new NotImplementedException();
        }
    }
}