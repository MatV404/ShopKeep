using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

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
            catch (DbException)
            {
                return false;
            }
        }
    }
}
