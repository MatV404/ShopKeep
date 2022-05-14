using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Retrievals;

namespace ShopKeepDB.Operations.Delete
{
    public static class ShopDestroyer
    {
        public static async Task<bool> DeleteShopAsync(Shop shop)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Shop.Remove(shop);
                var allStock = await ShopStockGetter.GetShopStock(shop.Id);
                database.ShopStock.RemoveRange(allStock);
                foreach (var stock in allStock)
                {
                    database.ShopStockPrice.Remove(stock.ShopStockPrice);
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
