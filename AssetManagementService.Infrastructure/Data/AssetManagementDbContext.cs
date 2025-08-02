using AssetManagementService.Domain.Aggregates.Asset;
using AssetManagementService.Domain.Common;
using AssetManagementService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
public class AssetManagementDbContext : DbContext
{
    public DbSet<Asset> Assets { get; set; } = null!;
    public DbSet<Trade> Trades { get; set; } = null!;
    public DbSet<Replenishment> Replenishments { get; set; } = null!;

    public AssetManagementDbContext(DbContextOptions<AssetManagementDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.HasQueryFilter(a => !a.IsDeleted);

            entity.Property(a => a.Symbol).IsRequired();
            entity.Property(a => a.Name).IsRequired();

            entity.HasMany(a => a.Trades)
                  .WithOne()
                  .HasForeignKey(t => t.AssetId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(a => a.Replenishments)
                  .WithOne()
                  .HasForeignKey(r => r.AssetId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Trade>(entity =>
        {
            entity.HasKey(t => t.Id);
            
            entity.HasQueryFilter(t => !t.IsDeleted);

            entity.Property(t => t.Price).IsRequired();

            entity.OwnsOne(t => t.Price, owned =>
            {
                owned.Property(p => p.Amount)
                     .HasColumnName("Price_Amount")
                     .IsRequired();

                owned.Property(p => p.Currency)
                     .HasColumnName("Price_Currency")
                     .IsRequired();
            });
        });

        modelBuilder.Entity<Replenishment>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasQueryFilter(r => !r.IsDeleted);

            entity.OwnsOne(r => r.Amount, owned =>
            {
                owned.Property(p => p.Amount)
                     .HasColumnName("Price_Amount")
                     .IsRequired();

                owned.Property(p => p.Currency)
                     .HasColumnName("Price_Currency")
                     .IsRequired();
            });
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<Entity<Guid>>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
