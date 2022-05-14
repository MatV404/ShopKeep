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
        public Type Type { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        public ItemTypes(){}

        public ItemTypes(Type type, Item item)
        {
            Type = type;
            Item = item;
        }
    }
}
