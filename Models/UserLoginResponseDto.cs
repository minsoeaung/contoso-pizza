namespace ContosoPizza.Models;

public class UserLoginResponseDto
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}