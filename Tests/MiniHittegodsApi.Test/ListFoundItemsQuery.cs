

using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;

namespace MiniHittegodsApi.Test;

public class ListFoundItemsQuery(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task ListItems_ListItemsUsingAValidSearch_Returns200WithListOfAllItemsMatchingSearch()
    {
        var client = Client;
        var searchText = "-SearchTest-";
        var foundItemsResponse = await CreateSeveralFoundItemsOnServer(client, foundItems);
        var numberOfFoundItemsWithCategoryOtherOnServer = foundItems.Count(item => item.Title.Contains(searchText) || item.Description.Contains(searchText));

        var listOfAllFoundItemsResponse = await client.GetAsync($"/api/items?q={searchText}");

        listOfAllFoundItemsResponse.EnsureSuccessStatusCode();
        var listOfAllFoundItems = await listOfAllFoundItemsResponse.Content.ReadFromJsonAsync<List<FoundItemResponseDTO>>();
        Assert.NotNull(listOfAllFoundItems);
        Assert.Equal(numberOfFoundItemsWithCategoryOtherOnServer, listOfAllFoundItems.Count);
        Assert.True(listOfAllFoundItems.All(item => item.Title.Contains(searchText) || item.Description.Contains(searchText)));
    }
}
