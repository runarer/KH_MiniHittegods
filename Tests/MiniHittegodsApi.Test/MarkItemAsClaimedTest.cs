

using System.Net;
using System.Net.Http.Json;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.Test;

public class MarkItemAsClaimedTest(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task ClaimItem_MarkItemAsClaimedItemIsAvailableAndClaimedByGotAValue_Return200WithUpdatedItemAndStatusSetToClaimed()
    {
        var client = Client;
        var foundItemResponse = await CreateAnItemOnTheServer(client, foundItems[0]);
        foundItemResponse.EnsureSuccessStatusCode();
        var location = await GetLocationOfResponse(foundItemResponse);
        var claimer = "Test claimer";

        var claimedItemResponse = await client.PostAsJsonAsync(location + "/claim", new FoundItemClaimRequestDTO(claimer));

        claimedItemResponse.EnsureSuccessStatusCode();

        var claimedItem = await GetFoundItemResponse(claimedItemResponse);
        Assert.Equal(Status.Claimed, claimedItem.Status);
        Assert.NotEqual(default, claimedItem.ClaimedAtUtc);
        Assert.Equal(claimer, claimedItem.ClaimedBy);

    }
    [Fact]
    public async Task ClaimItem_MarkItemAsClaimedIdOfItemNotAvailable_Return404AndErroMessage()
    {
        var client = Client;

        var response = await client.PostAsJsonAsync($"/api/items/{Guid.NewGuid()}/claim", new FoundItemClaimRequestDTO("Test claimer"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var notFoundRequest = await response.Content.ReadAsStringAsync();
        Assert.Contains("Item not found!", notFoundRequest);
    }
    [Fact]
    public async Task ClaimItem_MarkItemAsClaimedItemIsNotAvailable_Return409AndErrorMessage()
    {
        var client = Client;
        var foundItemResponse = await CreateAnItemOnTheServer(client, foundItems[0]);
        foundItemResponse.EnsureSuccessStatusCode();
        var location = await GetLocationOfResponse(foundItemResponse);
        var claimedItemResponse = await client.PostAsJsonAsync(location + "/claim", new FoundItemClaimRequestDTO("Test claimer"));
        claimedItemResponse.EnsureSuccessStatusCode();

        var claimClaimedItemResponse = await client.PostAsJsonAsync(location + "/claim", new FoundItemClaimRequestDTO("Second claimer"));

        Assert.Equal(HttpStatusCode.Conflict, claimClaimedItemResponse.StatusCode);
        var notFoundRequest = await claimClaimedItemResponse.Content.ReadAsStringAsync();
        Assert.Contains("Item not available!", notFoundRequest);
    }
}
