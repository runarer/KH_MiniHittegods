using Microsoft.EntityFrameworkCore;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Repository;

public class FoundItemDbContext(DbContextOptions<FoundItemDbContext> options) : DbContext(options)
{
    public DbSet<FoundItem> FoundItems { get; set; }
}
