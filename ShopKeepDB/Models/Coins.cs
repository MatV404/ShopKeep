namespace ShopKeepDB.Models
{
    public class Coins
    {
        public int Id { get; set; }
        public int Gold { get; set; } = 25;
        public int Silver { get; set; } = 0;
        public int Copper { get; set; } = 0;

        public User User { get; set; }
    }
}
