using ContosoPizza.Data;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using ContosoPizza.Models.Views;

namespace ContosoPizza.Services;

public class CourseService
{
    private readonly ContosoContext _context;


    public CourseService(ContosoContext context)
    {
        _context = context;
    }

    public IEnumerable<CourseViewModel> GetCourses()
    {
        var courses =
            from course in _context.Courses
            orderby course.Title
            select new CourseViewModel()
            {
                Title = course.Title,
                DepartmentName = course.Department.Name,
                Credits = course.Credits,
                Id = course.Id
            };
        return courses;
    }

    public CourseViewModel? GetCourseViewModel(int id)
    {
        var courses =
            from course in _context.Courses
            where course.Id == id
            select new CourseViewModel()
            {
                Title = course.Title,
                DepartmentName = course.Department.Name,
                Credits = course.Credits,
                Id = course.Id
            };
        return courses.FirstOrDefault();
    }

    public Course? GetCourse(int id)
    {
        var courses =
            from course in _context.Courses
            where course.Id == id
            select course;
        return courses.FirstOrDefault();
    }

    public void AddCourse(CourseDto course)
    {
        var newCourse = new Course()
        {
            Title = course.Title.Trim(),
            Credits = course.Credit,
            Id = course.Id,
            DepartmentId = course.DepartmentId
        };
        _context.Courses.Add(newCourse);
    }

    public void DeleteCourse(Course course)
    {
        _context.Courses.Remove(course);
    }

    public bool Save()
    {
        return (_context.SaveChanges() >= 0);
    }

    public bool DepartmentExists(int id)
    {
        return _context.Departments.Any(d => d.Id == id);
    }

    public bool CourseExists(int id)
    {
        return _context.Courses.Any(c => c.Id == id);
    }
}