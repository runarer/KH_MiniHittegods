using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NoteStorageTests;

public class TestEnvironment : IClassFixture<WebApplicationFactory<Program>>
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
}