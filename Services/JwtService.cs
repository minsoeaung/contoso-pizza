using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContosoPizza.Configurations;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContosoPizza.Services;

public class JwtService : IJwtService
{
    private const int ExpirationMinutes = 5;
    private readonly JwtConfig _jwtConfig;

    public JwtService(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
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
        new(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private static IEnumerable<Claim> CreateClaims(IdentityUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        claims.AddRange(roles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        return claims;
    }

    private SigningCredentials CreateSigningCredentials() =>
        new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
            SecurityAlgorithms.HmacSha256
        );
}