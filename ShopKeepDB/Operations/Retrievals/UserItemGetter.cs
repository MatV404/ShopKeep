using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class UserItemGetter
    {
        public static async Task<List<UserItem>> GetUserItems(int userId)
        {
            await using var database = new ShopKeepContext();
            return await database.UserItem.Include(item => item.Item)
                                          .Where(item => item.UserId == userId)
                                          .ToListAsync();
        }

        public static async Task<List<UserItem>> FilterUserItems(int userId, string itemName,
                                                                 string itemRarity, int? itemTypeId)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var items = database.UserItem.Include(item => item.Item)
                    .Where(item =>
                        item.UserId == userId 
                        && item.Item.Name.Contains(itemName) 
                        && item.Item.Rarity == (itemRarity ?? item.Item.Rarity));
                if (itemTypeId != null)
                {
                    var itemsByType = database.ItemTypes.Where(itemType => itemType.TypeId == itemTypeId).Select(itemType => itemType.ItemId);
                    items = items.Include(item => item.Item).Where(item => itemsByType.Contains(item.ItemId));
                }
                return await items.ToListAsync();
            }
            catch (DbException)
            {
                return null;
            }
        }
    }
}
