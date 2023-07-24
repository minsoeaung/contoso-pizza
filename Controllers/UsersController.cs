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
    private readonly IJwtService _jwtService;
    private readonly IApiKeyService _apiKeyService;

    public UsersController(UserManager<IdentityUser> userManager, IJwtService jwtService, IApiKeyService apiKeyService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _apiKeyService = apiKeyService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserCreationDto>> GetUser(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user is null
            ? NotFound()
            : new UserCreationDto() { UserName = user.UserName ?? "", Email = user.Email ?? "", Password = "" };
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
        if (!ModelState.IsValid) return BadRequest();

        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);

        if (user is null) return NotFound();

        var passwordValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

        if (!passwordValid) return BadRequest();

        return _jwtService.CreateToken(user);
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
}