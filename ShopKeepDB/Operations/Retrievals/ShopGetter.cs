using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class ShopGetter
    {
        public static async Task<List<Shop>> GetAllShopsAsync()
        {
            await using var database = new ShopKeepContext();
            return await database.Shop.Include(shop => shop.Type)
                                      .ToListAsync();
        }

        public static async Task<List<Shop>> FilterShopsAsync(string name, string owner, int? typeId, string locale)
        {
            await using var database = new ShopKeepContext();
            var shops = database.Shop.Include(shop => shop.Type)
                .Where(shop => shop.Name.Contains(name)
                               && shop.Owner.Contains(owner)
                               && shop.TypeId == (typeId ?? shop.TypeId)
                               && shop.Locale == (locale ?? shop.Locale));
            return await shops.ToListAsync();
        }
    }
}
