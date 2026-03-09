using Microsoft.EntityFrameworkCore;
using MiniHittegodsApi.Interfaces;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Repository;

public class PostgreSqlRepository : IFoundItemRepository
{
    private readonly FoundItemDbContext _dbContext;

    // public PostgreSqlRepository()
    // {
    //     string connectionString = "";

    //     var optionsBuilder = new DbContextOptionsBuilder<FoundItemDbContext>();
    //     optionsBuilder.UseNpgsql(connectionString);

    //     dbContext = new FoundItemDbContext(optionsBuilder.Options);
    // }

    public PostgreSqlRepository(FoundItemDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task AddFoundItemAsync(FoundItem foundItem)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFoundItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<FoundItem?> GetFoundItemAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<FoundItem>> GetItems(Status? status, Category? category, string? searchQuery)
    {
        throw new NotImplementedException();
    }

    public Task Save()
    {
        throw new NotImplementedException();
    }
}
