using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Delete
{
    public static class ItemDestroyer
    {
        public static async Task<bool> DeleteItem(Item item)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Item.Remove(item);
                database.BaseItemPrice.Remove(item.BaseItemPrice);
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteItems(List<Item> items)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Item.RemoveRange(items);
                foreach (var item in items)
                {
                    database.BaseItemPrice.Remove(item.BaseItemPrice);
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
