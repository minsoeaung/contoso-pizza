using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models;

public class Department
{
    public int Id { get; set; }

    [StringLength(50)] public string Name { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "money")]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)] public DateTime StartDate { get; set; }

    // If "InstructorId" is not nullable, Department would be deleted when the instructor assigned is deleted.
    // Now it is prevented by specifying it is nullable.
    public int? InstructorId { get; set; }

    public Instructor Administrator { get; set; }

    public ICollection<Course> Courses { get; set; }
}