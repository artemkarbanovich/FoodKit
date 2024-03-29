using API.Extensions;
using API.Helpers.Json;
using API.Middleware;
using API.SignalR;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddApplicationServices(config);
services.AddStegomasterServices();
services.AddIdentityServices(config);
services.AddSignalR();
services.AddCors();
services.AddControllers().AddJsonOptions(jo => 
{
    jo.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    jo.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
});


var app = builder.Build();

app.UseMiddleware<GlobalErrorHandler>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapFallbackToController("Index", "Fallback");
await app.ConfigureDatabaseAsync();
await app.RunAsync();