using Microsoft.Build.Framework;

namespace ContosoPizza.Models;

public class StudentDTO
{
    public int Id { get; set; }

    [Required] public string? FirstName { get; set; }

    public string? LastName { get; set; }
}