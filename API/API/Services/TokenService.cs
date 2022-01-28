using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey key;
    private readonly UserManager<AppUser> userManager;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        this.userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser appUser)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.NameId, appUser.Id.ToString())
        };
        var roles = await userManager.GetRolesAsync(appUser);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}