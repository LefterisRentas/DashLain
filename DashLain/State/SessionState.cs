using System.Security.Cryptography;

namespace DashLain.State;

public sealed class ProfileContext
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public byte[] SessionKey { get; set; } = [];
}

public static class SessionState {
    private static ProfileContext? _context;

    public static bool IsAuthenticated => _context is not null;

    public static ProfileContext Get() => _context ?? throw new InvalidOperationException("User not authenticated");

    public static void Set(ProfileContext context) => _context = context;

    // This should be called when app exits (gracefully or not) so the memory can be cleared.
    public static void Clear()
    {
        if (_context is not null)
        {
            CryptographicOperations.ZeroMemory(_context.SessionKey);
        }
        _context = null;
    }
}

