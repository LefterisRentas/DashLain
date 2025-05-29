using System.ComponentModel.DataAnnotations;

namespace DashLain.Data.Entities;
public sealed class DbMasterPasswordAuth {
    [Key]
    public Guid ProfileId { get; set; }

    public byte[] Hash { get; set; } = Array.Empty<byte>();

    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public DbProfile Profile { get; set; }
}
