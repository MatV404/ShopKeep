using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace ShopKeepDB.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int? CoinsId { get; set; } = null;
        public Coins? Coins { get; set; } = null;
        public List<UserItem> Items { get; set; } = new List<UserItem>();

        public User(string username, string password)
        {
            this.Name = username;
            this.Password = password;
        }

        public User() { }
    }
}
