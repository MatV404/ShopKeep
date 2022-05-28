using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Create
{
    public static class ShopCreator
    {
        /// <summary>
        /// Creates a shop entry in the Shop table of the database.
        /// </summary>
        /// <param name="shopName">Name of the shop</param>
        /// <param name="ownerName">Name of the shop's owner</param>
        /// <param name="shopTypeId">Id of the shop's Type</param>
        /// <param name="shopLocale">Shop locale (see constants)</param>
        /// <param name="shopDescription">Description of the shop.</param>
        /// <returns>Shop if all went well, null if an exception occurred.</returns>
        public static async Task<Shop> CreateShopAsync(string shopName, string ownerName, int shopTypeId,
            string shopLocale, string shopDescription)
        {
            try
            {
                await using var database = new ShopKeepContext();
                Shop newShop = new(shopName, shopLocale, shopDescription, ownerName, shopTypeId);
                await database.Shop.AddAsync(newShop);
                await database.SaveChangesAsync();
                return newShop;
            }
            catch (Exception e) when (e is ArgumentException 
                                        or DbUpdateException 
                                        or DbUpdateConcurrencyException)
            {
                return null;
            }

        }
    }
}
