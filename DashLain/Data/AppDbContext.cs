using DashLain.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashLain.Data;
public class AppDbContext : DbContext {
    public DbSet<DbEntry> Entries => Set<DbEntry>();

    public DbSet<DbProfile> Profiles => Set<DbProfile>();

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

        builder.Entity<DbMasterPasswordAuth>().ToTable("master-passwords");

        builder.Entity<DbProfile>()
            .ToTable("profiles")
            .HasOne(p => p.MasterPasswordAuth)
            .WithOne(a => a.Profile)
            .HasForeignKey<DbMasterPasswordAuth>(a => a.ProfileId);

        builder.Entity<DbEntry>()
            .HasOne(e => e.Profile)
            .WithMany(p => p.VaultEntries)
            .HasForeignKey(e => e.ProfileId);
    }
}
