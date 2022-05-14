using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

namespace ShopKeepDB.Operations.Create
{
    public static class ShopStockCreator
    {
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
            catch (DbException)
            {
                return null;
            }
        }

        public static async Task<bool> CreateOrUpdateShopStock(List<UserItem> itemList, int shopId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                foreach (var item in itemList)
                {
                    var shopStock = await database.ShopStock.FindAsync(shopId, item.ItemId);
                    if (shopStock == null)
                    {
                        ShopStockPrice price = new ShopStockPrice(item.Item.BaseItemPrice.Gold,
                                                                  item.Item.BaseItemPrice.Silver,
                                                                  item.Item.BaseItemPrice.Copper);
                        ShopStock newStock = new ShopStock(shopId, item.ItemId, item.Amount, price.Id);
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
            catch (DbException)
            {
                return false;
            }
        }

        public static async Task<bool> CreateShopStockFromList(List<ShopStock> stock)
        {
            try
            {
                await using var database = new ShopKeepContext();
                foreach (ShopStock item in stock)
                {
                    database.ShopStockPrice.Add(item.ShopStockPrice);
                    item.ShopStockPriceId = item.ShopStockPrice.Id;
                    database.ShopStock.Add(item);
                }
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
