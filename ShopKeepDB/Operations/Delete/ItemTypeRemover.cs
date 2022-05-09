using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;

namespace ShopKeepDB.Operations.Delete
{
    public class ItemTypeRemover
    {
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
            catch (DbException)
            {
                return false;
            }
        }
    }
}
