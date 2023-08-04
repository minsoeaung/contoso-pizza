namespace ContosoPizza.Models;

public class AuthenticationResult
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}