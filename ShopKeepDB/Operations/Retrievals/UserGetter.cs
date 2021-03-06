using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

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
            catch (ArgumentNullException)
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
            catch (ArgumentNullException)
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
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}
