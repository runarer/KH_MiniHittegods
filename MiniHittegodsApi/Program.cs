using MiniHittegodsApi.DTOs;
using Scalar.AspNetCore;

using MiniHittegodsApi.Services;
using MiniHittegodsApi.Repository;
using MiniHittegodsApi.Model.DTO;
using MiniHittegodsApi.Model;
using MiniHittegodsApi.Interfaces;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddValidation();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FoundItemDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Scoped);


builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IFoundItemRepository, PostgreSqlRepository>();
builder.Services.AddSingleton<IFoundItemService, FoundItemService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoundItemDbContext>();
    dbContext.Database.Migrate();
}

app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapPost("/api/items", async (FoundItemPostRequestDTO foundItem, IFoundItemService service) =>
{
    var createFoundItemDTO = new CreateFoundItemDTO
    {
        Title = foundItem.Title,
        FoundLocation = foundItem.FoundLocation,
        Description = foundItem.Description,
        Category = foundItem.Category
    };

    var (type, foundItemDTO) = await service.Add(createFoundItemDTO);

    if (type != FoundItemResultType.Ok || foundItem is null)
        throw new InvalidOperationException("Item was not created");

    return Results.Created($"api/items/{foundItemDTO!.Id}", foundItemDTO);
});

app.MapPost("/api/items/{id:Guid}/claim", async (Guid id, FoundItemClaimRequestDTO claimer, IFoundItemService service) =>
{

    var (type, foundItemDTO) = await service.Claim(id, claimer.ClaimedBy);

    if (type == FoundItemResultType.NotFound)
        return Results.NotFound("Item not found!");

    if (type == FoundItemResultType.Conflict)
        return Results.Conflict("Item not available!");

    if (foundItemDTO is null)
        throw new InvalidOperationException("Item was not created");

    return Results.Ok(foundItemDTO);

});

app.MapPost("/api/items/{id:Guid}/return", async (Guid id, IFoundItemService service) =>
{

    var (type, foundItemDTO) = await service.Return(id);

    if (type == FoundItemResultType.NotFound)
        return Results.NotFound("Item not found!");

    if (type == FoundItemResultType.Conflict)
        return Results.Conflict("Item not claimed!");

    if (foundItemDTO is null)
        throw new InvalidOperationException("Item was not created");

    return Results.Ok(foundItemDTO);

});

app.MapGet("/api/items/{id:Guid}", async (Guid id, IFoundItemService service) =>
{
    var (type, foundItemDTO) = await service.Get(id);

    if (type == FoundItemResultType.NotFound)
        return Results.NotFound("Item not found!");

    if (type != FoundItemResultType.Ok || foundItemDTO is null)
        throw new InvalidOperationException("Unexpected error!");

    return Results.Ok(foundItemDTO);
});

app.MapDelete("/api/items/{id:Guid}", async (Guid id, IFoundItemService service) =>
{
    var (type, _) = await service.Delete(id);

    if (type == FoundItemResultType.NotFound)
        return Results.NotFound("Item not found!");

    if (type == FoundItemResultType.Conflict)
        return Results.Conflict("Item cannot be deleted!");

    return Results.NoContent();
});

app.MapGet("/api/items", async (Status? status, Category? category, string? q, IFoundItemService service) =>
{
    var results = await service.GetAll(status, category, q);

    return Results.Ok(results);
});


app.Run();

