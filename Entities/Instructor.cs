using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Entities;

public class Instructor
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public required string FirstName { get; set; }

    [Required] [MaxLength(50)] public required string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    [Required] public DateTime DateOfBirth { get; set; }

    [Required] public DateTime HireDate { get; set; }

    public ICollection<Course> Courses { get; set; } = null!;

    public OfficeAssignment OfficeAssignment { get; set; } = null!;
}