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
}