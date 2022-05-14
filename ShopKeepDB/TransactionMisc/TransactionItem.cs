using System;
using System.Collections.Generic;
using System.Text;

namespace ShopKeepDB.TransactionMisc
{
    public class TransactionItem
    {
        public int TotalPriceGold { get; protected set; }
        public int TotalPriceSilver { get; protected set; }
        public int TotalPriceCopper { get; protected set; }
        public int Amount { get; protected set; }

        private void SetPricing(int gold, int silver, int copper)
        {
            int normalizedCopper = copper % 10;
            silver = silver + copper / 10;

            int normalizedSilver = silver % 10;
            gold = gold + silver / 10;

            TotalPriceGold = gold;
            TotalPriceSilver = normalizedSilver;
            TotalPriceCopper = normalizedCopper;
        }

        public TransactionItem(int amount, int gold, int silver, int copper)
        {
            Amount = amount;
            SetPricing(gold * amount, silver * amount, copper * amount);
        }
    }
}
