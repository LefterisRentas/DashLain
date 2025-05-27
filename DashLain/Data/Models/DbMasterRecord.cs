using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Data.Models;

public sealed class DbMasterRecord {
    public Guid Id { get; set; } = Guid.CreateVersion7();

    [Required]
    public byte[] Hash { get; set; } = [];

    [Required]
    public byte[] Salt { get; set; } = [];
}
