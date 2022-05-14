using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Create
{
    public static class ShopStockPriceCreator
    {
        public static async Task<ShopStockPrice> CreateShopStockPrice(int gold, int silver, int copper)
        {
            try
            {
                await using var database = new ShopKeepContext();
                ShopStockPrice newPrice = new ShopStockPrice(gold, silver, copper);
                await database.ShopStockPrice.AddAsync(newPrice);
                await database.SaveChangesAsync();
                return newPrice;
            }
            catch (DbException)
            {
                return null;
            }
        }
    }
}
