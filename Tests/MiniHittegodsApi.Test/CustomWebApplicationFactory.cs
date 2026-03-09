using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MiniHittegodsApi.Interfaces;
using MiniHittegodsApi.Repository;
using MiniHittegodsApi.Services;


namespace MiniHittegodsApi.Test;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IFoundItemRepository>();
            services.AddSingleton<IFoundItemRepository, InMemoryRepository>();

            services.RemoveAll<IFoundItemService>();
            services.AddSingleton<IFoundItemService, FoundItemService>();
        });
    }
}