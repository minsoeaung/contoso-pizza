using AutoMapper;
using ContosoPizza.Data;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly ContosoContext _context;
    private readonly IMapper _mapper;

    public StudentsController(ContosoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Student
    [HttpGet]
    public async Task<IEnumerable<StudentViewModel>> GetStudents()
    {
        var students = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<IEnumerable<StudentViewModel>>(students);
    }

    // GET: api/Student/5
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentViewModel>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        return _mapper.Map<StudentViewModel>(student);
    }

    // PUT: api/Student/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudent(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Student
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudent(StudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);
        var createdStudent = _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetStudent),
            new { id = createdStudent.Entity.Id },
            // TODO: enrollments should be empty array if null
            createdStudent.Entity
        );
    }

    // DELETE: api/Student/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var studentToDelete = await _context.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentToDelete == null)
        {
            return NotFound();
        }

        _context.Students.Remove(studentToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentExists(int id)
    {
        return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}