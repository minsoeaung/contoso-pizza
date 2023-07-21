using ContosoPizza.Entities;

namespace ContosoPizza.Models.Views;

public class InstructorViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;

    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public ICollection<Course> Courses { get; set; }

    public string office { get; set; }
}