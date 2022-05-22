﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShopKeepDB.Context;

namespace ShopKeepDB.Migrations
{
    [DbContext(typeof(ShopKeepContext))]
    [Migration("20220521195643_ShopKeepDB-Migration")]
    partial class ShopKeepDBMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ShopKeepDB.Models.BaseItemPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Copper")
                        .HasColumnType("integer");

                    b.Property<int>("Gold")
                        .HasColumnType("integer");

                    b.Property<int>("Silver")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("BaseItemPrice");
                });

            modelBuilder.Entity("ShopKeepDB.Models.Coins", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Copper")
                        .HasColumnType("integer");

                    b.Property<int>("Gold")
                        .HasColumnType("integer");

                    b.Property<int>("Silver")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("ShopKeepDB.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("BaseItemPriceId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Rarity")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BaseItemPriceId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("ShopKeepDB.Models.ItemTypes", b =>
                {
                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.HasKey("TypeId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("ShopKeepDB.Models.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Locale")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Owner")
                        .HasColumnType("text");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Shop");
                });

            modelBuilder.Entity("ShopKeepDB.Models.ShopStock", b =>
                {
                    b.Property<int>("ShopId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int>("ShopStockPriceId")
                        .HasColumnType("integer");

                    b.HasKey("ShopId", "ItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("ShopStockPriceId");

                    b.ToTable("ShopStock");
                });

            modelBuilder.Entity("ShopKeepDB.Models.ShopStockPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Copper")
                        .HasColumnType("integer");

                    b.Property<int>("Gold")
                        .HasColumnType("integer");

                    b.Property<int>("Silver")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ShopStockPrice");
                });

            modelBuilder.Entity("ShopKeepDB.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Type");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Blacksmith"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Alchemy"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Bookshop"
                        },
                        new
                        {
                            Id = 4,
                            Name = "General Goods"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Grocery Store"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Fletcher"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Magician's Supplies"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Tavern"
                        });
                });

            modelBuilder.Entity("ShopKeepDB.Models.User", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("CoinsId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Salt")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.HasIndex("CoinsId")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Name = "Admin",
                            IsActive = true,
                            IsAdmin = true,
                            Password = "hIzt9OGy39SF5xkVBOgZbf1UIV7XnEv6QpXcr4AHq4M=",
                            Salt = "BbaA+tc6d131iijAE0QSD2wnMIHVcJ0FsJ74NjW7oKk="
                        });
                });

            modelBuilder.Entity("ShopKeepDB.Models.UserItem", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.HasKey("ItemId", "UserName");

                    b.HasIndex("UserName");

                    b.ToTable("UserItem");
                });

            modelBuilder.Entity("ShopKeepDB.Models.Item", b =>
                {
                    b.HasOne("ShopKeepDB.Models.BaseItemPrice", "BaseItemPrice")
                        .WithMany()
                        .HasForeignKey("BaseItemPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopKeepDB.Models.ItemTypes", b =>
                {
                    b.HasOne("ShopKeepDB.Models.Item", "Item")
                        .WithMany("ItemTypes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopKeepDB.Models.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopKeepDB.Models.Shop", b =>
                {
                    b.HasOne("ShopKeepDB.Models.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopKeepDB.Models.ShopStock", b =>
                {
                    b.HasOne("ShopKeepDB.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopKeepDB.Models.Shop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopKeepDB.Models.ShopStockPrice", "ShopStockPrice")
                        .WithMany()
                        .HasForeignKey("ShopStockPriceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShopKeepDB.Models.User", b =>
                {
                    b.HasOne("ShopKeepDB.Models.Coins", "Coins")
                        .WithOne("User")
                        .HasForeignKey("ShopKeepDB.Models.User", "CoinsId");
                });

            modelBuilder.Entity("ShopKeepDB.Models.UserItem", b =>
                {
                    b.HasOne("ShopKeepDB.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShopKeepDB.Models.User", "User")
                        .WithMany("Items")
                        .HasForeignKey("UserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
