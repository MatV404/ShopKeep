using System;
using System.Security.Cryptography;

namespace ShopKeepDB.Operations.Credentials
{
    public static class Hashing
    {
        public static byte[] CreateHash(string password, byte[] salt)
        {
            try
            {
                using var hasher = new Rfc2898DeriveBytes(password, salt, Misc.Constants.IterationCount);
                return hasher.GetBytes(Misc.Constants.HashSize);
            }
            catch (Exception e) when (e is ArgumentException
                                        or ArgumentNullException
                                        or ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public static byte[] GenerateSalt()
        {
            try
            {
                byte[] salt;
                using var random = new RNGCryptoServiceProvider();
                random.GetBytes(salt = new byte[Misc.Constants.SaltSize]);
                return salt;
            }
            catch (Exception e) when (e is CryptographicException
                                        or ArgumentNullException)
            {
                return null;
            }

        }
    }
}
