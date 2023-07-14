using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [StringLength(50)] public string Title { get; set; }

    [Range(0, 5)] public int Credits { get; set; }

    // FK
    public int DepartmentId { get; set; }

    public Department Department { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; }

    public ICollection<Instructor> Instructors { get; set; }
}