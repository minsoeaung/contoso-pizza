using ContosoPizza.Authentication.ApiKey;
using ContosoPizza.Data;
using ContosoPizza.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CouponsController : ControllerBase
{
    private readonly ContosoContext _context;

    private const string AuthSchemes =
        $"{JwtBearerDefaults.AuthenticationScheme},{ApiKeyDefaults.AuthenticationScheme}";

    public CouponsController(ContosoContext context)
    {
        _context = context;
    }

    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [HttpGet]
    public IEnumerable<Coupon> Get()
    {
        return _context.Coupons
            .AsNoTracking()
            .ToList();
    }
}