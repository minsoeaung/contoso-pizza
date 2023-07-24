using Microsoft.Build.Framework;

namespace ContosoPizza.Models;

public class UserLoginDto
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}