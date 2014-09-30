using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PizzaTracker.Data;
using PizzaTracker.Models;

namespace PizzaTracker.Code
{
    public class Aes256
    {
        private const string Key = "VFh0cjRmZitaT1MzbUxHRk9zNzdHZz09LGlqT3RxNllrK0ZoOU1jY0szbDd4b1E9PQ==";
        private const int Keysize = 256;

        /// <summary>
        /// Generate a private key
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// </summary>
        public static string GenerateKey(int iKeySize)
        {
            var aesEncryption = new RijndaelManaged
            {
                KeySize = iKeySize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            aesEncryption.GenerateIV();
            var ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            var keyStr = Convert.ToBase64String(aesEncryption.Key);
            var completeKey = ivStr + "," + keyStr;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(completeKey));
        }

        /// <summary>
        /// Encrypt
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// </summary>
        public static string Encrypt(string iPlainStr)
        {
            var aesEncryption = new RijndaelManaged
            {
                KeySize = Keysize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(Key)).Split(',')[0]),
                Key = Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(Key)).Split(',')[1])
            };
            var plainText = Encoding.UTF8.GetBytes(iPlainStr);
            var crypto = aesEncryption.CreateEncryptor();
            var cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Decrypt
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// </summary>
        public static string Decrypt(string iEncryptedText)
        {
            var aesEncryption = new RijndaelManaged
            {
                KeySize = Keysize,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV =
                    Convert.FromBase64String(
                        Encoding.UTF8.GetString(Convert.FromBase64String(Key)).Split(',')[0]),
                Key =
                    Convert.FromBase64String(
                        Encoding.UTF8.GetString(Convert.FromBase64String(Key)).Split(',')[1])
            };
            var decrypto = aesEncryption.CreateDecryptor();
            var encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
            return Encoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
    }

    //public static class UserInfo
    //{
    //    public static User GetUserInfo(PizzaTrackerRepo repo, string encrypted)
    //    {
    //        var decrypted = Aes256.Decrypt(encrypted);
    //        var loginVm = JsonConvert.DeserializeObject<LoginVm>(decrypted);

    //        var userDb = repo.GetUserById(loginVm.UserId);
    //        if (userDb == null)
    //        { throw new AuthenticationException("User Id not found: " + loginVm.UserId); }

    //        if (userDb.LoginToken != loginVm.UserToken)
    //        {
    //            throw new AuthenticationException("User Token not valid: " + loginVm.UserToken);
    //        }

    //        if (userDb.LoginExpiration < DateTime.UtcNow)
    //        {
    //            throw new AuthenticationException("User Token Expired: " + userDb.LoginExpiration);
    //        }

    //        return userDb;
    //    }
    //}
}