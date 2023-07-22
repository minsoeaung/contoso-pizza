namespace ContosoPizza.Models;

public class InstructorViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => FirstName + " " + LastName;

    public DateTime DateOfBirth { get; set; }
    public DateTime HireDate { get; set; }
    public string Office { get; set; }
}