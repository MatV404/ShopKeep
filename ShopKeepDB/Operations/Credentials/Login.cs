using System;
using System.Data.Common;
using System.Threading.Tasks;
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
                await database.Database.EnsureCreatedAsync();
                var user = await Retrievals.UserGetter.GetUserByUsername(username);

                if (user == null)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Invalid, null);
                } 

                if (!Password.ValidateUserPassword(password, user.Salt, user.Password))
                {
                    return new Tuple<LoginResults, User>(LoginResults.Invalid, null);
                }

                if (!user.IsActive)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Banned, null);
                }

                if (user.IsAdmin)
                {
                    return new Tuple<LoginResults, User>(LoginResults.Admin, user);
                }

                return new Tuple<LoginResults, User>(LoginResults.User, user);
            }
            catch (DbException)
            {
                return new Tuple<LoginResults, User>(LoginResults.DbError, null);
            }
        }
    }
}
