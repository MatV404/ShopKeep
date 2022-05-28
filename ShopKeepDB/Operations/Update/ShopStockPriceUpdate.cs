using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Threading.Tasks;

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
            catch (Exception e) when (e is ArgumentException
                                        or DbUpdateException
                                        or DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
