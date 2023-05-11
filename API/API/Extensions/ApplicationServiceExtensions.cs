using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Data;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;
using Stegomaster;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        services.AddSingleton<PresenceTracker>();
        services.AddDbContext<DataContext>(options =>
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string connStr = "";

            if (env == "Development")
            {
                connStr = config.GetConnectionString("DefaultConnection");
            }
            else
            {
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL").Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
            }

            options.UseNpgsql(connStr);
        });

        return services;
    }

    public static IServiceCollection AddStegomasterServices(this IServiceCollection services)
    {
        services.AddSingleton<IStegomasterService, StegomasterService>();

        return services;
    }
}