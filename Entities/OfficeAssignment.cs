using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Entities;

public class OfficeAssignment
{
    // use "InstructorId" as PK instead of it's own.
    // This is also is a FK to Instructor entity.
    [Key] public int InstructorId { get; set; }

    [Required] [MaxLength(50)] public required string Location { get; set; }

    public Instructor Instructor { get; set; } = null!;
}