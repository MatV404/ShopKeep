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
    }
}
