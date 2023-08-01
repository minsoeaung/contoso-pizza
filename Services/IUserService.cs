using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public interface IUserService
{
    Task<IEnumerable<UserCreationResponseDto>> GetUsers();
    Task<UserCreationResponseDto?> GetUser(string username);

    Task<bool> ConfirmEmail(string userId, string token);

    Task<IdentityUser?> GetOrCreateExternalLoginUser(string provider, string key, string email, bool emailConfirmed);
}