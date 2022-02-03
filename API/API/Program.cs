using API.Extensions;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddApplicationServices(config);
services.AddIdentityServices(config);
services.AddControllers();
services.AddCors();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.ConfigureDatabaseAsync();
await app.RunAsync();