using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ContosoPizza.Entities;

public class Department
{
    public int Id { get; set; }

    [MaxLength(50)] public required string Name { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)] public DateTime StartDate { get; set; }

    // If "InstructorId" is not nullable, Department would be deleted when the instructor assigned is deleted.
    // Now it is prevented by specifying it is nullable.
    [JsonIgnore] public int? InstructorId { get; set; }

    public Instructor Administrator { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = null!;
}