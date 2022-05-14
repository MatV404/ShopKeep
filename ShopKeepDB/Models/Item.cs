using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ShopKeepDB.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rarity { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public int BaseItemPriceId { get; set; }
        public BaseItemPrice BaseItemPrice { get; set; }

        public List<ItemTypes> ItemTypes { get; set; } = new List<ItemTypes>();
        public Item() {}

        public Item(string name, string? description, string rarity, BaseItemPrice price)
        {
            Name = name;
            Description = description;
            Rarity = rarity;
            BaseItemPrice = price;
            BaseItemPriceId = price.Id;
        }
    }
}
