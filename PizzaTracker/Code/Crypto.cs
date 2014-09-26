using System;
using System.Security.Cryptography;
using System.Text;

namespace PizzaTracker.Code
{
    public static class Crypto
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        public static string GenerateSalt()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            var random = new byte[32];
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }

        public static string HashPassword(string plainText, string salt)
        {
            // Convert plain text into a byte array.
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Convert.FromBase64String(salt);

            // Allocate array, which will hold plain text and salt.
            var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }


            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }
            var hash = new SHA512Managed();
            var passwordHashed = hash.ComputeHash(plainTextWithSaltBytes);
            return Convert.ToBase64String(passwordHashed);
        }
    }
}