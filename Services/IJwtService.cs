using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public interface IJwtService
{
    AuthenticationResult CreateToken(IdentityUser user, IList<string>? roles);
}