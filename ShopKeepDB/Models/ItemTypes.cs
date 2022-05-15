using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopKeepDB.Models
{
    public class ItemTypes
    {
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public Type Type { get; set; } = null!;

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public ItemTypes(){}

        public ItemTypes(Type type, Item item)
        {
            Type = type;
            Item = item;
        }
    }
}
