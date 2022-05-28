using ShopKeepDB.Models;
using System;

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
            var random = new Random();
            var minGold = basePrice.Gold - GetPercentage(basePrice.Gold, minPricePercentage);
            var maxGold = basePrice.Gold + GetPercentage(basePrice.Gold, maxPricePercentage);

            if (minGold < 0)
            {
                minGold = 0;
            }

            var minSilver = basePrice.Silver - GetPercentage(basePrice.Silver, minPricePercentage);
            var maxSilver = basePrice.Silver + GetPercentage(basePrice.Silver, maxPricePercentage);

            if (minSilver < 0)
            {
                minSilver = 0;
            }

            if (maxSilver >= 10)
            {
                maxSilver = 9;
            }

            var minCopper = basePrice.Copper - GetPercentage(basePrice.Copper, minPricePercentage);
            var maxCopper = basePrice.Copper + GetPercentage(basePrice.Copper, maxPricePercentage);

            if (minCopper < 0)
            {
                minCopper = 0;
            }

            if (maxCopper > 10)
            {
                minCopper = 10;
            }

            var goldValue = random.Next(minGold, maxGold);
            var silverValue = random.Next(minSilver, maxSilver);
            var copperValue = random.Next(minCopper, maxCopper);
            return new ShopStockPrice(goldValue, silverValue, copperValue);
        }
    }
}
