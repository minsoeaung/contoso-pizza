using AutoMapper;
using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class UserService : IUserService
{
    private readonly ContosoContext _contosoContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;

    public UserService(ContosoContext contosoContext, UserManager<IdentityUser> userManager, IMapper mapper,
        IMailService mailService)
    {
        _contosoContext = contosoContext;
        _userManager = userManager;
        _mapper = mapper;
        _mailService = mailService;
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

    public async Task<bool> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
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
}