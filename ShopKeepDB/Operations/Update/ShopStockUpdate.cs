using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Update
{
    public static class ShopStockUpdate
    {
        public static async Task<bool> ChangeShopStockAmountAsync(ShopStock stock, int newAmount)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStock.Update(stock);
                stock.Amount = newAmount;
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
