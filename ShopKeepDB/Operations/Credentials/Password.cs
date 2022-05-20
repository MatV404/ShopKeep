using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopKeepDB.Operations.Credentials
{
    public static class Password
    {
        /// <summary>
        /// Hashes and salts a password.
        /// </summary>
        /// <param name="plainText">The password in plaintext.</param>
        /// <returns>Tuple of (salt, password) in base64 format. 
        /// Please note, base64 does not provide any additional safety guarantees.</returns>
        public static Tuple<string, string> CreatePasswordHash(string plainText)
        {
            var salt = Hashing.GenerateSalt();
            if (salt == null)
            {
                return null;
            }
            var hashedPassword = Hashing.CreateHash(plainText, salt);
            if (hashedPassword == null)
            {
                return null;
            }
            var base64Salt = Convert.ToBase64String(salt);
            var base64HashedPassword = Convert.ToBase64String(hashedPassword);
            return new(base64Salt, base64HashedPassword);
        }

        public static bool ValidateUserPassword(string password, string salt, string hash)
        {
            var newHash = Hashing.CreateHash(password, Convert.FromBase64String(salt));
            var toCompare = Convert.FromBase64String(hash);
            return newHash != null && newHash.SequenceEqual(toCompare);
        }
    }
}
