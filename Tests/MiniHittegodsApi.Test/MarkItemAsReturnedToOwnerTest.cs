

using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.Test;

public class MarkItemAsReturnedToOwnerTest(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task ReturnItem_MarkItemAsReturnedItemExcistsAndGotStatusAsClaimed_Return200WithItemAndSetStatusToReturnedAndTimeItWasClaimed()
    {
        var client = Client;
        var foundItemResponse = await CreateAnItemOnTheServer(client, foundItems[0]);
        foundItemResponse.EnsureSuccessStatusCode();
        var location = await GetLocationOfResponse(foundItemResponse);
        var claimer = "Test claimer";
        var claimedItemResponse = await client.PostAsJsonAsync(location + "/claim", new FoundItemClaimRequestDTO(claimer));
        claimedItemResponse.EnsureSuccessStatusCode();

        var returnedFoundItemResponse = await client.PostAsync(location + "/return", null);

        returnedFoundItemResponse.EnsureSuccessStatusCode();
        var returnedFoundItem = await GetFoundItemResponse(returnedFoundItemResponse);
        Assert.Equal(Status.Returned, returnedFoundItem.Status);
        Assert.NotEqual(default, returnedFoundItem.ReturnedAtUtc);

    }

    [Fact]
    public async Task ReturnItem_MarkItemAsReturnedItemDoedntExcists_Return404AndErrorMessage()
    {
        var client = Client;

        var response = await client.PostAsJsonAsync($"/api/items/{Guid.NewGuid()}/return", new FoundItemClaimRequestDTO("Test claimer"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFoundRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Item not found!", notFoundRequest);

    }

    [Fact]
    public async Task ReturnItem_MarkItemAsReturnedItemExcistsButDoNotGotStatusAsClaimed_Return409AndErrorMessage()
    {
        var client = Client;
        var foundItemResponse = await CreateAnItemOnTheServer(client, foundItems[0]);
        foundItemResponse.EnsureSuccessStatusCode();
        var location = await GetLocationOfResponse(foundItemResponse);

        var claimClaimedItemResponse = await client.PostAsync(location + "/return", null);

        Assert.Equal(HttpStatusCode.Conflict, claimClaimedItemResponse.StatusCode);
        var notFoundRequest = await claimClaimedItemResponse.Content.ReadAsStringAsync();
        Assert.Contains("Item not claimed!", notFoundRequest);
    }
}
