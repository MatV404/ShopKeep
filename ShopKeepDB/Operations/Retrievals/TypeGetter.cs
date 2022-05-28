using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            catch (ArgumentException)
            {
                return new List<Type>();
            }
        }

        public static async Task<List<Type>> GetAllTypesByIdAsync(List<int> typeIds)
        {
            try {
                await using var database = new ShopKeepContext();
                return await database.Type.Where(type => typeIds.Contains(type.Id)).ToListAsync();
            }
            catch (ArgumentException)
            {
                return new List<Type>();
            }
        }
    }
}
