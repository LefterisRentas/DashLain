using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Handlers;
public sealed class AddVaultEntryCommand {
    public string Title { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}
