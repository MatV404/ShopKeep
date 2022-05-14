using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Credentials
{
    public static class Register
    {
        public static async Task<RegistrationResults> RegisterAsync(string username, string password)
        {
            try
            {
                await using var database = new ShopKeepContext();
                try
                {
                    if (await database.User.FindAsync(username) != null)
                    {
                        return RegistrationResults.RegistrationFailure;
                    }
                    var salt = Hashing.GenerateSalt();
                    var hashedPass = Hashing.CreateHash(password, salt);
                    string convertedPass = Convert.ToBase64String(hashedPass);
                    string convertedSalt = Convert.ToBase64String(salt);
                    User newUser = new(username, convertedPass, convertedSalt);
                    var coinsQuery = await database.Coins.AddAsync(new Coins());
                    newUser.Coins = coinsQuery.Entity;
                    var userQuery = await database.User.AddAsync(newUser);
                    await database.SaveChangesAsync();
                    return RegistrationResults.RegistrationSuccess;
                }
                catch (TaskCanceledException)
                {
                    return RegistrationResults.RegistrationFailure;
                }
            }
            catch (DbException)
            {
                return RegistrationResults.DbError;
            }
        }
    }
}
