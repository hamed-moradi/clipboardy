using System;
using System.Security.Cryptography;
using System.Text;

namespace Assets.Utility.Infrastructure
{
    public static class PasswordUtils
    {
        public static string GenerateBase64(int length)
        {
            byte[] randomBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        public static string EncryptWithSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
