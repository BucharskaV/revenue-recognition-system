using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RevenueRecognitionSystem.Services.Security;

public class SecurityHelpers
{
    public static Tuple<string, string> GetHashedPasswordsAndSalt(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        
        string saltBase64 = Convert.ToBase64String(salt); 
        return new Tuple<string, string>(hashedPassword, saltBase64);
    }

    public static string GetHashedPasswordWithSalt(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        
        string currentHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password:password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        
        return currentHashedPassword;
    }

    public static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }
}