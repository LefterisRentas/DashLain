using DashLain.Entities;
using DashLain.Entities.Models;
using DashLain.Handlers;
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

    public async Task AddEntry(AddVaultEntryCommand command)
    {
        var iv = _cryptographer.GenerateIV();

        var entry = new DbEntry
        {
            ProfileId = SessionState.Get().Id,
            InitialVector = iv,
            Title = Convert.ToBase64String(_cryptographer.Encrypt(command.Title, SessionState.Get().SessionKey)),
            Username = Convert.ToBase64String(_cryptographer.Encrypt(command.Username, SessionState.Get().SessionKey)),
            Password = _cryptographer.Encrypt(command.Password, SessionState.Get().SessionKey),
            Email = Convert.ToBase64String(_cryptographer.Encrypt(command.Email, SessionState.Get().SessionKey)),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _context.Entries.Add(entry);
        await _context.SaveChangesAsync();
    }
}
