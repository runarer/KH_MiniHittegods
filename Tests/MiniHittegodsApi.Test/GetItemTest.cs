

using System.Net;
using System.Net.Http.Json;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsApi.Test;

public class GetItemTest(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task GetItem_GetItemWithSpecifiedId_Return200AndItemObject()
    {
        var client = Client;

        var addedFoundItem = await CreateAnItemOnTheServer(client, foundItems[0]);
        var addedFoundItemLocation = await GetLocationOfResponse(addedFoundItem);


        var response = await client.GetAsync(addedFoundItemLocation);


        response.EnsureSuccessStatusCode();
        var foundItem = await response.Content.ReadFromJsonAsync<FoundItemDTO>();
        Assert.NotNull(foundItem);
        Assert.Equal(foundItems[0].Title, foundItem.Title);
        Assert.Equal(foundItems[0].Description, foundItem.Description);
        Assert.Equal(foundItems[0].Category, foundItem.Category);
        Assert.Equal(foundItems[0].FoundLocation, foundItem.FoundLocation);
    }
    [Fact]
    public async Task GetItem_GetItemWithSpecifiedIdWhenIdDoesntExcist_Return404AndErrorMessage()
    {
        var client = Client;

        var response = await client.GetAsync($"/api/items/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFoundRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Item not found!", notFoundRequest);
    }
}
