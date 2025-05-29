using DashLain.Handlers;
using DashLain.State;
using DashLain.Data;
using DashLain.Data.Entities;

namespace DashLain.Services;

public sealed class VaultService {
    readonly AppDbContext _context;
    readonly Cryptographer _cryptographer;

    public VaultService(AppDbContext context, Cryptographer cryptographer)
    {
        _context = context;
        _cryptographer = cryptographer;
    }

    public async Task AddEntry(AddVaultEntryCommand command)
    {
        var entry = new DbEntry
        {
            ProfileId = SessionState.Get().Id,
            Title = Convert.ToBase64String(_cryptographer.Encrypt(command.Title, SessionState.Get().SessionKey)),
            Username = Convert.ToBase64String(_cryptographer.Encrypt(command.Username, SessionState.Get().SessionKey)),
            Password = _cryptographer.Encrypt(command.Password, SessionState.Get().SessionKey),
            Email = Convert.ToBase64String(_cryptographer.Encrypt(command.Email, SessionState.Get().SessionKey)),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        await _context.Entries.AddAsync(entry);
        await _context.SaveChangesAsync();
    }
}
