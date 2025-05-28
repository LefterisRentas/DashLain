using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashLain.Data.Models;
public sealed class DbMasterPasswordAuth {
    [Key]
    public Guid ProfileId { get; set; }

    public byte[] Hash { get; set; } = Array.Empty<byte>();

    public byte[] Salt { get; set; } = Array.Empty<byte>();

    public DbProfile Profile { get; set; }
}
