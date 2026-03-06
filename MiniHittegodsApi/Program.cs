using MiniHittegodsApi.DTOs;
using Scalar.AspNetCore;

using MiniHittegodsCore.Services;
using MiniHittegodsCore.Repository;
using MiniHittegodsCore.Model.DTO;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var storage = new InMemoryRepository();
var service = new FoundItemService(storage, TimeProvider.System);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapPost("/api/items", async (FoundItemPostRequestDTO foundItem) =>
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


app.Run();

