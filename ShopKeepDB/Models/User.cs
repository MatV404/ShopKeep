using System.Collections.Generic;

namespace ShopKeepDB.Models
{
    public class User
    {
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public string Salt { get; set; } = null!;
        public int? CoinsId { get; set; } = null;
        public Coins Coins { get; set; } = null;
        public List<UserItem> Items { get; set; } = new List<UserItem>();

        public User(string username, string password, string salt)
        {
            Name = username;
            Password = password;
            Salt = salt;
        }

        public User() { }
    }
}
