using System.Security.Cryptography;
using System.Text;

namespace Car_projekt.Helper
{
    public class SecurityHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            // Generiere ein Salt
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            salt = Convert.ToBase64String(saltBytes);

            // Iterative SHA-512 Hashing
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            // Mehrere Iterationen für zusätzliche Sicherheit
            byte[] hashBytes = combinedBytes;
            for (int i = 0; i < 100000; i++) // 100.000 Iterationen
            {
                using (var sha = SHA512.Create())
                {
                    hashBytes = sha.ComputeHash(hashBytes);
                }
            }

            return Convert.ToBase64String(hashBytes);
        }


        public static bool VerifyPassword(string password, string salt, string storedHash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            byte[] hashBytes = combinedBytes;
            for (int i = 0; i < 100000; i++) // 100.000 Iterationen
            {
                using (var sha = SHA512.Create())
                {
                    hashBytes = sha.ComputeHash(hashBytes);
                }
            }

            string computedHash = Convert.ToBase64String(hashBytes);
            return storedHash == computedHash;
        }
    }
}
