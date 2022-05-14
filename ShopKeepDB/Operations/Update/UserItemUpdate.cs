using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Update
{
    public static class UserItemUpdate
    {
        public static async Task<bool> ChangeUserItemAmountAsync(UserItem item, int newAmount)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.UserItem.Update(item);
                item.Amount = newAmount;
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        public static async Task<bool> UserItemUpdateOrCreate(UserItem item)
        {
            //await using var database = new ShopKeepContext();
            //database.UserItem.Addra
            return true;
        }
    }
}
