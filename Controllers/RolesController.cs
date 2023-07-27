using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IEnumerable<IdentityRole>> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<IdentityRole>> GetRole(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest();

        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == name);
        return role is null ? NotFound() : role;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IdentityRole), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IdentityRole>> CreateRole(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest();

        if (await _roleManager.RoleExistsAsync(name))
            return Conflict();

        await _roleManager.CreateAsync(new IdentityRole(name));
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == name);
        return CreatedAtAction(nameof(GetRole), new { name }, role);
    }
}

