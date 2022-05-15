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
            ItemGenerator generator = new(_shop);
            return await generator.GenerateShopStock(_generationCount);
        }

        private int DetermineGenerationCount(string shopLocale)
        {
            Random randGen = new();
            return shopLocale switch
            {
                "Rural" => randGen.Next(5, 20),
                "Town" => randGen.Next(10, 30),
                "City" => randGen.Next(15, 40),
                "Metropolitan" => randGen.Next(20, 50),
                _ => randGen.Next(5, 10)
            };
        }

        public StockGenerationWrapper(Shop shop)
        {
            _shop = shop;
            _generationCount = DetermineGenerationCount(shop.Locale);
        }
    }
}
