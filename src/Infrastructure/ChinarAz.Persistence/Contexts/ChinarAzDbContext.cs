using ChinarAz.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Contexts;

public class ChinarAzDbContext : IdentityDbContext<AppUser>
{
    public ChinarAzDbContext(DbContextOptions<ChinarAzDbContext> options) : base(options)
    {
    }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Favourite> Favourites { get; set; }
    public DbSet<Bio> Bios { get; set; }
    public DbSet<Image> Images { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChinarAzDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
