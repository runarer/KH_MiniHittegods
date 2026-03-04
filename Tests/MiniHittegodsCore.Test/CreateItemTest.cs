

using Microsoft.Extensions.Time.Testing;
using MiniHittegodsCore.Model;

namespace MiniHittegodsCore.Test;

public class MiniHittegodsCoreTest
{
    [Fact]
    public void AddFoundItem_CreateAnItemWithValidParameters_ItemCreatedStatusIsSetToAvailableAndTimeIsSet()
    {
        var frozenTime = DateTime.UtcNow;
        var clock = new FakeTimeProvider(frozenTime);

        var foundItemService = new FoundItemService(, clock);
        var title = "Test title";
        var description = "Test description";
        var category = Category.Other;
        var foundLocation = "Attic near crepy doll.";
        var foundItemDTO = new CreateFoundItemDTO { };

        var foundItem = foundItemService.AddFoundItem(foundItemDTO);

        Assert.NotNull(foundItem);
        Assert.NotEqual(Guid.Empty, foundItem.Id);
        Assert.Equal(Status.Available, foundItem.Status);
        Assert.Equal(frozenTime, foundItem.FoundAt);
        Assert.Equal(title, foundItem.Title);
        Assert.Equal(description, foundItem.Description);
        Assert.Equal(Category.Other, foundItem.Category);
    }

    [Fact(Skip = "Test not implemented")]
    public void CreateItem_CreateAnItemWithATitleLongerThan80Characters_ItemNotCreatedAndExceptionIsThrown()
    {

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
}