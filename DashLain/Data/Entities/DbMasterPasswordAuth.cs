using System.ComponentModel.DataAnnotations;

namespace DashLain.Data.Entities;
public sealed class DbMasterPasswordAuth {
    [Key]
    public Guid ProfileId { get; set; }

    public byte[] Hash { get; set; } = [];

    public byte[] Salt { get; set; } = [];

    public DbProfile Profile { get; set; } = null!;
}
