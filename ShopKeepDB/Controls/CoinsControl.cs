using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Update;

namespace ShopKeepDB.Controls
{
    public static class CoinsControl
    {
        public static async Task<bool> UpdateUserCoinsAsync(User user, int gold, int silver, int copper)
        {
            if (user.Coins == null)
            {
                return false;
            }
            int copperCoins = user.Coins.Copper;
            int silverCoins = user.Coins.Silver;
            int goldCoins = user.Coins.Gold;

            copperCoins += copper;
            if (copperCoins > 10)
            {
                silverCoins += copperCoins / 10;
                copperCoins %= 10;
            }

            if (copperCoins < 0)
            {
                silverCoins -= (-1 * copperCoins) / 10;
                copperCoins = 10 - copperCoins % 10;
            }

            silverCoins += silver;

            if (silverCoins > 10)
            {
                goldCoins += silverCoins / 10;
                silverCoins %= 10;
            }

            if (silverCoins < 0)
            {
                goldCoins -= (-1 * silverCoins) / 10;
                silverCoins = 10 - silverCoins % 10;
            }

            goldCoins += gold;

            if (goldCoins < 0)
            {
                goldCoins = 0;
            }

            return await CoinsUpdate.UpdateCoins(user.Coins, goldCoins, silverCoins, copperCoins);
        }
    }
}
