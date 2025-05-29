using System.ComponentModel.DataAnnotations;

namespace DashLain.Data.Entities;

public sealed class DbTag {
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DbEntry> VaultEntries { get; set; } = new HashSet<DbEntry>();
}
