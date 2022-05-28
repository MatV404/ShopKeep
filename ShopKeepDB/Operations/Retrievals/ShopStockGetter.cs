using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class ShopStockGetter
    {
        public static async Task<List<ShopStock>> GetShopStock(int shopId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.ShopStock.Include(stock => stock.ShopStockPrice)
                                               .Include(stock => stock.Item)
                                               .Where(stock => stock.ShopId == shopId)
                                               .ToListAsync();
            }
            catch (ArgumentException)
            {
                return new List<ShopStock>();
            }
        }

        public static async Task<bool> ShopStockExistsAsync(int shopId, int itemId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.ShopStock.FindAsync(shopId, itemId) != null;

            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
