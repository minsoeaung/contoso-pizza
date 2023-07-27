using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;
    private readonly IApiKeyService _apiKeyService;
    private readonly IMapper _mapper;

    public UsersController(UserManager<IdentityUser> userManager, IJwtService jwtService, IApiKeyService apiKeyService,
        RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _apiKeyService = apiKeyService;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserCreationResponseDto>> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        var userWithRoles = _mapper.Map<UserCreationResponseDto>(user);
        userWithRoles.roles = roles;

        return userWithRoles;
    }

    [HttpPost("signup")]
    public async Task<ActionResult<UserCreationDto>> PostUser(UserCreationDto userCreationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userManager.CreateAsync(
            new IdentityUser() { UserName = userCreationDto.UserName, Email = userCreationDto.Email },
            userCreationDto.Password
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        userCreationDto.Password = string.Empty;

        return CreatedAtAction(nameof(GetUser), new { username = userCreationDto.UserName }, userCreationDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponseDto>> Login(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        if (user is null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var passwordValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!passwordValid)
            return BadRequest();

        return _jwtService.CreateToken(user, roles);
    }

    [HttpPost("api-key")]
    public async Task<ActionResult<UserApiKey>> CreateApiKey(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        if (user is null) return NotFound();

        var passwordValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!passwordValid) return BadRequest();

        return _apiKeyService.AddApiKey(user);
    }

    [HttpPut("assign-role")]
    public async Task<ActionResult> AssignRole(string username, string roleName)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return NotFound();

        var role = await _roleManager.FindByNameAsync(roleName);
        if (role is null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(role.Name))
            return BadRequest();

        await _userManager.AddToRoleAsync(user, role.Name);
        return NoContent();
    }
}