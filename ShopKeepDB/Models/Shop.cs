namespace ShopKeepDB.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Locale { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int TypeId { get; set; }
        public Type Type { get; set; }

        public Shop() { }

        public Shop(string name, string locale, string description,
            string owner, int typeId)
        {
            Name = name;
            Locale = locale;
            Description = description;
            Owner = owner;
            TypeId = typeId;
        }
    }
}
