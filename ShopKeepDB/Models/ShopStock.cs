namespace ShopKeepDB.Models
{
    public class ShopStock
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int Amount { get; set; }
        public int ShopStockPriceId { get; set; }
        public ShopStockPrice ShopStockPrice { get; set; }

        public ShopStock(int shopId, int itemId, int amount, int priceId)
        {
            ShopId = shopId;
            ItemId = itemId;
            ShopStockPriceId = priceId;
            Amount = amount;
        }

        public ShopStock(int shopId, int itemId, int amount)
        {
            ShopId = shopId;
            ItemId = itemId;
            Amount = amount;
        }

        public ShopStock() { }
    }
}
