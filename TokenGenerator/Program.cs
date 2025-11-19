using DatabaseLibrary.DTOs;
using DatabaseLibrary.Models;
using DatabaseLibrary.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

CinemaUser user = new CinemaUser()
{
    Login = "user",
    UserId = 1,
};
string roleName = "Пользователь";

var token = await GenerateTokenAsync(user, roleName);
Console.WriteLine(token.Token);

async Task<TokenResponse> GenerateTokenAsync(CinemaUser user, string roleName)
{
    int minutes = 15;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.SecretKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new Claim[]
    {
                new ("id", user.UserId.ToString()),
                new ("login", user.Login),
                new ("role", roleName),
    };

    var token = new JwtSecurityToken(signingCredentials: credentials,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(minutes),
        issuer: AuthOptions.Issuer,
        audience: AuthOptions.Audience);

    return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), null);
}