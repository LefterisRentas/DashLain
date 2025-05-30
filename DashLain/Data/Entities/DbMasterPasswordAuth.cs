using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashLain.Entities.Models;
public sealed class DbMasterPasswordAuth {
    [Key]
    public Guid ProfileId { get; set; } = Guid.CreateVersion7();

    public byte[] Hash { get; set; } = [];

    public byte[] Salt { get; set; } = [];

    public DbProfile? Profile { get; set; }
}
