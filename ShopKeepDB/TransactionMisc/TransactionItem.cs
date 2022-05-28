namespace ShopKeepDB.TransactionMisc
{
    /// <summary>
    /// General representation for an item that is to be included in a shop - user transaction.
    /// </summary>
    public class TransactionItem
    {
        public int TotalPriceGold { get; protected set; }
        public int TotalPriceSilver { get; protected set; }
        public int TotalPriceCopper { get; protected set; }
        public int Amount { get; protected set; }

        /// <summary>
        /// Sets the price for the object, normalizing it to fit with the standard of 10 copper = 1 silver; 10 silver = 1 gold.
        /// </summary>
        /// <param name="gold"></param>
        /// <param name="silver"></param>
        /// <param name="copper"></param>
        private void SetPricing(int gold, int silver, int copper)
        {
            int normalizedCopper = copper % 10;
            silver += copper / 10;

            int normalizedSilver = silver % 10;
            gold += silver / 10;

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
