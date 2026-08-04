using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GarageLog.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GarageLog.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration) => _configuration = configuration;

    public string GenerateToken(int userId, string email, string firstName, string lastName)
    {
        IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");
        string secret = jwtSettings["Secret"]!;
        string issuer = jwtSettings["Issuer"]!;
        string audience = jwtSettings["Audience"]!;
        int expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"]!);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secret));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.GivenName, firstName),
            new(JwtRegisteredClaimNames.FamilyName, lastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ];

        JwtSecurityToken token = new(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
