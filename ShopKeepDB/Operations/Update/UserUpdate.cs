using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Credentials;

namespace ShopKeepDB.Operations.Update
{
    public static class UserUpdate
    {
        public static async Task<bool> ChangeUserBanState(User user, bool isUnban)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.User.Update(user);
                user.IsActive = isUnban;
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        public static async Task<bool> ChangeUserPassword(User user, string password)
        {
            try
            {
                var hashedPass = Hashing.CreateHash(password, Convert.FromBase64String(user.Salt));
                var hashedPassString = Convert.ToBase64String(hashedPass);
                await using var database = new ShopKeepContext();
                database.User.Update(user);
                user.Password = hashedPassString;
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }
    }
}
