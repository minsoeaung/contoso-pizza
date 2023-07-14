using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class Instructor
{
    public int Id { get; set; }

    [Required] [StringLength(50)] public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    [Required] public DateTime DateOfBirth { get; set; }

    [Required] public DateTime HireDate { get; set; }

    public ICollection<Course> Courses { get; set; }

    public OfficeAssignment OfficeAssignment { get; set; }
}