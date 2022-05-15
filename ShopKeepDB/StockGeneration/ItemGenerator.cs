using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopKeepDB.Misc;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Create;
using ShopKeepDB.Operations.Retrievals;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.StockGeneration
{
    public class ItemGenerator
    {
        private Dictionary<string, Tuple<int, int>> _chances;
        private int _minPricePercentageDifference;
        private int _maxPricePercentageDifference;
        private List<Item> _commonItems = null;
        private List<Item> _uncommonItems = null;
        private List<Item> _rareItems = null;
        private List<Item> _otherItems = null;
        private Shop _shop;
        private Random _generator;

        private List<Item> DetermineItemPool(string rarity)
        {
            return rarity switch
            {
                "Common" => _commonItems,
                "Uncommon" => _uncommonItems,
                "Rare" => _rareItems,
                _ => _otherItems
            };
        }

        private void GenerateShopStockItem(string rarity, List<ShopStock> shopStockList)
        {
            List<Item> itemPool = DetermineItemPool(rarity);
            if (itemPool.Count == 0)
            {
                return;
            }
            int itemIndex = _generator.Next(0, itemPool.Count - 1);
            Item item = itemPool[itemIndex];
            int amount = _generator.Next(1, 10);
            var matching = shopStockList.Where(stock => stock.ItemId == item.Id);
            if (matching.Any())
            {
                matching.First().Amount += amount;
                return;
            }
            ShopStockPrice price = PriceGenerator.GeneratePrice(_minPricePercentageDifference,
                _maxPricePercentageDifference,
                item.BaseItemPrice);
            var stock = new ShopStock(_shop.Id, item.Id, amount)
            {
                ShopStockPrice = price
            };
            shopStockList.Add(stock);
        }

        private string DetermineRarity(int percentage)
        {
            if (percentage <= _chances["Common"].Item2)
            {
                return "Common";
            }
            
            if (_chances["Uncommon"].Item1 <= percentage 
                && percentage <= _chances["Uncommon"].Item2)
            {
                return "Uncommon";
            }

            if (_chances["Rare"].Item1 <= percentage
                && percentage <= _chances["Rare"].Item2)
            {
                return "Rare";
            }

            return "Other";
        }

        public async Task<bool> GenerateShopStock(int generationSize)
        {
            List<ShopStock> generatedStock = new();
            _commonItems = await GetItemCollection("Common");
            _uncommonItems = await GetItemCollection("Uncommon");
            _rareItems = await GetItemCollection("Rare");
            _otherItems = await GetItemCollection("Other");
            for (int i = 0; i < generationSize; i++)
            {
                int rarityChance = _generator.Next(0, 100);
                string rarity = DetermineRarity(rarityChance);
                GenerateShopStockItem(rarity, generatedStock);
            }

            return await ShopStockCreator.CreateShopStockFromList(generatedStock);
        }

        private async Task<List<Item>> GetItemCollection(string rarity)
        {
            return await ItemGetter.FilterItemsAsync("", _shop.Type, rarity,
                int.MinValue, int.MaxValue,
                int.MinValue, int.MaxValue,
                int.MinValue, int.MaxValue);
        }
        
        public ItemGenerator(Shop shop)
        {
            _generator = new Random();
            _shop = shop;
            _chances = Constants.ItemChances[shop.Locale];
            _minPricePercentageDifference = Constants.PricePercentageDifferencePerLocale[_shop.Locale].Item1;
            _maxPricePercentageDifference = Constants.PricePercentageDifferencePerLocale[_shop.Locale].Item2;
        }
    }
}
