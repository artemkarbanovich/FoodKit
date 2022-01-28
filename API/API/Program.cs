using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddScoped<ITokenService, TokenService>();
services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(config.GetConnectionString("DefaultConnection"));
});
services.AddControllers();
services.AddCors();
services.AddIdentityCore<AppUser>(opt => { opt.Password.RequireNonAlphanumeric = false; })
           .AddRoles<AppRole>()
           .AddRoleManager<RoleManager<AppRole>>()
           .AddSignInManager<SignInManager<AppUser>>()
           .AddRoleValidator<RoleValidator<AppRole>>()
           .AddEntityFrameworkStores<DataContext>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
services.AddAuthorization(opt =>
{
    opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("RequireCourierRole", policy => policy.RequireRole("Admin", "Courier"));
});


var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    
    try
    {
        var context = serviceProvider.GetRequiredService<DataContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        await context.Database.MigrateAsync();
    }
    catch(Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();