﻿using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
    AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options) { }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<UserDish> UserDishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderSetParameter> OrderSetParameters { get; set; }
    public DbSet<OrderDishParameter> OrderDishParameters { get; set; }
    public DbSet<Set> Sets { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Image> Images { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(m => m.MessagesSent);

        builder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(u => u.MessagesReceived);

        builder.Entity<AppUser>()
            .HasMany(au => au.AppUserRoles)
            .WithOne(aur => aur.AppUser)
            .HasForeignKey(aur => aur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ar => ar.AppUserRoles)
            .WithOne(aur => aur.AppRole)
            .HasForeignKey(aur => aur.RoleId)
            .IsRequired();

        builder.Entity<Set>()
            .HasMany(s => s.Dishes)
            .WithMany(d => d.Sets)
            .UsingEntity(etb => etb.ToTable("SetDish"));
    
        builder.Entity<Dish>()
            .HasMany(d => d.Ingredients)
            .WithMany(i => i.Dishes)
            .UsingEntity(etb => etb.ToTable("DishIngredient"));
    }
}