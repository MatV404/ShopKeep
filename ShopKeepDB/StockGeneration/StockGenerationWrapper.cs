using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Models;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.StockGeneration
{
    public class StockGenerationWrapper
    {
        private int _generationCount;
        private Shop _shop;

        public async Task<bool> GenerateStockAsync()
        {
            ItemGenerator generator = new ItemGenerator(_shop);
            return await generator.GenerateShopStock(_generationCount);
        }

        private int DetermineGenerationCount(string shopLocale)
        {
            Random randGen = new Random();
            switch (shopLocale)
            {
                default:
                    return randGen.Next(5, 10);
                case "Rural":
                    return randGen.Next(5, 20);
                case "Town":
                    return randGen.Next(10, 25);
                case "City":
                    return randGen.Next(15, 30);
                case "Metropolitan":
                    return randGen.Next(15, 35);
            }
        }

        public StockGenerationWrapper(Shop shop)
        {
            _shop = shop;
            _generationCount = DetermineGenerationCount(shop.Locale);
        }
    }
}
