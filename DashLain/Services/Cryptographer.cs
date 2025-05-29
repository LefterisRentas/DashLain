using System.Security.Cryptography;
using System.Text;

namespace DashLain.Services;

public sealed class Cryptographer {
    public const int SaltSize = 32;
    public const int IVSize = 12;
    public const int KeySize = 32;
    public const int TagSize = 16;
    public const int Pbkdf2Iterations = 100_000;

    public byte[] Encrypt(string plainText, byte[] key)
    {
        var plaintextBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherText = new byte[plaintextBytes.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(key, TagSize);
        var iv = GenerateIV();
        aes.Encrypt(iv, plaintextBytes, cipherText, tag);

        return Combine(cipherText, tag);
    }

    public string Decrypt(byte[] ciphertextWithTag, byte[] key)
    {
        var ciphertext = ciphertextWithTag[..^TagSize];
        var tag = ciphertextWithTag[^TagSize..];
        var plaintextBytes = new byte[ciphertext.Length];

        using var aes = new AesGcm(key, TagSize);
        var iv = GenerateIV();
        aes.Decrypt(iv, ciphertext, tag, plaintextBytes);

        return Encoding.UTF8.GetString(plaintextBytes);
    }

    private static byte[] Combine(byte[] a, byte[] b)
    {
        var result = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, result, 0, a.Length);
        Buffer.BlockCopy(b, 0, result, a.Length, b.Length);
        return result;
    }

    public byte[] GenerateSalt() => RandomNumberGenerator.GetBytes(SaltSize);

    public byte[] GenerateIV() => RandomNumberGenerator.GetBytes(IVSize);

    public byte[] DeriveKey(string password, byte[] salt)
    {
        return new Rfc2898DeriveBytes(
            password,
            salt,
            Pbkdf2Iterations,
            HashAlgorithmName.SHA3_512
        ).GetBytes(KeySize);
    }
}
