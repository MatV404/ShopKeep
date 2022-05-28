using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class UserGetter
    {
        public static async Task<List<User>> GetAllRegularUsers()
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.User.Include(user => user.Coins)
                                     .Where(user => !user.IsAdmin)
                                     .ToListAsync();
            }
            catch (Exception e) when (e is ArgumentNullException
                                        or ArgumentException)
            {
                return null;
            }
        }

        public static async Task<List<User>> FilterUsers(string userName)
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.User.Include(user => user.Coins)
                                          .Where(user => user.Name.Contains(userName))
                                          .ToListAsync();
            }
            catch (Exception e) when (e is ArgumentNullException
                                        or ArgumentException)
            {
                return null;
            }
        }

        public static async Task<User> GetUserByUsername(string userName)
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.User.Include(user => user.Coins)
                                          .Where(user => user.Name == userName)
                                          .FirstOrDefaultAsync();
            }
            catch (Exception e) when (e is ArgumentNullException
                                        or ArgumentException)
            {
                return null;
            }
        }
    }
}
