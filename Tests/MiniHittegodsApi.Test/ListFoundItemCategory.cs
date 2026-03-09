
using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Test;

public class ListFoundItemsOfACategorys(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

    [Fact]
    public async Task ListItems_ListItemsWithValidCategory_Returns200WithListOfAllItemsWithGivenCategory()
    {
        var client = Client;
        var foundItemsResponse = await CreateSeveralFoundItemsOnServer(client, foundItems);
        var numberOfFoundItemsWithCategoryOtherOnServer = 4;// foundItems.Count(item => item.Category == Category.Other);

        var listOfAllFoundItemsResponse = await client.GetAsync("/api/items?Category=Other");

        listOfAllFoundItemsResponse.EnsureSuccessStatusCode();
        var listOfAllFoundItems = await listOfAllFoundItemsResponse.Content.ReadFromJsonAsync<List<FoundItemResponseDTO>>();
        Assert.NotNull(listOfAllFoundItems);
        Assert.Equal(numberOfFoundItemsWithCategoryOtherOnServer, listOfAllFoundItems.Count);
        Assert.True(listOfAllFoundItems.All(item => item.Category == Category.Other));
    }
    [Fact]
    public async Task ListItems_ListItemsWithANonExcistingCategory_Returns400()
    {
        var client = Client;
        var fakeCategory = "NotACategory";

        var response = await client.GetAsync($"/api/items?status={fakeCategory}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
