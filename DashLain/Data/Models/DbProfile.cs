using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Data.Models;
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
