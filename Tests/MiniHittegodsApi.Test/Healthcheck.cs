
using Microsoft.AspNetCore.Mvc.Testing;

namespace MiniHittegodsApi.Test;

public class HealthCheck(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task Get_EndpointReturnSuccess()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        response.EnsureSuccessStatusCode();
    }
}