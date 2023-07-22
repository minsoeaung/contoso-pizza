using Microsoft.Build.Framework;

namespace ContosoPizza.Models;

public class CourseDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Credit { get; set; }
    public int DepartmentId { get; set; }
}