namespace ContosoPizza.Entities;

// Many-to-many join table with payload for student and course.
public class Enrollment
{
    public int Id { get; set; }

    // FK
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    public Grade? Grade { get; set; }

    // Navigation property
    // Navigation property is should be non-nullable.
    // An empty collection means that no relatable entity exists, 
    // but the list itself should never be null.
    public Course Course { get; set; } = null!;
    public Student Student { get; set; } = null!;
}

public enum Grade
{
    A,
    B,
    C,
    D,
    F
}