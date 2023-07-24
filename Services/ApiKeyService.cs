using ContosoPizza.Data;
using ContosoPizza.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContosoPizza.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly ContosoContext _context;

    public ApiKeyService(ContosoContext context)
    {
        _context = context;
    }

    public UserApiKey AddApiKey(IdentityUser user)
    {
        var newApiKey = new UserApiKey()
        {
            User = user,
            Value = GenerateApiKey()
        };

        _context.UserApiKeys.Add(newApiKey);
        _context.SaveChanges();

        return newApiKey;
    }

    // Should generate real cryptographically secure random string in production
    // https://jonathancrozier.com/blog/how-to-generate-a-cryptographically-secure-random-string-in-dot-net-with-c-sharp
    private static string GenerateApiKey() =>
        $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}";
}