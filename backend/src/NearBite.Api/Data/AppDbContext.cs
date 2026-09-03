using Microsoft.EntityFrameworkCore;
using NearBite.Api.Domain;
using NearBite.Api.Domain.Enums;

namespace NearBite.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<AdminActionLog> AdminActionLogs => Set<AdminActionLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-to-many: Listing → MenuItems (from Sprint 4)
        modelBuilder.Entity<Listing>()
            .HasMany(l => l.MenuItems)
            .WithOne(m => m.Listing)
            .HasForeignKey(m => m.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: Listing → Reviews (NEW in Sprint 5)
        modelBuilder.Entity<Listing>()
            .HasMany(l => l.Reviews)
            .WithOne(r => r.Listing)
            .HasForeignKey(r => r.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enum stored as string, not int (NEW in Sprint 5)
        modelBuilder.Entity<Listing>()
            .Property(l => l.SubmissionStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Index on Reviews.ListingId for fast lookups
        modelBuilder.Entity<Review>()
            .HasIndex(r => r.ListingId);

        // Index on AdminActionLogs.Timestamp for time-based queries
        modelBuilder.Entity<AdminActionLog>()
            .HasIndex(l => l.Timestamp);
    }
}