using System.ComponentModel.DataAnnotations;
using DashLain.Models;

namespace DashLain.Data.Entities;

public sealed class DbProfile {
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public AuthType AuthType { get; set; }

    public DbMasterPasswordAuth? MasterPasswordAuth { get; set; } = null;

    public ICollection<DbEntry> VaultEntries { get; set; } = new HashSet<DbEntry>();
}
