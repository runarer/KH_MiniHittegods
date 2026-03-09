

using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.Test;

public class ListFoundItemsTests(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{


    [Fact]
    public async Task ListItems_ListAllItems_Returns200WithListOfAllItems()
    {
        var client = Client;
        var foundItemsResponse = await CreateSeveralFoundItemsOnServer(client, foundItems);
        var listOfTitlesForItemsAddedToServer = foundItems.Select(item => item.Title);

        var listOfAllFoundItemsResponse = await client.GetAsync("/api/items");

        listOfAllFoundItemsResponse.EnsureSuccessStatusCode();
        var listOfAllFoundItems = await listOfAllFoundItemsResponse.Content.ReadFromJsonAsync<List<FoundItemResponseDTO>>();
        Assert.NotNull(listOfAllFoundItems);
        Assert.Equal(foundItemsResponse.Count, listOfAllFoundItems.Count);
        var listOfTitlesFromServer = listOfAllFoundItems.Select(item => item.Title);
        Assert.True(listOfTitlesFromServer.All(listOfTitlesForItemsAddedToServer.Contains));
    }

    [Fact]
    public async Task ListItems_ListItemsWithValidStatus_Returns200WithListOfAllItemsWithGivenStatus()
    {
        var client = Client;
        var foundItemsResponse = await CreateSeveralFoundItemsOnServer(client, foundItems);
        var firstClaimLocation = await GetLocationOfResponse(foundItemsResponse[0]);
        var secondClaimLocation = await GetLocationOfResponse(foundItemsResponse[1]);
        var thirdClaimLocation = await GetLocationOfResponse(foundItemsResponse[2]);
        var firstClaimedItemResponse = await client.PostAsJsonAsync(firstClaimLocation + "/claim", new FoundItemClaimRequestDTO("First claimer"));
        firstClaimedItemResponse.EnsureSuccessStatusCode();
        var secondClaimedItemResponse = await client.PostAsJsonAsync(secondClaimLocation + "/claim", new FoundItemClaimRequestDTO("Second claimer"));
        secondClaimedItemResponse.EnsureSuccessStatusCode();
        var thirdClaimedItemResponse = await client.PostAsJsonAsync(thirdClaimLocation + "/claim", new FoundItemClaimRequestDTO("Third claimer"));
        thirdClaimedItemResponse.EnsureSuccessStatusCode();

        var listOfAllFoundItemsResponse = await client.GetAsync("/api/items?status=Claimed");

        listOfAllFoundItemsResponse.EnsureSuccessStatusCode();
        var listOfAllFoundItems = await listOfAllFoundItemsResponse.Content.ReadFromJsonAsync<List<FoundItemResponseDTO>>();
        Assert.NotNull(listOfAllFoundItems);
        Assert.Equal(3, listOfAllFoundItems.Count);
        Assert.True(listOfAllFoundItems.All(item => item.Status == Status.Claimed));

        var listOfTitlesFromServer = listOfAllFoundItems.Select(item => item.Title);
        Assert.Contains(foundItems[0].Title, listOfTitlesFromServer);
        Assert.Contains(foundItems[1].Title, listOfTitlesFromServer);
        Assert.Contains(foundItems[2].Title, listOfTitlesFromServer);
    }

    [Fact]
    public async Task ListItems_ListItemsWithANonExcistingStatus_Returns400()
    {
        var client = Client;
        var fakeStatus = "Eaten";

        var response = await client.GetAsync($"/api/items?status={fakeStatus}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // var unprocessableEntitydRequest = await response.Content.ReadAsStringAsync();
        // Assert.Contains($"Category {fakeStatus} is not an accepted status.", unprocessableEntitydRequest);

    }

    [Fact]
    public async Task ListItems_ListItemsWithValidCategory_Returns200WithListOfAllItemsWithGivenCategory()
    {
        var client = Client;
        var foundItemsResponse = await CreateSeveralFoundItemsOnServer(client, foundItems);
        var numberOfFoundItemsWithCategoryOtherOnServer = foundItems.Count(item => item.Category == Category.Other);

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
        // var unprocessableEntitydRequest = await response.Content.ReadAsStringAsync();
        // Assert.Contains($"Category {fakeCategory} is not an accepted category.", unprocessableEntitydRequest);
    }

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
