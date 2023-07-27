namespace ContosoPizza.Models;

public class UserCreationResponseDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public ICollection<string> roles { get; set; }
}