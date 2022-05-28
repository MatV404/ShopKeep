using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Threading.Tasks;

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
            catch (Exception e) when (e is ArgumentException 
                                        or DbUpdateException 
                                        or DbUpdateConcurrencyException )
            {
                return null;
            }
        }
    }
}
