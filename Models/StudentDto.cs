using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class StudentDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string? FirstName { get; set; }

    [StringLength(50)] public string? LastName { get; set; }
}