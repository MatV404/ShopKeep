using System;
using System.Collections.Generic;
using System.Text;

namespace ShopKeepDB.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
        public Type Type { get; set; }
    }
}
