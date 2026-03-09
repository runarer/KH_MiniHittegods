using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;
using MiniHittegodsCore.Model.DTO;

namespace MiniHittegodsApi.Test;

public class TestEnvironment : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected WebApplicationFactory<Program> _factory;

    public TestEnvironment(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public HttpClient Client => _factory.CreateClient();

    public async Task RestartAsync()
    {
        await _factory.DisposeAsync();
        _factory = new WebApplicationFactory<Program>();
    }

    protected static async Task<HttpResponseMessage> CreateAnItemOnTheServer(HttpClient client, FoundItemPostRequestDTO foundItem)
    {
        var response = await client.PostAsJsonAsync("api/items", foundItem);
        response.EnsureSuccessStatusCode();

        return response;
    }

    protected static async Task<Uri> GetLocationOfResponse(HttpResponseMessage response)
    {
        Assert.NotNull(response.Headers.Location);
        return response.Headers.Location;
    }

    protected static async Task<FoundItemResponseDTO> GetFoundItemResponse(HttpResponseMessage response)
    {
        var createdNoteResponse = await response.Content.ReadFromJsonAsync<FoundItemResponseDTO>();
        Assert.NotNull(createdNoteResponse);
        return createdNoteResponse;
    }

    protected static async Task<List<HttpResponseMessage>> CreateSeveralFoundItemsOnServer(HttpClient client, IEnumerable<FoundItemPostRequestDTO> items)
    {
        List<HttpResponseMessage> results = [];

        foreach (var item in items)
        {
            var addedNote = await CreateAnItemOnTheServer(client, item);
            results.Add(addedNote);
        }

        return results;
    }

    protected static readonly FoundItemPostRequestDTO[] foundItems =
    [
        new FoundItemPostRequestDTO("Test item with index 0","Test description for item 0",Category.Other,"Test found location for item 0"),
        new FoundItemPostRequestDTO("Test item with index 1 -SearchTest-","Test description for item 1",Category.Keys,"Test found location for item 1"),
        new FoundItemPostRequestDTO("Test item with index 2","Test description for item 2",Category.Clothning,"Test found location for item 2"),
        new FoundItemPostRequestDTO("Test item with index 3","Test description for item 3",Category.Wallet,"Test found location for item 3"),
        new FoundItemPostRequestDTO("Test item with index 4 -SearchTest-","Test description for item 4",Category.Other,"Test found location for item 4"),
        new FoundItemPostRequestDTO("Test item with index 5","Test description for item 0",Category.Other,"Test found location for item 5"),
        new FoundItemPostRequestDTO("Test item with index 6","Test description for item 1",Category.Keys,"Test found location for item 6"),
        new FoundItemPostRequestDTO("Test item with index 7","Test description for item 2",Category.Clothning,"Test found location for item 7"),
        new FoundItemPostRequestDTO("Test item with index 8","Test description for item 3",Category.Wallet,"Test found location for item 8 -SearchTest-"),
        new FoundItemPostRequestDTO("Test item with index 9","Test description for item 4",Category.Other,"Test found location for item 9 -SearchTest-"),
    ];
}