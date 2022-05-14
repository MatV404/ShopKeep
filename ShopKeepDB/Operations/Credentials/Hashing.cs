using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Security.Policy;

namespace ShopKeepDB.Operations.Credentials
{
    public static class Hashing
    {
        public static byte[] CreateHash(string password, byte[] salt)
        {
            using var hasher = new Rfc2898DeriveBytes(password, salt, Misc.Constants.IterationCount);
            return hasher.GetBytes(Misc.Constants.HashSize);
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt;
            using var random = new RNGCryptoServiceProvider();
            random.GetBytes(salt = new byte[Misc.Constants.SaltSize]);
            return salt;
        }
    }
}
