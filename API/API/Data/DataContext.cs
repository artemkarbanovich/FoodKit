using API.Entities;
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
    public DbSet<OrderDishParameter> OrderDishParameters { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<DishIngredient> DishIngredients { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Message> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(au => au.MessagesSent);

        builder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(au => au.MessagesReceived);

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

        builder.Entity<DishIngredient>()
            .HasKey(di => new { di.DishId, di.IngredientId });

        builder.Entity<DishIngredient>()
            .HasOne(di => di.Dish)
            .WithMany(d => d.Ingredients)
            .HasForeignKey(di => di.DishId);

        builder.Entity<DishIngredient>()
          .HasOne(di => di.Ingredient)
          .WithMany(i => i.Dishes)
          .HasForeignKey(di => di.IngredientId);

        builder.Entity<Order>()
            .HasOne(o => o.Address)
            .WithMany(a => a.Orders)
            .OnDelete(DeleteBehavior.SetNull);
    }
}