using AutoMapper;
using ContosoPizza.Data;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstructorsController : ControllerBase
{
    private readonly ContosoContext _context;
    private readonly IMapper _mapper;

    public InstructorsController(ContosoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Instructor
    [HttpGet]
    public async Task<IEnumerable<InstructorViewModel>> GetInstructors()
    {
        var instructors = await _context.Instructors
            .Include(i => i.OfficeAssignment)
            .AsNoTracking()
            .ToListAsync();
        return _mapper.Map<IEnumerable<InstructorViewModel>>(instructors);
    }

    // GET: api/Instructor/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorViewModel>> GetInstructor(int id)
    {
        var instructor = await _context.Instructors
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
        return instructor is null ? NotFound() : _mapper.Map<InstructorViewModel>(instructor);
    }
}