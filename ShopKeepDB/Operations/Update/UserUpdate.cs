using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                var hashResult = Password.CreatePasswordHash(password);
                if (hashResult == null)
                {
                    return false;
                }
                var salt = hashResult.Item1;
                var hashedPassword = hashResult.Item2;
                await using var database = new ShopKeepContext();
                database.User.Update(user);
                user.Password = hashedPassword;
                user.Salt = salt;
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception e) when (e is DbUpdateException 
                                        or DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
