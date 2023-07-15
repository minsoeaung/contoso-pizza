using ContosoPizza.Data;
using ContosoPizza.Models;
using ContosoPizza.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ContosoContext _context;

        public CourseController(ContosoContext context)
        {
            _context = context;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetCourses()
        {
            return await _context.Courses
                .Select(c => new CourseViewModel()
                {
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name,
                    Title = c.Title,
                    Id = c.Id
                })
                .ToListAsync();
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseViewModel>> GetCourse(int id)
        {
            var course = await _context.Courses
                .Select(c => new CourseViewModel()
                {
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name,
                    Title = c.Title,
                    Id = c.Id
                })
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            return course;
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static CourseViewModel CourseToCourseViewModel(Course course) => new()
        {
            Id = course.Id,
            Title = course.Title,
            Credits = course.Credits,
            DepartmentName = course.Department.Name
        };
    }
}