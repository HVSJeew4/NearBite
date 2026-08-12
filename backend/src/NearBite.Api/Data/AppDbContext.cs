using Microsoft.EntityFrameworkCore;
using NearBite.Api.Domain;

namespace NearBite.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly configure the one-to-many relationship + cascade delete.
        modelBuilder.Entity<Listing>()
            .HasMany(l => l.MenuItems)
            .WithOne(m => m.Listing)
            .HasForeignKey(m => m.ListingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}