using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public static class TypeGetter
    {
        public static async Task<List<Type>> GetAllTypesAsync()
        {
            try
            {
                await using var database = new ShopKeepContext();
                return await database.Type.ToListAsync();
            }
            catch (DbException)
            {
                return null;
            }
        }

        public static async Task<List<Type>> GetAllTypesByIdAsync(List<int> typeIds)
        {
            await using var database = new ShopKeepContext();
            return await database.Type.Where(type => typeIds.Contains(type.Id)).ToListAsync();
        }
    }
}
