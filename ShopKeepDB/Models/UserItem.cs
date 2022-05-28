namespace ShopKeepDB.Models
{
    public class UserItem
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }
        public int Amount { get; set; }

        public UserItem(int itemId, string userName, int amount)
        {
            ItemId = itemId;
            UserName = userName;
            Amount = amount;
        }

        public UserItem() { }
    }
}
