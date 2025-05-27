using System.ComponentModel.DataAnnotations;

namespace DashLain.Data.Models;

public sealed class DbCategory {
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DbEntry> Entries { get; set; } = new HashSet<DbEntry>();
}
