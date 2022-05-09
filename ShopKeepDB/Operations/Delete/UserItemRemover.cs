using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Delete
{
    public static class UserItemRemover
    {
        public static async Task<bool> DeleteUserItemsAsync(List<UserItem> userItems)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.RemoveRange(userItems);
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
