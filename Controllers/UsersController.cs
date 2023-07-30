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
    private readonly IMailService _mailService;
    private readonly IUserService _userService;

    public UsersController(UserManager<IdentityUser> userManager, IJwtService jwtService, IApiKeyService apiKeyService,
        RoleManager<IdentityRole> roleManager, IMailService mailService,
        IUserService userService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _apiKeyService = apiKeyService;
        _roleManager = roleManager;
        _mailService = mailService;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<UserCreationResponseDto>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    [HttpGet("{username}")]
    [ProducesResponseType(typeof(UserCreationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserCreationResponseDto>> GetUser(string username)
    {
        var user = await _userService.GetUser(username);
        return user is null ? NotFound() : user;
    }

    [HttpGet("confirm-email")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        var success = await _userService.ConfirmEmail(userId, token);
        return success ? Ok() : BadRequest();
    }

    [HttpPost("send-confirm-email-url")]
    public async Task<ActionResult> SendConfirmEmailLink(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return NotFound();

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _mailService.SendEmailAsync(
            new MailRequest()
            {
                ToEmail = user.Email ?? string.Empty,
                Subject = "Please confirm your email",
                Body =
                    Request.Scheme +
                    "://" +
                    Request.Host +
                    Url.Action(
                        nameof(ConfirmEmail),
                        "Users",
                        new { userId = user.Id, token }
                    )
            }
        );

        return Ok();
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(UserCreationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserCreationDto>> PostUser(UserCreationDto userCreationDto)
    {
        var newUser = new IdentityUser() { UserName = userCreationDto.UserName, Email = userCreationDto.Email };

        var result = await _userManager.CreateAsync(newUser, userCreationDto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

        await _mailService.SendEmailAsync(
            new MailRequest()
            {
                ToEmail = userCreationDto.Email,
                Subject = "Please confirm your email",
                Body =
                    Request.Scheme +
                    "://" +
                    Request.Host +
                    Url.Action(
                        nameof(ConfirmEmail),
                        "Users",
                        new { userId = newUser.Id, token }
                    )
            }
        );

        userCreationDto.Password = string.Empty;

        return CreatedAtAction(nameof(GetUser), new { username = userCreationDto.UserName }, userCreationDto);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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