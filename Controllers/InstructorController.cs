using ContosoPizza.Data;
using ContosoPizza.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ContosoContext _context;

        public InstructorController(ContosoContext context)
        {
            _context = context;
        }

        // GET: api/Instructor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstructorViewModel>>> GetInstructors()
        {
            return await _context.Instructors
                .Select(i => new InstructorViewModel
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    DateOfBirth = i.DateOfBirth,
                    HireDate = i.HireDate,
                    office = i.OfficeAssignment.Location ?? "",
                    Courses = i.Courses
                })
                .ToListAsync();
        }

        // GET: api/Instructor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorViewModel>> GetInstructor(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.Courses)
                .ThenInclude(c => c.Department)
                .Select(i => new InstructorViewModel
                {
                    Id = i.Id,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    DateOfBirth = i.DateOfBirth,
                    HireDate = i.HireDate,
                    office = i.OfficeAssignment.Location ?? "",
                    Courses = i.Courses
                })
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return instructor;
        }

        private bool InstructorExists(int id)
        {
            return (_context.Instructors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}