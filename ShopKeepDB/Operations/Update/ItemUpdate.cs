using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;

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
            catch (DbException)
            {
                return null;
            }
        }
    }
}
