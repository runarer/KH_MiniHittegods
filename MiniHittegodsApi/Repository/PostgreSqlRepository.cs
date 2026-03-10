using Microsoft.EntityFrameworkCore;
using MiniHittegodsApi.Interfaces;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Repository;

public class PostgreSqlRepository : IFoundItemRepository
{
    private readonly FoundItemDbContext _dbContext;

    public PostgreSqlRepository(FoundItemDbContext dbContext)
    {
        _dbContext = dbContext;
        dbContext.Database.Migrate();
    }
    public async Task AddFoundItemAsync(FoundItem foundItem)
    {
        _dbContext.FoundItems.Add(foundItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFoundItemAsync(Guid id)
    {
        await _dbContext.FoundItems.Where(item => item.Id == id).ExecuteDeleteAsync();
    }

    public async Task<FoundItem?> GetFoundItemAsync(Guid id)
    {
        return await _dbContext.FoundItems.FirstOrDefaultAsync(item => item.Id == id);
    }

    public async Task<IReadOnlyList<FoundItem>> GetItems(Status? status, Category? category, string? searchQuery)
    {
        var query = _dbContext.FoundItems.AsQueryable();

        if (status is not null)
            query = query.Where(item => item.Status == status.Value);

        if (category is not null)
            query = query.Where(item => item.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(searchQuery))
            query = query.Where(item => item.Title.Contains(searchQuery) || item.Description.Contains(searchQuery));

        return await query.ToListAsync();
    }
}
