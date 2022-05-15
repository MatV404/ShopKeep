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
        /// <summary>
        /// Removes item and every record attached to it.
        /// </summary>
        public static async Task<bool> DeleteItem(Item item)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Item.Remove(item);
                database.BaseItemPrice.Remove(item.BaseItemPrice);
                database.UserItem.RemoveRange(database.UserItem.Where(userItem => userItem.ItemId == item.Id));
                database.ShopStock.RemoveRange(database.ShopStock.Where(stock => stock.ItemId == item.Id));
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes items and every record attached to them.
        /// </summary>
        public static async Task<bool> DeleteItems(List<Item> items)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Item.RemoveRange(items);
                foreach (var item in items)
                {
                    database.BaseItemPrice.Remove(item.BaseItemPrice);
                    database.UserItem.RemoveRange(database.UserItem.Where(userItem => userItem.ItemId == item.Id));
                    database.ShopStock.RemoveRange(database.ShopStock.Where(stock => stock.ItemId == item.Id));
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
