using System;
using System.Collections.Generic;
using System.Text;
using ShopKeepDB.Models;

namespace ShopKeepDB.StockGeneration
{
    internal static class PriceGenerator
    {
        private static int GetPercentage(int value, int percentage)
        {
            return (value / 100) * percentage;
        }
        public static ShopStockPrice GeneratePrice(int minPricePercentage, int maxPricePercentage,
            BaseItemPrice basePrice)
        {
            Random random = new Random();
            int minGold = basePrice.Gold - GetPercentage(basePrice.Gold, minPricePercentage);
            int maxGold = basePrice.Gold + GetPercentage(basePrice.Gold, maxPricePercentage);

            if (minGold < 0)
            {
                minGold = 0;
            }

            int minSilver = basePrice.Silver - GetPercentage(basePrice.Silver, minPricePercentage);
            int maxSilver = basePrice.Silver + GetPercentage(basePrice.Silver, maxPricePercentage);
            
            if (minSilver < 0)
            {
                minSilver = 0;
            }

            if (maxSilver >= 10)
            {
                maxSilver = 9;
            }

            int minCopper = basePrice.Copper - GetPercentage(basePrice.Copper, minPricePercentage);
            int maxCopper = basePrice.Copper + GetPercentage(basePrice.Copper, maxPricePercentage);
            
            if (minCopper < 0)
            {
                minCopper = 0;
            }

            if (maxCopper > 10)
            {
                minCopper = 10;
            }

            int goldValue = random.Next(minGold, maxGold);
            int silverValue = random.Next(minSilver, maxSilver);
            int copperValue = random.Next(minCopper, maxCopper);
            return new ShopStockPrice(goldValue, silverValue, copperValue);
        }
    }
}
