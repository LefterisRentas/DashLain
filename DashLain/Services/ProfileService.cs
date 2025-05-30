using DashLain.Handlers;
using DashLain.Models;
using DashLain.State;
using Microsoft.EntityFrameworkCore;
using DashLain.Data;
using DashLain.Data.Entities;

namespace DashLain.Services;
public sealed class ProfileService {
    readonly AppDbContext _context;
    readonly MasterPasswordService _masterPasswordService;
    readonly Cryptographer _cryptographer;

    public ProfileService(AppDbContext context, MasterPasswordService masterPasswordService, Cryptographer cryptographer)
    {
        _context = context;
        _masterPasswordService = masterPasswordService;
        _cryptographer = cryptographer;
    }

    public async Task<UIResult<Profile[]>> GetProfiles()
    {
        var result = await _context.Profiles
            .Select(x => new Profile
            {
                Name = x.Name,
                AuthType = x.AuthType
            })
            .ToArrayAsync();
        return UIResult.Succeed(result);
    }

    public async Task<UIResult<Profile>> CreateProfileWithMasterPassword(CreateProfileMasterPasswordCommand command)
    {
        if (await _context.Profiles.AnyAsync(p => p.Name == command.Name))
        {
            return UIResult.Fail<Profile>(["Profile name already exists."]);
        }

        var (hash, salt) = _masterPasswordService.ComputeHash(command.Password);

        var profile = new DbProfile
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            AuthType = AuthType.MasterPassword,
            MasterPasswordAuth = new DbMasterPasswordAuth
            {
                ProfileId = Guid.NewGuid(),
                Hash = hash,
                Salt = salt
            }
        };

        profile.MasterPasswordAuth.ProfileId = profile.Id;

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        return UIResult.Succeed(new Profile { Name = profile.Name});
    }

    public async Task<UIResult<bool>> LoginWithMasterPassword(LoginMasterPasswordCommand command)
    {
        var storedProfile = await _context.Profiles
            .Where(x => x.Name == command.Name)
            .Select(x => x.MasterPasswordAuth)
            .FirstOrDefaultAsync();

        if (storedProfile is null)
        {
            return UIResult.Fail<bool>(["The provided credentials are invalid."]);
        }

        var loginSuccess = _masterPasswordService.VerifyPassword(command.Password, storedProfile!.Hash, storedProfile.Salt);
        if (!loginSuccess)
        {
            return UIResult.Fail<bool>(["The provided credentials are invalid."]);
        }

        SessionState.Set(new ProfileContext
        {
            Name = command.Name,
            Id = storedProfile.ProfileId,
            SessionKey = _cryptographer.DeriveKey(command.Password, storedProfile.Salt)
        });
        return UIResult.Succeed(true);
    }
}
