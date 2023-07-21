using ContosoPizza.Models;
using ContosoPizza.Models.Views;
using ContosoPizza.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly CourseService _service;

    public CoursesController(CourseService service)
    {
        _service = service;
    }

    // GET: api/Course
    [HttpGet]
    public IEnumerable<CourseViewModel> GetCourses()
    {
        return _service.GetCourses();
    }

    // GET: api/Course/5
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CourseViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CourseViewModel> GetCourse(int id)
    {
        var result = _service.GetCourseViewModel(id);
        return result == null ? NotFound() : result;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<CourseDto> Create(CourseDto courseDto)
    {
        if (
            !_service.DepartmentExists(courseDto.DepartmentId) ||
            _service.CourseExists(courseDto.Id) ||
            courseDto.Credit is <= 0 or > 5 ||
            courseDto.Title.Trim().Length is < 2 or > 50
        )
            return BadRequest();

        _service.AddCourse(courseDto);
        _service.Save();

        return CreatedAtAction(
            nameof(GetCourse),
            new { id = courseDto.Id },
            courseDto
        );
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteCourse(int id)
    {
        var courseToDelete = _service.GetCourse(id);
        if (courseToDelete == null)
        {
            return NotFound();
        }

        _service.DeleteCourse(courseToDelete);
        _service.Save();

        return NoContent();
    }
}