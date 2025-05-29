using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashLain.Entities.Models;

public sealed class DbEntry {
    [Key]
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid ProfileId { get; set; }

    public DbProfile? Profile { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public byte[] Password { get; set; } = [];

    [MaxLength(2048)]
    public string Link { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(2048)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    public byte[] Salt { get; set; } = [];

    [Required]
    public byte[] InitialVector { get; set; } = [];

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Guid? CategoryId { get; set; }

    public DbCategory? Category { get; set; }

    public ICollection<DbTag> Tags { get; set; } = new HashSet<DbTag>();
}
