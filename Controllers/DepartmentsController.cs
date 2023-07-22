using AutoMapper;
using ContosoPizza.Data;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly ContosoContext _context;
    private readonly IMapper _mapper;

    public DepartmentsController(ContosoContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var departments = await _context.Departments
            .Include(d => d.Administrator)
            .AsNoTracking()
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<DepartmentDto>>(departments));
    }

    // GET: api/Department/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var department = await _context.Departments
            .Include(d => d.Administrator)
            .Where(d => d.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return department is null ? NotFound() : _mapper.Map<DepartmentDto>(department);
    }
}