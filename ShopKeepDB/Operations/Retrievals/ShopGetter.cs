using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Retrievals
{
    public static class ShopGetter
    {
        public static async Task<List<Shop>> GetAllShopsAsync()
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.Shop.Include(shop => shop.Type)
                                          .ToListAsync();
            }
            catch (ArgumentException)
            {
                return new List<Shop>();
            }
        }

        public static async Task<List<Shop>> FilterShopsAsync(string name, string owner, int? typeId, string locale)
        {
            try
            {
                await using var database = new ShopKeepContext();
                var shops = database.Shop.Include(shop => shop.Type)
                    .Where(shop => shop.Name.Contains(name)
                                   && shop.Owner.Contains(owner)
                                   && shop.TypeId == (typeId ?? shop.TypeId)
                                   && shop.Locale == (locale ?? shop.Locale));
                return await shops.ToListAsync();
            }
            catch (ArgumentException)
            {
                return new List<Shop>();
            }
        }
    }
}
