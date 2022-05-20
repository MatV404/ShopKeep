using System;
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
                    
                    var hashResult = Password.CreatePasswordHash(password);
                    
                    if (hashResult == null)
                    {
                        return RegistrationResults.RegistrationFailure;
                    }

                    var salt = hashResult.Item1;
                    var hashedPass = hashResult.Item2;
                    User newUser = new(username, hashedPass, salt);

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
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return RegistrationResults.DbError;
            }
        }
    }
}
