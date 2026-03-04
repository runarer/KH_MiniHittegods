using System;
using Microsoft.EntityFrameworkCore;
using MiniHittegodsCore.Model;

namespace MiniHittegodsCore.Context;

public class FoundItemContext : DbContext
{
    public DbSet<FoundItem> FoundItems { get; set; }

}
