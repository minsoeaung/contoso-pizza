using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContosoPizza.Entities;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [MaxLength(50)] public required string Title { get; set; }

    [Range(0, 5)] public int Credits { get; set; }

    // FK
    [JsonIgnore] public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = null!;

    public ICollection<Instructor> Instructors { get; set; } = null!;
}