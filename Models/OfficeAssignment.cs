using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class OfficeAssignment
{
    // use "InstructorId" as PK instead of it's own.
    // This is also is a FK to Instructor entity.
    [Key] public int InstructorId { get; set; }

    [StringLength(50)] public string Location { get; set; }

    public Instructor Instructor { get; set; }
}