namespace DashLain.Data.Models;

public sealed class DbTag {
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Name { get; set; } = string.Empty;

    public ICollection<DbEntry> VaultEntries { get; set; } = new HashSet<DbEntry>();
}
