using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Retrievals;

namespace ShopKeepDB.Operations.Delete
{
    public static class ShopDestroyer
    {
        /// <summary>
        /// Deletes the given shop and all of its ShopStock items along with their prices.
        /// </summary>
        /// <param name="shop">The shop object that will be removed from the database.</param>
        /// <returns>True if everything was successful, else false.</returns>
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
            catch (Exception e) when (e is DbUpdateConcurrencyException or DbUpdateException)
            {
                return false;
            }
        }
    }
}
