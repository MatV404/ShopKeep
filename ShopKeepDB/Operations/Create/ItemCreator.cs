using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Create
{
    public static class ItemCreator
    {
        
        public static async Task<bool> CreateItemAsync(string itemName, string itemRarity, string? itemDescription, int goldPrice, 
                                                       int silverPrice, int copperPrice, List<Type> itemTypes)
        {
            try
            {
                await using var database = new ShopKeepContext();
                BaseItemPrice newPrice = new BaseItemPrice(goldPrice, copperPrice, silverPrice);
                Item newItem = new Item(itemName, itemDescription, itemRarity, newPrice);
                await database.BaseItemPrice.AddAsync(newPrice);
                await database.Item.AddAsync(newItem);
                foreach (var itemType in itemTypes)
                {
                    database.Type.Attach(itemType);
                    var itemTypeBinding = new ItemTypes(itemType, newItem);
                    newItem.ItemTypes.Add(itemTypeBinding);
                    await database.ItemTypes.AddAsync(itemTypeBinding);
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
