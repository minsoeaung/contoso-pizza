using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ContosoPizza.Services;

public class JwtService : IJwtService
{
    private const int ExpirationMinutes = 5;
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public UserLoginResponseDto CreateToken(IdentityUser user, IList<string> roles)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

        var token = CreateJwtToken(
            CreateClaims(user, roles),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new UserLoginResponseDto
        {
            AccessToken = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private static IEnumerable<Claim> CreateClaims(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        claims.AddRange(roles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        return claims;
    }

    private SigningCredentials CreateSigningCredentials() =>
        new(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("Jwt key missing!")
                )
            ),
            SecurityAlgorithms.HmacSha256
        );
}