using System.ComponentModel.DataAnnotations;

namespace ShopKeepDB.Models
{
    public class BaseItemPrice
    {
        public int Id { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Copper { get; set; }
        public BaseItemPrice() { }

        public BaseItemPrice(int gold, int silver, int copper)
        {
            Gold = gold;
            Silver = silver;
            Copper = copper;
        }
    }
}
