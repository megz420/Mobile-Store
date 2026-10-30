using System.Security.Cryptography;
using System.Text;

namespace MobileStore.Application.Helpers
{
    public static class PasswordHashHelper
    {
        // ============================================
        // Hash Password using BCrypt (Recommended)
        // ============================================
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // ============================================
        // Verify Password
        // ============================================
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // ============================================
        // Alternative: SHA256 (Less Secure)
        // ============================================
        public static string HashPasswordSHA256(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // ============================================
        // Generate Random Token
        // ============================================
        public static string GenerateRandomToken(int length = 32)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}