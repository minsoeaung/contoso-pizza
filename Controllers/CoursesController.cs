using System.Text.Json;
using AutoMapper;
using ContosoPizza.Entities;
using ContosoPizza.Models;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseService _service;
    private readonly IMapper _mapper;

    public CoursesController(CourseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CourseViewModel>> GetCourses(int pageNumber = 1, int pageSize = 20)
    {
        var (courses, paginationMetaData) = await _service.GetCourses(pageNumber, pageSize);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
        return _mapper.Map<IEnumerable<CourseViewModel>>(courses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseViewModel>> GetCourse(int id)
    {
        var result = await _service.GetCourse(id);
        return result == null ? NotFound() : _mapper.Map<CourseViewModel>(result);
    }

    [HttpPost]
    public ActionResult<Course> Create(CourseDto courseDto)
    {
        if (
            !_service.DepartmentExists(courseDto.DepartmentId) ||
            _service.CourseExists(courseDto.Id) ||
            courseDto.Credit is <= 0 or > 5 ||
            courseDto.Title.Trim().Length is < 2 or > 50
        )
            return BadRequest();

        var course = _mapper.Map<Course>(courseDto);
        _service.AddCourse(course);
        _service.Save();

        return CreatedAtAction(
            nameof(GetCourse),
            new { id = courseDto.Id },
            course
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(int id)
    {
        var courseToDelete = await _service.GetCourse(id);
        if (courseToDelete == null)
        {
            return NotFound();
        }

        _service.DeleteCourse(courseToDelete);
        _service.Save();

        return NoContent();
    }
}