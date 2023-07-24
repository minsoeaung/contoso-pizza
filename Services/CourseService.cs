using ContosoPizza.Data;
using ContosoPizza.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class CourseService : ICourseService
{
    private readonly ContosoContext _context;

    public CourseService(ContosoContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Course>, PaginationMetaData)> GetCourses(int pageNumber, int pageSize)
    {
        var totalItemCount = await _context.Courses.CountAsync();
        var paginationMetaData = new PaginationMetaData(totalItemCount, pageNumber, pageSize);
        var courses = await _context.Courses
            .Include(c => c.Department)
            .OrderBy(c => c.Title)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        return (
            courses,
            paginationMetaData
        );
    }

    public async Task<Course?> GetCourse(int id)
    {
        var course = await _context.Courses
            .Where(c => c.Id == id)
            .Include(c => c.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return course;
    }

    public void AddCourse(Course course)
    {
        _context.Courses.Add(course);
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