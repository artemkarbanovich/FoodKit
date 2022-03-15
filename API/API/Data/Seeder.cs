using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seeder
{
    public static async Task SetBaseRoleConfiguration(RoleManager<AppRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<AppRole>
        {
            new AppRole { Name = "User" },
            new AppRole { Name = "Admin" },
            new AppRole { Name = "Courier" }
        };

        foreach (var role in roles)
            await roleManager.CreateAsync(role);
    }

    public static async Task AddAdminToDB(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync("Admin");

        if (await userManager.Users.AnyAsync(ap => ap.AppUserRoles.Any(aur => aur.RoleId == adminRole.Id))) 
            return;

        var admin = new AppUser
        {
            UserName = "375330000000",
            PhoneNumber = "+375330000000",
            Name = "Administrator",
            Email = "admin@food.kit",
            RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRoleAsync(admin, "Admin");
    }
}