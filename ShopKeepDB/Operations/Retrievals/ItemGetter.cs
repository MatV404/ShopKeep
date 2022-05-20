using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class ItemGetter
    {
        public static async Task<List<Item>> GetAllItemsAsync()
        {
            await using var database = new ShopKeepContext();
            return await database.Item.Include(item => item.BaseItemPrice)
                                      .Include(item => item.ItemTypes)
                                      .ToListAsync();
        }
        
        public static async Task<List<Item>> FilterItemsAsync(string name, Type itemType, string itemRarity, 
            int goldMin, int goldMax, int silverMin, 
            int silverMax, int copperMin, int copperMax) 
        {
            await using var database = new ShopKeepContext();
            var items = database.Item.Include(item => item.BaseItemPrice)
                .Include(item => item.ItemTypes)
                .Where(item => item.Name.Contains(name)
                               && item.ItemTypes.Any(type => type.TypeId == (itemType == null ? type.TypeId : itemType.Id))
                               && item.Rarity == (string.IsNullOrWhiteSpace(itemRarity) ? item.Rarity : itemRarity))
                .Where(item => item.BaseItemPrice.Copper >= copperMin && item.BaseItemPrice.Copper <= copperMax
                                                                      && item.BaseItemPrice.Silver >= silverMin 
                                                                      && item.BaseItemPrice.Silver <= silverMax
                                                                      && item.BaseItemPrice.Gold >= goldMin 
                                                                      && item.BaseItemPrice.Gold <= goldMax);
            return await items.ToListAsync();
        }


        public static async Task<Item> RetrieveItem(int itemId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var item = await database.Item.FindAsync(itemId);
                return item;
            }
            catch (DbException)
            {
                return null;
            }
        }
    }
}
