using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ShopKeepDB.Models
{
    public class UserItem
    {
        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
