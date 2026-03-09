using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MiniHittegodsApi.DTOs;
using MiniHittegodsCore.Model;

namespace MiniHittegodsApi.Test;

public class RegisterFoundItemTest(CustomWebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task RegisterItem_RegisterAValidItem_Returns201LocationAndObjectCreated()
    {
        var client = Client;
        var foundItemPostRequestDTO = new FoundItemPostRequestDTO("Test Item", "Test item description", Category.Other, "Alley behind the abandoned mansion.");

        var response = await client.PostAsJsonAsync("/api/items", foundItemPostRequestDTO);
        response.EnsureSuccessStatusCode();

        var createResponse = await response.Content.ReadFromJsonAsync<FoundItemResponseDTO>();
        Assert.NotNull(createResponse);
        Assert.NotEqual(Guid.Empty, createResponse.Id);
        Assert.Equal(foundItemPostRequestDTO.Title, createResponse.Title);
        Assert.Equal(foundItemPostRequestDTO.Description, createResponse.Description);
        Assert.Equal(foundItemPostRequestDTO.Category, createResponse.Category);
        Assert.Equal(foundItemPostRequestDTO.FoundLocation, createResponse.FoundLocation);
        Assert.Equal(Status.Available, createResponse.Status);
        Assert.NotEqual(default, createResponse.FoundAtUtc);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"api/items/{createResponse.Id}", response.Headers.Location.ToString());
    }
}