using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Operations.Create
{
    public static class ShopCreator
    {
        public static async Task<Shop> CreateShopAsync(string shopName, string ownerName, int shopTypeId,
            string shopLocale, string shopDescription)
        {
            try
            {
                await using var database = new ShopKeepContext();
                Shop newShop = new Shop(shopName, shopLocale, shopDescription, ownerName, shopTypeId);
                await database.Shop.AddAsync(newShop);
                await database.SaveChangesAsync();
                return newShop;
            }
            catch (DbException)
            {
                return null;
            }

        }
    }
}
