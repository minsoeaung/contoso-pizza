using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public interface IJwtService
{
    UserLoginResponseDto CreateToken(IdentityUser user, IList<string> roles);
}