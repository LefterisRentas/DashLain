using DashLain.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DashLain.Data;
public class AppDbContext : DbContext {
    public DbSet<DbEntry> Entries => Set<DbEntry>();

    public DbSet<DbMasterRecord> MasterRecords => Set<DbMasterRecord>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<DbEntry>()
            .ToTable("entries")
            .HasIndex(e => e.Title);

        builder
            .Entity<DbEntry>()
            .HasOne(e => e.Category)
            .WithMany(c => c.Entries)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .Entity<DbEntry>()
            .HasMany(e => e.Tags)
            .WithMany(t => t.VaultEntries)
            .UsingEntity(x => x.ToTable("entry-tags"));

        builder.Entity<DbTag>().ToTable("tags");

        builder.Entity<DbCategory>().ToTable("categories");

        builder.Entity<DbMasterRecord>().ToTable("master-record");
    }
}
