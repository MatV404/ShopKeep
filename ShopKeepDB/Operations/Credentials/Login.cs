using System;
using System.Linq;
using System.Configuration;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Credentials
{
    public static class Login
    {
        public static async Task<Tuple<LoginResults, User>> ValidateLoginAsync(string username, string password)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var query = await database.User.Include(user => user.Coins)
                                                    .FirstOrDefaultAsync(user => user.Name == username && user.Password == password);

                if (query == null)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Invalid, null);
                }

                if (!query.IsActive)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Banned, null);
                }

                if (query.IsAdmin)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Admin, query);
                }

                return new Tuple<LoginResults, User>(LoginResults.User, query);
            }
            catch (DbException)
            {
                return new Tuple<LoginResults, User>(LoginResults.DbError, null);
            }
        }
    }
}
