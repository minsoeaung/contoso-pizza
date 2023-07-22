namespace ContosoPizza.Models;

public class StudentViewModel
{
    public class StudentEnrollment
    {
        public int Id { get; set; }
        public EnrolledCourse Course { get; set; }
        public int Grade { get; set; }
    }

    public class EnrolledCourse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
    }

    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    public DateTime EnrollmentDate { get; set; }

    public ICollection<StudentEnrollment> Enrollments { get; set; }
}