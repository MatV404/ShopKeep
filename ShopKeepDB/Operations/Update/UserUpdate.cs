using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

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
    }
}
