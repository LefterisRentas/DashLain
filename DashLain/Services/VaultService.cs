using DashLain.Entities;
using DashLain.Entities.Models;
using DashLain.Handlers;
using DashLain.Models;
using DashLain.State;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DashLain.Services;

public sealed class VaultService {
    readonly AppDbContext _context;
    readonly Cryptographer _cryptographer;

    public VaultService(AppDbContext context, Cryptographer cryptographer)
    {
        _context = context;
        _cryptographer = cryptographer;
    }

    public async Task<UIResult<bool>> AddEntry(AddVaultEntryCommand command)
    {
        var entry = new DbEntry
        {
            ProfileId = SessionState.Get().Id,
            Title = Convert.ToBase64String(_cryptographer.Encrypt(command.Title)),
            Username = Convert.ToBase64String(_cryptographer.Encrypt(command.Username)),
            Password = _cryptographer.Encrypt(command.Password),
            Email = Convert.ToBase64String(_cryptographer.Encrypt(command.Email)),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        await _context.Entries.AddAsync(entry);
        await _context.SaveChangesAsync();
        return UIResult.Succeed(true);
    }
}
