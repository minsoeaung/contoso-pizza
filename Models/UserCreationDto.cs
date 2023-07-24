using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class UserCreationDto
{
    [Required] public string UserName { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Email { get; set; }
}