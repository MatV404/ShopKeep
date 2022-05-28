using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Delete
{
    public class ItemTypeRemover
    {
        /// <summary>
        /// Removes ItemType entries for the given item Id and list of type Ids
        /// </summary>
        /// <param name="itemId">The id of the item</param>
        /// <param name="typeIds">The id of the types you wish to remove</param>
        /// <returns>True if all went well, false on failure.</returns>
        public static async Task<bool> RemoveItemTypes(int itemId, List<int> typeIds)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var itemTypes = database.ItemTypes.Where(itemType => typeIds.Contains(itemType.TypeId)
                                                                     && itemType.ItemId == itemId);
                database.ItemTypes.RemoveRange(itemTypes);
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
