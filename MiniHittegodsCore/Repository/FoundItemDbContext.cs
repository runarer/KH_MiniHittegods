using Microsoft.EntityFrameworkCore;
using MiniHittegodsCore.Model;

namespace MiniHittegodsCore.Repository;

public class FoundItemDbContext(DbContextOptions<FoundItemDbContext> options) : DbContext(options)
{
    public DbSet<FoundItem> FoundItems { get; set; }
}
