using ContosoPizza.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public interface IApiKeyService
{
    UserApiKey AddApiKey(IdentityUser user);
}