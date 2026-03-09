
using Microsoft.EntityFrameworkCore;
using MiniHittegodsApi.Model;

namespace MiniHittegodsApi.Context;

public class FoundItemContext : DbContext
{
    public DbSet<FoundItem> FoundItems { get; set; }

}
