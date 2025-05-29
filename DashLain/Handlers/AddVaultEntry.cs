namespace DashLain.Handlers;

public sealed class AddVaultEntryCommand {
    public required string Title { get; set; }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }
}
