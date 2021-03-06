using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Create
{
    public static class UserItemCreator
    {
        public static async Task<bool> CreateOrUpdateUserItems(List<UserItem> items)
        {
            try
            {
                await using var database = new ShopKeepContext();
                foreach (var item in items)
                {
                    var userItem = await database.UserItem.FindAsync(item.ItemId, item.UserName);
                    if (userItem == null)
                    {
                        await database.UserItem.AddAsync(item);
                    }
                    else
                    {
                        database.UserItem.Update(userItem);
                        userItem.Amount += item.Amount;
                    }
                }

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
