using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Testing;
using MiniHittegodsApi.DTOs;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Test;

public class TestEnvironment : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected WebApplicationFactory<Program> _factory;

    public TestEnvironment(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    public HttpClient Client => _factory.CreateClient();

    protected async Task<HttpResponseMessage> CreateAnItemOnTheServer(HttpClient client, FoundItemPostRequestDTO foundItem)
    {
        var response = await client.PostAsJsonAsync("api/items", foundItem);
        response.EnsureSuccessStatusCode();

        return response;
    }

    protected async Task<Uri> GetLocationOfResponse(HttpResponseMessage response)
    {
        Assert.NotNull(response.Headers.Location);
        return response.Headers.Location;
    }

    protected async Task<FoundItemResponseDTO> GetFoundItemResponse(HttpResponseMessage response)
    {
        var createdNoteResponse = await response.Content.ReadFromJsonAsync<FoundItemResponseDTO>();
        Assert.NotNull(createdNoteResponse);
        return createdNoteResponse;
    }

    protected async Task<List<HttpResponseMessage>> CreateSeveralFoundItemsOnServer(HttpClient client, IEnumerable<FoundItemPostRequestDTO> items)
    {
        List<HttpResponseMessage> results = [];

        foreach (var item in items)
        {
            var addedNote = await CreateAnItemOnTheServer(client, item);
            results.Add(addedNote);
        }

        return results;
    }

    // protected readonly FoundItemPostRequestDTO[] foundItems =
    // [
    //     new FoundItemPostRequestDTO("Test item with index 0","Test description for item 0",Category.Other,"Test found location for item 0"),
    //     new FoundItemPostRequestDTO("Test item with index 1 -SearchTest-","Test description for item 1",Category.Keys,"Test found location for item 1"),
    //     new FoundItemPostRequestDTO("Test item with index 2","Test description for item 2",Category.Clothning,"Test found location for item 2"),
    //     new FoundItemPostRequestDTO("Test item with index 3","Test description for item 3",Category.Wallet,"Test found location for item 3"),
    //     new FoundItemPostRequestDTO("Test item with index 4 -SearchTest-","Test description for item 4",Category.Other,"Test found location for item 4"),
    //     new FoundItemPostRequestDTO("Test item with index 5","Test description for item 0",Category.Other,"Test found location for item 5"),
    //     new FoundItemPostRequestDTO("Test item with index 6","Test description for item 1",Category.Keys,"Test found location for item 6"),
    //     new FoundItemPostRequestDTO("Test item with index 7","Test description for item 2",Category.Clothning,"Test found location for item 7"),
    //     new FoundItemPostRequestDTO("Test item with index 8","Test description for item 3 -SearchTest-",Category.Wallet,"Test found location for item 8"),
    //     new FoundItemPostRequestDTO("Test item with index 9","Test description for item 4 -SearchTest-",Category.Other,"Test found location for item 9"),
    // ];
    protected readonly FoundItemPostRequestDTO[] foundItems =
[
    new FoundItemPostRequestDTO{Title = "Test item with index 0", Description = "Test description for item 0",Category = Category.Other,FoundLocation = "Test found location for item 0"},
            new FoundItemPostRequestDTO{Title = "Test item with index 1 -SearchTest-", Description = "Test description for item 1",Category = Category.Keys,FoundLocation = "Test found location for item 1"},
            new FoundItemPostRequestDTO{Title = "Test item with index 2", Description = "Test description for item 2",Category = Category.Clothning,FoundLocation = "Test found location for item 2"},
            new FoundItemPostRequestDTO{Title = "Test item with index 3", Description = "Test description for item 3",Category = Category.Wallet,FoundLocation = "Test found location for item 3"},
            new FoundItemPostRequestDTO{Title = "Test item with index 4 -SearchTest-", Description = "Test description for item 4",Category = Category.Other,FoundLocation = "Test found location for item 4"},
            new FoundItemPostRequestDTO{Title = "Test item with index 5", Description = "Test description for item 0",Category = Category.Other,FoundLocation = "Test found location for item 5"},
            new FoundItemPostRequestDTO{Title = "Test item with index 6", Description = "Test description for item 1",Category = Category.Keys,FoundLocation = "Test found location for item 6"},
            new FoundItemPostRequestDTO{Title = "Test item with index 7", Description = "Test description for item 2",Category = Category.Clothning,FoundLocation = "Test found location for item 7"},
            new FoundItemPostRequestDTO{Title = "Test item with index 8", Description = "Test description for item 3 -SearchTest-",Category = Category.Wallet,FoundLocation = "Test found location for item 8"},
            new FoundItemPostRequestDTO{Title = "Test item with index 9", Description = "Test description for item 4 -SearchTest-",Category = Category.Other,FoundLocation = "Test found location for item 9"},
        ];
}