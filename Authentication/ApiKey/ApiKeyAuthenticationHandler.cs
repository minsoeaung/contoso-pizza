using System.Security.Claims;
using System.Text.Encodings.Web;
using ContosoPizza.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContosoPizza.Authentication.ApiKey;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeader = "Api-Key";
    private readonly ContosoContext _context;

    public ApiKeyAuthenticationHandler(
        ContosoContext context,
        IOptionsMonitor<AuthenticationSchemeOptions> optionsMonitor,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(optionsMonitor, loggerFactory, encoder, clock)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(ApiKeyHeader))
            return AuthenticateResult.Fail("Header Not Found!");

        string? apiKeyToValidate = Request.Headers[ApiKeyHeader];

        var apiKey = await _context.UserApiKeys
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Value == apiKeyToValidate);

        return apiKey is null
            ? AuthenticateResult.Fail("Invalid Key!")
            : AuthenticateResult.Success(CreateTicket(apiKey.User));
    }

    private AuthenticationTicket CreateTicket(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return ticket;
    }
}