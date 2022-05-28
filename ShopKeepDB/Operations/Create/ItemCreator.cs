using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Create
{
    public static class ItemCreator
    {

        /// <summary>
        /// Creates a given item, along with the BaseItemPrice table entry associated with it.
        /// </summary>
        /// <param name="itemName">Name of the item</param>
        /// <param name="itemRarity">Rarity of the item</param>
        /// <param name="itemDescription">Description of the item</param>
        /// <param name="goldPrice">Price of the item in Gold</param>
        /// <param name="silverPrice">Price of the item in Silver</param>
        /// <param name="copperPrice">Price of the item in Copper</param>
        /// <param name="itemTypes">A list of Types the item should fall under.</param>
        /// <returns>True if no problem occurs, false on an exception thrown.</returns>
        public static async Task<bool> CreateItemAsync(string itemName, string itemRarity, string itemDescription, int goldPrice,
                                                       int silverPrice, int copperPrice, List<Type> itemTypes)
        {
            try
            {
                await using var database = new ShopKeepContext();
                BaseItemPrice newPrice = new(goldPrice, copperPrice, silverPrice);
                Item newItem = new(itemName, itemDescription, itemRarity, newPrice);
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
            catch (Exception e) when (e is ArgumentException 
                                        or DbUpdateException 
                                        or DbUpdateConcurrencyException)
            {
                return false;
            }

        }
    }
}
