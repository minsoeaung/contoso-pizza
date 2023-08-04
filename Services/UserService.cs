using System.Text.Json;
using AutoMapper;
using ContosoPizza.Configurations;
using ContosoPizza.Data;
using ContosoPizza.External.Contracts;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContosoPizza.Services;

public class UserService : IUserService
{
    private readonly ContosoContext _contosoContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly FacebookConfig _facebookConfig;
    private readonly IHttpClientFactory _httpClientFactory;

    private const string FacebookTokenValidationUrl =
        "https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}";

    private const string FacebookUserInfoUrl =
        "https://graph.facebook.com/me?fields=first_name,last_name,picture,email&access_token={0}";

    public UserService(ContosoContext contosoContext, UserManager<IdentityUser> userManager, IMapper mapper,
        IOptions<FacebookConfig> facebookConfig, IHttpClientFactory httpClientFactory)
    {
        _contosoContext = contosoContext;
        _userManager = userManager;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
        _facebookConfig = facebookConfig.Value;
    }

    public async Task<IEnumerable<UserCreationResponseDto>> GetUsers()
    {
        var usersWithRolesQuery =
            from user in _contosoContext.Users
            orderby user.UserName
            select new UserCreationResponseDto()
            {
                Email = user.Email,
                UserName = user.UserName,
                Id = user.Id,
                EmailConfirmed = user.EmailConfirmed,
                roles = (
                    from role in _contosoContext.Roles
                    join userRole in _contosoContext.UserRoles on role.Id equals userRole.RoleId
                    where user.Id == userRole.UserId
                    select role.Name
                ).ToList()
            };

        return await usersWithRolesQuery.ToListAsync();
    }

    public async Task<UserCreationResponseDto?> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        var userWithRoles = _mapper.Map<UserCreationResponseDto>(user);
        userWithRoles.roles = roles;

        return userWithRoles;
    }

    public async Task<IdentityUser?> GetOrCreateExternalLoginUser(
        string provider,
        string key,
        string email,
        bool emailConfirmed
    )
    {
        var user = await _userManager.FindByLoginAsync(provider, key);
        if (user is not null)
        {
            return user;
        }

        user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new IdentityUser
            {
                Email = email,
                UserName = email,
                EmailConfirmed = emailConfirmed
            };

            await _userManager.CreateAsync(user);
        }

        // Link
        var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());
        var result = await _userManager.AddLoginAsync(user, info);

        return result.Succeeded ? user : null;
    }

    public async Task<FacebookTokenValidationResult?> ValidateFacebookAccessTokenAsync(string accessToken)
    {
        var formattedUrl = string.Format(FacebookTokenValidationUrl, accessToken, _facebookConfig.AppId,
            _facebookConfig.AppSecret);

        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();

        var responseAsString = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FacebookTokenValidationResult>(responseAsString);
    }

    public async Task<FacebookUserInfoResult?> GetFacebookUserInfoAsync(string accessToken)
    {
        var formattedUrl = string.Format(FacebookUserInfoUrl, accessToken);

        var result = await _httpClientFactory.CreateClient().GetAsync(formattedUrl);
        result.EnsureSuccessStatusCode();

        var responseAsString = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<FacebookUserInfoResult>(responseAsString);
    }
}