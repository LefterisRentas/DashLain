namespace DashLain.Data.Models;

public sealed class DbEntry {
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Title { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public byte[] Password { get; set; } = Array.Empty<byte>();

    public string Link { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public byte[] IV { get; set; } = Array.Empty<byte>();

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Guid? CategoryId { get; set; } = null;

    public DbCategory? Category { get; set; } = null;

    public ICollection<DbTag> Tags { get; set; } = new HashSet<DbTag>();
}
