using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
