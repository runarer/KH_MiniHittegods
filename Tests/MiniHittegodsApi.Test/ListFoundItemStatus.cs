
using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.Test;

public class ListFoundItemStatus(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{

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
    }
}

