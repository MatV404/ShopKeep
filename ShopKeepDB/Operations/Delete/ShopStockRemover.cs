using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Delete
{
    public static class ShopStockRemover
    {
        public static async Task<bool> RemoveShopStockAsync(ShopStock toDelete)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStock.Remove(toDelete);
                database.ShopStockPrice.Remove(toDelete.ShopStockPrice);
                await database.SaveChangesAsync();
                return true;
            }
            catch (DbException)
            {
                return false;
            }
        }

        public static async Task<bool> RemoveShopStockAsync(List<ShopStock> toDelete)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStock.RemoveRange(toDelete);
                toDelete.ForEach(stock => database.ShopStockPrice.Remove(stock.ShopStockPrice));
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
