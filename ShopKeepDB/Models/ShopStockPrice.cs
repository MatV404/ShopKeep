using System;
using System.Collections.Generic;
using System.Text;

namespace ShopKeepDB.Models
{
    public class ShopStockPrice
    {
        public int Id { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Copper { get; set; }

        public ShopStockPrice(int gold, int silver, int copper)
        {
            Gold = gold;
            Silver = silver;
            Copper = copper;
        }

        public ShopStockPrice()  { }
    }
}
