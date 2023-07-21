using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Entities;

public class Student
{
    public int Id { get; set; }

    [Required] [StringLength(50)] public required string FirstName { get; set; }

    [Required] [StringLength(50)] public required string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    public DateTime EnrollmentDate { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = null!;
}