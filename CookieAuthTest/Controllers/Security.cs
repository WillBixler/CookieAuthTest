using JwtTest_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtTest_backend.Controllers;

[Route("[controller]"), ApiController]
public class Security : ControllerBase
{
    private readonly IConfiguration _configuration;

    public Security(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("createToken"), AllowAnonymous]
    public IActionResult CreateToken(User user)
    {
        if (user is { UserName: "Test", Password: "User" })
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                SameSite = SameSiteMode.Strict,
                Secure = true,
                MaxAge = TimeSpan.FromDays(7),
            };
            Response.Cookies.Append("token", jwtToken, option);
            return Ok();
        }
        return Unauthorized();
    }
}
