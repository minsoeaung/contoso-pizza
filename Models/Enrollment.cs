using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

// Many-to-many join table with payload for student and course.
public class Enrollment
{
    public int Id { get; set; }

    // FK
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    [DisplayFormat(NullDisplayText = "No grade")]
    public Grade? Grade { get; set; }

    // Navigation property
    public Course Course { get; set; }
    public Student Student { get; set; }
}

public enum Grade
{
    A,
    B,
    C,
    D,
    F
}