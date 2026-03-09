
using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;

namespace MiniHittegodsApi.Test;

public class DeleteFoundTestCustom(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task DeleteItem_DeleteItemWithIdItemExcistsAndGotStatusAsAvailable_Return204()
    {
        var client = Client;
        var createdFoundItem = await CreateAnItemOnTheServer(client, foundItems[0]);
        var createdFoundItemLocation = await GetLocationOfResponse(createdFoundItem);

        var response = await client.DeleteAsync(createdFoundItemLocation);

        response.EnsureSuccessStatusCode();

        var responseGetDeletedFoundItem = await client.GetAsync(createdFoundItemLocation);
        Assert.Equal(HttpStatusCode.NotFound, responseGetDeletedFoundItem.StatusCode);
    }

    [Fact]
    public async Task DeleteItem_DeleteItemWithIdItemDoesntExcist_Return404AndErrorMessage()
    {
        var client = Client;
        var nonExistingFoundItemLocation = $"/api/items/{Guid.NewGuid()}";

        var response = await client.DeleteAsync(nonExistingFoundItemLocation);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFoundRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Item not found!", notFoundRequest);
    }

    [Fact]
    public async Task DeleteItem_DeleteItemWithIdItemExcistsButStatusIsNotSetToAvailable_Return409()
    {
        var client = Client;
        var createdFoundItem = await CreateAnItemOnTheServer(client, foundItems[0]);
        var createdFoundItemLocation = await GetLocationOfResponse(createdFoundItem);
        var claimeItemResponse = await client.PostAsJsonAsync(
            createdFoundItemLocation + "/claim",
            new FoundItemClaimRequestDTO("Test claimer"));
        claimeItemResponse.EnsureSuccessStatusCode();

        var response = await client.DeleteAsync(createdFoundItemLocation);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var notFoundRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Item cannot be deleted!", notFoundRequest);
    }
}
