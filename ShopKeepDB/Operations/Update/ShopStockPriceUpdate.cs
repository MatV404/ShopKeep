using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Update
{
    public static class ShopStockPriceUpdate
    {
        public static async Task<bool> UpdateShopStockPriceAsync(ShopStockPrice price, int gold, int silver, int copper)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStockPrice.Update(price);
                price.Gold = gold;
                price.Silver = silver;
                price.Copper = copper;
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
