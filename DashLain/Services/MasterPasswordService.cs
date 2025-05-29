using System.Security.Cryptography;

namespace DashLain.Services;
public sealed class MasterPasswordService {
    const int HashSize = 64;
    const int SaltSize = 32;
    const int Iterations = 100_000;

    public (byte[] Hash, byte[] Salt) ComputeHash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512).GetBytes(HashSize);
        return (hash, salt);
    }

    public bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        var computedHash = new Rfc2898DeriveBytes(password, storedSalt, Iterations, HashAlgorithmName.SHA512).GetBytes(HashSize);
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }

    public byte[] DeriveKey(string password, byte[] salt, int keySize = 32)
    {
        return new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512).GetBytes(keySize);
    }
}
