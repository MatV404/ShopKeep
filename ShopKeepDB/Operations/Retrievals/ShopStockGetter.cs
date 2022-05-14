using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

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
            catch (DbException)
            {
                return new List<ShopStock>();
            }
        }

        public static async Task<bool> ShopStockExistsAsync(int shopId, int itemId)
        {
            await using var database = new ShopKeepContext();
            return await database.ShopStock.FindAsync(shopId, itemId) != null;
        }
    }
}
