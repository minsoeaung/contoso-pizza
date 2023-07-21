using ContosoPizza.Data;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using ContosoPizza.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly ContosoContext _context;

    public DepartmentsController(ContosoContext context)
    {
        _context = context;
    }

    // GET: api/Department
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
    {
        var departmentsQuery =
            from d in _context.Departments
            orderby d.Name
            select new DepartmentDto { Id = d.Id, Name = d.Name };

        return await departmentsQuery.ToListAsync();
    }

    // GET: api/Department/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
    {
        var departmentQuery =
            from d in _context.Departments
            select new DepartmentDto { Id = d.Id, Name = d.Name };
        var result = await departmentQuery.FirstOrDefaultAsync(d => d.Id == id);
        return result is not null ? result : NotFound();
    }

    private bool DepartmentExists(int id)
    {
        return (_context.Departments?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    private static DepartmentDto DepartmentToDto(Department department)
    {
        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
        };
    }
}