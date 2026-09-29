using System.Security.Cryptography;
using System.Text;

namespace ProyectoIA.Domain;

public static class PasswordHasher
{
    private const int CurrentIterations = 210_000;
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string Hash(string password)
    {
        return HashWithSalt(password, RandomNumberGenerator.GetBytes(SaltSize), CurrentIterations);
    }

    public static string HashForSeed(string password, string salt) =>
        HashWithSalt(password, Encoding.UTF8.GetBytes(salt), CurrentIterations);

    public static bool Verify(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            return false;

        if (hash.Length == 64 && hash.All(Uri.IsHexDigit))
        {
            var actualLegacyHash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var expectedLegacyHash = Convert.FromHexString(hash);
            return CryptographicOperations.FixedTimeEquals(actualLegacyHash, expectedLegacyHash);
        }

        var parts = hash.Split('$');
        if (parts.Length != 4 ||
            parts[0] != "pbkdf2-sha256" ||
            !int.TryParse(parts[1], out var iterations) ||
            iterations < 100_000 ||
            iterations > 1_000_000)
            return false;

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expectedHash = Convert.FromBase64String(parts[3]);
            if (salt.Length < SaltSize || expectedHash.Length != HashSize)
                return false;

            var actualHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static bool NeedsRehash(string hash) =>
        !hash.StartsWith("pbkdf2-sha256$", StringComparison.Ordinal) ||
        !int.TryParse(hash.Split('$').ElementAtOrDefault(1), out var iterations) ||
        iterations < CurrentIterations;

    private static string HashWithSalt(string password, byte[] salt, int iterations)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            HashSize);
        return $"pbkdf2-sha256${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }
}
