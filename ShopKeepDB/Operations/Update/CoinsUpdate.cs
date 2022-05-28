using Microsoft.EntityFrameworkCore;
using ShopKeepDB.Context;
using ShopKeepDB.Models;
using System;
using System.Threading.Tasks;

namespace ShopKeepDB.Operations.Update
{
    public static class CoinsUpdate
    {
        public static async Task<bool> UpdateCoins(Coins coins, int goldValue, int silverValue, int copperValue)
        {
            try
            {
                await using var database = new ShopKeepContext();
                database.Coins.Update(coins);
                if (coins.Gold != goldValue)
                {
                    coins.Gold = goldValue;
                }

                if (coins.Silver != silverValue)
                {
                    coins.Silver = silverValue;
                }

                if (coins.Copper != copperValue)
                {
                    coins.Copper = copperValue;
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
