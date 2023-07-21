namespace ContosoPizza.Models.Views;

public class CourseViewModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Credits { get; set; }
    public required string DepartmentName { get; set; }
}