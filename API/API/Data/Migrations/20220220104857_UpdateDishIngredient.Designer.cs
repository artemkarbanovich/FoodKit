﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220220104857_UpdateDishIngredient")]
    partial class UpdateDishIngredient
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("API.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApartmentNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<string>("EntranceNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Floor")
                        .HasColumnType("TEXT");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Locality")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("API.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BodyHeight")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BodyWeight")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("PhysicalActivityCoefficient")
                        .HasColumnType("REAL");

                    b.Property<DateOnly>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("API.Entities.AppUserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("API.Entities.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("CookingTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAvailableForSingleOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("YouWillNeed")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("API.Entities.DishIngredient", b =>
                {
                    b.Property<int>("DishId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IngredientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IngredientWeightPerPortion")
                        .HasColumnType("INTEGER");

                    b.HasKey("DishId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("DishIngredients");
                });

            modelBuilder.Entity("API.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DishId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PublicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("API.Entities.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Carbohydrates")
                        .HasColumnType("REAL");

                    b.Property<float>("Fats")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<float>("Proteins")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("API.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateRead")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipientId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RecipientName")
                        .HasColumnType("TEXT");

                    b.Property<int>("SenderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SenderName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Evaluation")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("API.Entities.OrderDishParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DishId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfPersons")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDishParameters");
                });

            modelBuilder.Entity("API.Entities.OrderSetParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfMeals")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfPersons")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SetId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("SetId");

                    b.ToTable("OrderSetParameters");
                });

            modelBuilder.Entity("API.Entities.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAvailableForOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("PrivateName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("API.Entities.UserDish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Carbohydrates")
                        .HasColumnType("REAL");

                    b.Property<DateOnly>("DishDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("DishWeight")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Fats")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<float>("Proteins")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserDishes");
                });

            modelBuilder.Entity("DishIngredient", b =>
                {
                    b.Property<int>("DishesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IngredientsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DishesId", "IngredientsId");

                    b.HasIndex("IngredientsId");

                    b.ToTable("DishIngredient", (string)null);
                });

            modelBuilder.Entity("DishSet", b =>
                {
                    b.Property<int>("DishesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SetsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DishesId", "SetsId");

                    b.HasIndex("SetsId");

                    b.ToTable("SetDish", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("API.Entities.Address", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("Addresses")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.AppUserRole", b =>
                {
                    b.HasOne("API.Entities.AppRole", "AppRole")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppRole");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.DishIngredient", b =>
                {
                    b.HasOne("API.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Ingredient", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Ingredient");
                });

            modelBuilder.Entity("API.Entities.Image", b =>
                {
                    b.HasOne("API.Entities.Dish", "Dish")
                        .WithMany("Images")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("API.Entities.Message", b =>
                {
                    b.HasOne("API.Entities.AppUser", "Recipient")
                        .WithMany("MessagesReceived")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.AppUser", "Sender")
                        .WithMany("MessagesSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("API.Entities.OrderDishParameter", b =>
                {
                    b.HasOne("API.Entities.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Order", null)
                        .WithMany("OrderDishParameters")
                        .HasForeignKey("OrderId");

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("API.Entities.OrderSetParameter", b =>
                {
                    b.HasOne("API.Entities.Order", null)
                        .WithMany("OrderSetParameters")
                        .HasForeignKey("OrderId");

                    b.HasOne("API.Entities.Set", "Set")
                        .WithMany()
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("API.Entities.UserDish", b =>
                {
                    b.HasOne("API.Entities.AppUser", "AppUser")
                        .WithMany("UserDishes")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("DishIngredient", b =>
                {
                    b.HasOne("API.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Ingredient", null)
                        .WithMany()
                        .HasForeignKey("IngredientsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DishSet", b =>
                {
                    b.HasOne("API.Entities.Dish", null)
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Set", null)
                        .WithMany()
                        .HasForeignKey("SetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("API.Entities.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("API.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("API.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("API.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.AppRole", b =>
                {
                    b.Navigation("AppUserRoles");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("AppUserRoles");

                    b.Navigation("MessagesReceived");

                    b.Navigation("MessagesSent");

                    b.Navigation("Orders");

                    b.Navigation("UserDishes");
                });

            modelBuilder.Entity("API.Entities.Dish", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("API.Entities.Order", b =>
                {
                    b.Navigation("OrderDishParameters");

                    b.Navigation("OrderSetParameters");
                });
#pragma warning restore 612, 618
        }
    }
}
