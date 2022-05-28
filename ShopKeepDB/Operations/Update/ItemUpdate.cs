using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Update
{
    public class ItemUpdate
    {
        public static async Task<Item> UpdateItemAsync(Item item, string itemName, string itemRarity,
            string itemDescription, int goldPrice, int silverPrice, int copperPrice)

        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Item.Update(item);
                database.BaseItemPrice.Update(item.BaseItemPrice);
                if (item.Name != itemName)
                {
                    item.Name = itemName;
                }

                if (item.Rarity != itemRarity)
                {
                    item.Rarity = itemRarity;
                }

                if (item.Description != itemDescription)
                {
                    item.Description = itemDescription;
                }
                if (item.BaseItemPrice.Gold != goldPrice)
                {
                    item.BaseItemPrice.Gold = goldPrice;
                }

                if (item.BaseItemPrice.Silver != silverPrice)
                {
                    item.BaseItemPrice.Silver = silverPrice;
                }

                if (item.BaseItemPrice.Copper != copperPrice)
                {
                    item.BaseItemPrice.Copper = copperPrice;
                }

                await database.SaveChangesAsync();
                return item;
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
