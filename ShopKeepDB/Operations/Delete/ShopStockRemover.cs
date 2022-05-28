using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Delete
{
    public static class ShopStockRemover
    {
        /// <summary>
        /// Removes a single ShopStock database entry and the ShopStockPrice associated with it.
        /// </summary>
        /// <param name="toDelete"> The ShopStock that will be removed from the database. </param>
        /// <returns>True if all went well, false on a thrown exception.</returns>
        public static async Task<bool> RemoveShopStockAsync(ShopStock toDelete)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStock.Remove(toDelete);
                database.ShopStockPrice.Remove(toDelete.ShopStockPrice);
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

        /// <summary>
        /// Removes all the ShopStock objects (and their ShopStockPrice fields) in the toDelete list from the database.
        /// </summary>
        /// <param name="toDelete"></param>
        /// <returns>True if everything went well, false on a thrown exception. </returns>
        public static async Task<bool> RemoveShopStockAsync(List<ShopStock> toDelete)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.ShopStock.RemoveRange(toDelete);
                toDelete.ForEach(stock => database.ShopStockPrice.Remove(stock.ShopStockPrice));
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
