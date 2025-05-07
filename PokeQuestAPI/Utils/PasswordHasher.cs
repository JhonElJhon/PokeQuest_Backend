using System.Security.Cryptography;

public class PasswordHasher
{
    private const int SaltSize = 32; // 256-bit salt
    private const int HashSize = 64; // 512-bit hash
    private const int Iterations = 10000; // PBKDF2 iteration count

    public static (byte[] hash, byte[] salt) CreateHash(string password)
    {
        // Generate random salt
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        // Create hash using PBKDF2 with HMAC-SHA512
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA512);

        var hash = pbkdf2.GetBytes(HashSize);

        return (hash, salt);
    }

    public static bool VerifyHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        // Recreate the hash with the stored salt
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            storedSalt,
            Iterations,
            HashAlgorithmName.SHA512);

        var computedHash = pbkdf2.GetBytes(HashSize);

        // Compare hashes in constant time
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}