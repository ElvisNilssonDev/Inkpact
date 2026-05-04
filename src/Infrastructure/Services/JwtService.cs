using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config) => _config = config;

    public string GenerateToken(User user)
    {
        var settings = _config.GetSection("JwtSettings");
        var secretKey = settings["SecretKey"]
            ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
        var issuer = settings["Issuer"]
            ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        var audience = settings["Audience"]
            ?? throw new InvalidOperationException("JWT Audience is not configured.");
        var expirationMinutes = int.Parse(settings["ExpirationMinutes"] ?? "45");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Guid? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        var settings = _config.GetSection("JwtSettings");
        var secretKey = settings["SecretKey"];
        if (string.IsNullOrEmpty(secretKey)) return null;

        var key = Encoding.UTF8.GetBytes(secretKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = settings["Issuer"],
                ValidateAudience = true,
                ValidAudience = settings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwt.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            return Guid.Parse(userIdClaim);
        }
        catch
        {
            return null;
        }
    }
}
