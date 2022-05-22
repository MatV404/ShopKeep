using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ShopKeepDB.Models;
using ShopKeepDB.Operations.Credentials;
using Type = ShopKeepDB.Models.Type;

namespace ShopKeepDB.Context
{
    public class ShopKeepContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Coins> Coins { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Type> Type { get; set; }

        public DbSet<ItemTypes> ItemTypes { get; set; }

        public DbSet<BaseItemPrice> BaseItemPrice { get; set; }

        public DbSet<Shop> Shop { get; set; }

        public DbSet<ShopStock> ShopStock { get; set; }
        public DbSet<ShopStockPrice> ShopStockPrice { get; set; }
        public DbSet<UserItem> UserItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(Misc.ConnectionStrings.GetConnectionString("ShopKeep"));


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(user => user.Name);
            modelBuilder.Entity<User>().HasMany(user => user.Items);

            modelBuilder.Entity<Coins>().HasKey(coins => coins.Id);
            

            modelBuilder.Entity<Item>().HasKey(item => item.Id);
            modelBuilder.Entity<Item>().HasOne(item => item.BaseItemPrice);
            modelBuilder.Entity<Item>().HasMany(item => item.ItemTypes).WithOne(itemtype => itemtype.Item);

            modelBuilder.Entity<Type>().HasKey(type => type.Id);
            
            modelBuilder.Entity<ItemTypes>().HasKey(itemType => new {itemType.TypeId, itemType.ItemId});
            modelBuilder.Entity<ItemTypes>().HasOne<Type>(itemType => itemType.Type);
            modelBuilder.Entity<ItemTypes>().HasOne<Item>(itemType => itemType.Item);

            modelBuilder.Entity<BaseItemPrice>().HasKey(basePrice => basePrice.Id);
            
            modelBuilder.Entity<Shop>().HasKey(shop => shop.Id);
            
            modelBuilder.Entity<ShopStock>().HasKey(stock => new {stock.ShopId, stock.ItemId});
            modelBuilder.Entity<ShopStock>().HasOne(stock => stock.ShopStockPrice);

            modelBuilder.Entity<ShopStockPrice>().HasKey(stockPrice => stockPrice.Id);
            
            modelBuilder.Entity<UserItem>().HasKey(userItem => new {userItem.ItemId, userItem.UserName});

            // Data Seeding //
            Tuple<string, string> adminPassTuple = Password.CreatePasswordHash("Admin");
            modelBuilder.Entity<User>().HasData(
                new User { Name = "Admin", Password = adminPassTuple.Item2, IsAdmin = true, IsActive = true, Salt = adminPassTuple.Item1});
            modelBuilder.Entity<Type>().HasData(
                new Type {Id = 1, Name = "Blacksmith"},
                new Type {Id = 2, Name = "Alchemy"}, 
                new Type {Id = 3, Name = "Bookshop"}, 
                new Type {Id = 4, Name = "General Goods"}, 
                new Type {Id = 5, Name = "Grocery Store"},
                new Type {Id = 6, Name = "Fletcher"},
                new Type {Id = 7, Name = "Magician's Supplies"},
                new Type {Id = 8, Name = "Tavern"});
        }
    }

}
