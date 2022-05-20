using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using ShopKeepDB.TransactionMisc;

namespace ShopKeepDB.Operations.Create
{
    public static class ShopStockCreator
    {
        /// <summary>
        /// Creates a ShopStock entry (and the corresponding ShopStockPrice entry) in the database ShopStock table.
        /// </summary>
        /// <param name="shopId">The shop Id of the shop that this stock belongs to.</param>
        /// <param name="itemId">The item Id of the item that this stock represents.</param>
        /// <param name="stockAmount">The amount of this stock in the given shop.</param>
        /// <param name="goldPrice">The price of the stock in gold</param>
        /// <param name="silverPrice">The price of the stock in silver</param>
        /// <param name="copperPrice">The price of the stock in copper</param>
        /// <returns>The created ShopStock object on success or null on failure.</returns>
        public static async Task<ShopStock> CreateShopStock(int shopId, int itemId, int stockAmount, int goldPrice,
            int silverPrice, int copperPrice)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var price = await ShopStockPriceCreator.CreateShopStockPrice(goldPrice, silverPrice, copperPrice);
                if (price == null)
                {
                    return null;
                }
                var shopStock = new ShopStock(shopId, itemId, stockAmount, price.Id);
                await database.ShopStock.AddAsync(shopStock);
                await database.SaveChangesAsync();
                shopStock.ShopStockPrice = price;
                return shopStock;
            }
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates or updates the given ShopStock entries (and their corresponding ShopStockPrice objects) based on UserItem objects and a shop Id.
        /// </summary>
        /// <param name="itemList">A list of UserItems</param>
        /// <param name="shopId">The id of the given shop.</param>
        /// <returns>True if the operation succeeded, false on failure.</returns>
        public static async Task<bool> CreateOrUpdateShopStock(List<SaleItem> itemList, int shopId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                foreach (var item in itemList)
                {
                    var shopStock = await database.ShopStock.FindAsync(shopId, item.OriginalUserItem.ItemId);
                    if (shopStock == null)
                    {
                        ShopStockPrice price = new ShopStockPrice(item.OriginalUserItem.Item.BaseItemPrice.Gold,
                                                                  item.OriginalUserItem.Item.BaseItemPrice.Silver,
                                                                  item.OriginalUserItem.Item.BaseItemPrice.Copper);
                        ShopStock newStock = new ShopStock(shopId, item.OriginalUserItem.ItemId, item.Amount, price.Id);
                        newStock.ShopStockPrice = price;
                        await database.ShopStockPrice.AddAsync(price);
                        await database.ShopStock.AddAsync(newStock);
                    }
                    else
                    {
                        database.ShopStock.Update(shopStock);
                        shopStock.Amount += item.Amount;
                    }
                }

                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates ShopStock entries (and their corresponding ShopStockPrice objects) in the database from a list of ShopStock objects.
        /// </summary>
        /// <param name="stock">A list of ShopStock objects.</param>
        /// <returns></returns>
        public static async Task<bool> CreateShopStockFromList(List<ShopStock> stock)
        {
            try
            {
                await using var database = new ShopKeepContext();
                foreach (var item in stock)
                {
                    database.ShopStockPrice.Add(item.ShopStockPrice);
                    item.ShopStockPriceId = item.ShopStockPrice.Id;
                    database.ShopStock.Add(item);
                }
                await database.SaveChangesAsync();
                return true;
            }
            catch (Exception e) when (e is DbUpdateException or DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
