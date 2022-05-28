namespace ShopKeepDB.Models
{
    public class ItemTypes
    {
        public int TypeId { get; set; }
        public Type Type { get; set; } = null!;
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public ItemTypes() { }

        public ItemTypes(Type type, Item item)
        {
            Type = type;
            Item = item;
        }
    }
}
