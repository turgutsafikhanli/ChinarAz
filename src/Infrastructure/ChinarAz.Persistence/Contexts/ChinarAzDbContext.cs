using ChinarAz.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Contexts;

public class ChinarAzDbContext : IdentityDbContext<AppUser>
{
    public ChinarAzDbContext(DbContextOptions<ChinarAzDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChinarAzDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
