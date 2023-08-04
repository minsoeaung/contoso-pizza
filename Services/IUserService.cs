using ContosoPizza.External.Contracts;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public interface IUserService
{
    Task<IEnumerable<UserCreationResponseDto>> GetUsers();
    Task<UserCreationResponseDto?> GetUser(string username);

    Task<IdentityUser?> GetOrCreateExternalLoginUser(string provider, string key, string email, bool emailConfirmed);

    Task<FacebookTokenValidationResult?> ValidateFacebookAccessTokenAsync(string accessToken);

    Task<FacebookUserInfoResult?> GetFacebookUserInfoAsync(string accessToken);
}