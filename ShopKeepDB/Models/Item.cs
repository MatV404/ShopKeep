using System.Collections.Generic;

namespace ShopKeepDB.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Rarity { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public int BaseItemPriceId { get; set; }
        public BaseItemPrice BaseItemPrice { get; set; } = null!;

        public List<ItemTypes> ItemTypes { get; set; } = new List<ItemTypes>();
        public Item() { }

        public Item(string name, string description, string rarity, BaseItemPrice price)
        {
            Name = name;
            Description = description ?? "";
            Rarity = rarity;
            BaseItemPrice = price;
            BaseItemPriceId = price.Id;
        }
    }
}
